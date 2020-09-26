using System.Collections;
using UnityEngine;

public class CharacterController : MonoBehaviour, IEntity
{
    public bool IsLocalController;

    protected bool isChampionInitialized;

    public Animator Anim;
    protected Champion currChampion = new Characters.AeiaeiScript();

    protected int segments = 50;
    protected float radius = 2;
    protected LineRenderer line;

    void Start()
    {
        line = gameObject.GetComponent<LineRenderer>();
        line.SetVertexCount(segments + 1);
        line.useWorldSpace = false;

        Anim = this.GetComponent<Animator>();
    }

    void CreatePoints()
    {
        float x,z;
        float angle = 20f;

        for (int i = 0; i < (segments + 1); i++)
        {
            x = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;
            z = Mathf.Cos(Mathf.Deg2Rad * angle) * radius;

            line.SetPosition(i, new Vector3(x, 0.5f, z));
            angle += (360f / segments);
        }
    }

    public Entity GetEntity() => currChampion as Champion;
    
    void Update()
    {
        if (currChampion != null)
        {
            if (!isChampionInitialized)
            {
                currChampion.charactercontroller = this;
                currChampion.OnStart();
                isChampionInitialized = true;
            }

            if (IsLocalController)
            {
                currChampion.OnUpdate();
                radius = currChampion.AttackRange;
                CreatePoints();
            }
        }
    }
}

public abstract class Champion : Entity
{
    //Property
    public Vector3 MoveTarget
    {
        get => movetarget;
        protected set
        {
            movetarget = value;
            IsMoving = true;
            IsAttacking = false;
            CurrentAnimation = AnimState.Movement;
        }
    }

    public GameObject AttackTarget
    {
        get => attacktarget;
        protected set
        {
            attacktarget = value;
            IsMoving = false;
            if (value != null)
            {
                CurrentAnimation = AnimState.Movement;
                IsAttacking = true;
            }
        }
    }

    //Odwołania property

    private Vector3 movetarget;
    private GameObject attacktarget;

    //Fieldy

    public CharacterController charactercontroller { get; set; }

    public AttackType AttackMode { get; protected set; }
    public AnimState CurrentAnimation { get; protected set; }

    public bool IsMoving { get; protected set; }
    public bool IsAttacking { get; protected set; }
    public bool AnimationRun { get; protected set; }
    public bool AACooldown { get; private set; }
    public bool NextAACrit { get; private set; }
    public bool WaitForResurrect { get; private set; }

    private Vector3 targetDirection;
    private Vector3 newDirection;

    public virtual void OnStart()
    {
        IsMoving = false;
        IsAttacking = false;
        AnimationRun = false;
        WaitForResurrect = false;
        CalculateCritChance();
        MaxHealth = 100;
        MovementSpeed = 340;
        AttackRange = 10f;
        LoadBasicStats();

        base.SetEntityObject(charactercontroller.gameObject);
    }

    public void OnUpdate()
    {
        if (!WaitForResurrect)
        {
            this.Animations();
            if (!AnimationRun)
            {
                if (Input.GetMouseButtonDown(1) && Cursor.HitObject.transform?.GetComponent<IEntity>().GetEntity() is Entity)
                {
                    AttackMove(Cursor.HitObject.transform.gameObject);
                    Debug.Log("Ta");
                }
                else if (Input.GetMouseButtonDown(1))
                {
                    MoveTo(Cursor.WorldPointer);
                    Debug.Log("Ta2");
                }

                if (IsAttacking)
                {
                    CurrentAnimation = AnimState.Movement;

                    Vector3 _attacktarget = AttackTarget.transform.position;
                    _attacktarget.y = 0.5f;
                    targetDirection = _attacktarget - charactercontroller.transform.position;
                    float singleStep = 10 * Time.deltaTime;
                    newDirection = Vector3.RotateTowards(charactercontroller.transform.forward, targetDirection, singleStep, 0.0f);

                    charactercontroller.transform.rotation = Quaternion.LookRotation(newDirection);
                }
                else if (IsMoving)
                {
                    CurrentAnimation = AnimState.Movement;
                    targetDirection = MoveTarget - charactercontroller.transform.position;
                    float singleStep = 10 * Time.deltaTime;
                    newDirection = Vector3.RotateTowards(charactercontroller.transform.forward, targetDirection, singleStep, 0.0f);

                    charactercontroller.transform.rotation = Quaternion.LookRotation(newDirection);
                }
                else if (CurrentAnimation != AnimState.BasicAttack)
                    CurrentAnimation = AnimState.Stay;
            }

            if (IsAttacking && AttackTarget != null)
            {
                if (Vector3.Distance(charactercontroller.transform.position, AttackTarget.transform.position) < AttackRange)
                {
                    Vector3 _attacktarget = AttackTarget.transform.position;
                    _attacktarget.y = 0.5f;
                    charactercontroller.transform.LookAt(_attacktarget);
                    DoBasicAttack(AttackTarget);
                    IsAttacking = false;
                }
                else
                {
                    Vector3 _attacktarget = AttackTarget.transform.position;
                    _attacktarget.y = 0.5f;
                    charactercontroller.transform.position = Vector3.MoveTowards(charactercontroller.transform.position, _attacktarget, (MovementSpeed * Time.deltaTime) / 40);// / 30000);
                }
            }
            else if (IsMoving && MoveTarget != Vector3.zero)
            {
                if (Vector3.Distance(charactercontroller.transform.position, MoveTarget) < 0.1f)
                    IsMoving = false;
                else
                    charactercontroller.transform.position = Vector3.MoveTowards(charactercontroller.transform.position, MoveTarget, (MovementSpeed * Time.deltaTime) / 40);// / 30000);
            }

            if (Input.GetKeyDown(KeyCode.R))
                Debug.Log($"{Vector3.Distance(charactercontroller.transform.position, Cursor.WorldPointer)},{Cursor.HitObject.collider?.name}");
        }
    }

    public IEnumerator AATimerCooldown(float Time)
    {
        AACooldown = true;
        CalculateCritChance();
        yield return new WaitForSeconds(Time);
        Debug.Log("End");
        AACooldown = false;
    }

    public void CalculateCritChance()
    {
        if (CriticalChance == 0)
            NextAACrit = false;
        else
        {
            int r = Random.Range(1, 100);
            if (r <= CriticalChance)
                NextAACrit = true;
            else
                NextAACrit = false;
        }
    }

    public abstract void LoadBasicStats();

    public abstract void Animations();

    public override void Heal(float heal)
    {
        throw new System.NotImplementedException();
    }

    public override void Die()
    {
        charactercontroller.transform.localRotation = new Quaternion(90, 0, 0, 0);
    }

    public override void SwitchAttack()
    {
        throw new System.NotImplementedException();
    }

    public override void Resurrect()
    {
        Health = MaxHealth;
        Mana = MaxMana;
        WaitForResurrect = false;
    }

    public virtual void MoveTo(Vector3 Point) => MoveTarget = Point;

    public virtual void AttackMove(GameObject Target) => AttackTarget = Target;

    public virtual void DoBasicAttack(GameObject Target) => CurrentAnimation = AnimState.BasicAttack;

    public virtual void Attack(GameObject Target, bool IsCrit)
    {
        float Damage = AttackDamage;
        if (IsCrit)
            Damage *= 2;

        charactercontroller.StartCoroutine(CalculateDamage(Target, DamageType.TrueDamage, Damage,IsCrit));
    }

    public IEnumerator CalculateDamage(GameObject Target, DamageType Type, float Damage, bool IsCrit)
    {
        yield return new WaitForSeconds((1 / AttackSpeed) / 3);
        Target?.GetComponent<CharacterController>().GetEntity().DoDamage(Damage,Type,IsCrit);
    }
}

public struct BasicAttack
{
    GameObject Prefab;
    float Speed;
}

public enum AttackType
{
    Melee,
    Ranged,
    Switched
}

public enum AnimState
{
    Stay = 0,
    Movement = 1,
    BasicAttack = 2,
    QAbility = 3,
    WAbility = 4,
    EAbility = 5,
    RAbility = 6,
    Recall = 7,
    Taunt1 = 8,
    Taunt2 = 9,
    Taunt3 = 10,
    Taunt4 = 11,
    Die = 12,
}

public enum DamageType
{
    AP,
    AD,
    TrueDamage
}