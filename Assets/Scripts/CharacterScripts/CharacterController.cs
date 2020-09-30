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
        get => moveTarget;
        protected set
        {
            moveTarget = value;
            IsMoving = true;
            IsAttacking = false;
            CurrentAnimation = AnimState.Movement;
        }
    }

    public Vector3 AttackTarget
    {
        get
        {
            Vector3 V = attackTarget.transform.position;
            V.y = 0.5f;
            return V;
        }
    }

    public GameObject AttackTargetSet
    {
        get => attackTarget;
        protected set
        {
            attackTarget = value;
            IsMoving = false;
            if (value != null)
            {
                CurrentAnimation = AnimState.Movement;
                IsAttacking = true;
            }
        }
    }

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

    public bool canUseFirstAbility { get; private set; }
    public bool canUseSecondAbility { get; private set; }
    public bool canUseThirdAbility { get; private set; }
    public bool canUseUltimate { get; private set; }

    public float firstAbilityCDCap { get; protected set; }
    public float secondAbilityCDCap { get; protected set; }
    public float thirdAbilityCDCap { get; protected set; }
    public float UltimateCDCap { get; protected set; }

    public bool CancelNextAutoattack { get; protected set; }

    public float RessurectTimer { get; private set; }

    //Prywatne lokalne zmienne

    private bool debug = false;
    private Vector3 moveTarget;
    private GameObject attackTarget;

    private Vector3 targetDirection;
    private Vector3 newDirection;

    private float firstAbilityCD;
    private float secondAbilityCD;
    private float thirdAbilityCD;
    private float UltimateCD;

    private float seconds;

    public override void OnStart()
    {
        IsMoving = false;
        IsAttacking = false;
        AnimationRun = false;
        WaitForResurrect = false;
        CalculateCritChance();

        MaxHealth = 2000;
        MovementSpeed = 340;
        AttackRange = 10f;

        if(charactercontroller.IsLocalController)
            LoadBasicStats();

        Health = MaxHealth;
        Mana = MaxMana;

        Untargetable = false;

        base.SetEntityObject(charactercontroller.gameObject);
    }

    public override void OnUpdate()
    {
        if (!WaitForResurrect) // Wszystko w czasie życia
        {
            this.Animations();
            if (!AnimationRun)
            {
                if (Input.GetMouseButtonDown(1))
                {
                    var entity = Cursor.HitObject.transform?.GetComponent<IEntity>().GetEntity();
                    if (entity != null && !entity.Untargetable)
                    {
                        AttackMove(Cursor.HitObject.transform.gameObject);
                        Debug.Log("Ta");
                    }
                    else
                    {
                        MoveTo(Cursor.WorldPointer);
                        Debug.Log("Ta2");
                    }
                }

                if (IsAttacking)
                {
                    CurrentAnimation = AnimState.Movement;

                    targetDirection = AttackTarget - charactercontroller.transform.position;
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

            if (IsAttacking && AttackTarget != null && CurrentAnimation != AnimState.BasicAttack)
            {
                if (Vector3.Distance(charactercontroller.transform.position, AttackTarget) <= AttackRange)
                {
                    charactercontroller.transform.LookAt(AttackTarget);
                    DoBasicAttack();
                    IsAttacking = false;
                }
                else
                    charactercontroller.transform.position = Vector3.MoveTowards(charactercontroller.transform.position, AttackTarget, (MovementSpeed * Time.deltaTime) / 40);// / 30000);
            }
            else if (IsMoving && MoveTarget != Vector3.zero)
            {
                if (Vector3.Distance(charactercontroller.transform.position, MoveTarget) < 0.1f)
                    IsMoving = false;
                else
                    charactercontroller.transform.position = Vector3.MoveTowards(charactercontroller.transform.position, MoveTarget, (MovementSpeed * Time.deltaTime) / 40);// / 30000);
            }

            if (AttackTargetSet != null)
            {
                if (!CancelNextAutoattack && CurrentAnimation == AnimState.BasicAttack && Vector3.Distance(charactercontroller.transform.position, AttackTarget) > AttackRange || AttackTargetSet.GetComponent<IEntity>().GetEntity().Untargetable)
                    CancelNextAutoattack = true;
            }

            if(debug)
                Debug.Log($"Animacja: {CurrentAnimation}, Atakowanie: {IsAttacking}, Poruszanie: {IsMoving}, Target: {MoveTarget}");

            if (Input.GetKeyDown(KeyCode.Z))
                Debug.Log($"{Vector3.Distance(charactercontroller.transform.position, Cursor.WorldPointer)},{Cursor.HitObject.collider?.name}");
            if (Input.GetKeyDown(KeyCode.X))
                Debug.Log($"{AttackTarget}");

            AbilityController();
            EverySecond();
            CharacterUpdate();
        }
        else //Wszystko podczas śmierci
        {
            RessurectTimer -= Time.deltaTime;
            if (RessurectTimer <= 0)
                Resurrect();
        }
    }

    public void AbilityController()
    {
        if (firstAbilityCD < 0 && !canUseFirstAbility)
            canUseFirstAbility = true;
        else
            firstAbilityCD -= Time.deltaTime;

        if (secondAbilityCD < 0 && !canUseSecondAbility)
            canUseSecondAbility = true;
        else
            secondAbilityCD -= Time.deltaTime;

        if (thirdAbilityCD < 0 && !canUseThirdAbility)
            canUseThirdAbility = true;
        else
            thirdAbilityCD -= Time.deltaTime;

        if (UltimateCD < 0 && !canUseUltimate)
            canUseUltimate = true;
        else
            UltimateCD -= Time.deltaTime;

        if (Input.GetKeyDown(Settings.Q) && canUseFirstAbility)
            UseFirstAbility();
        else if (Input.GetKeyDown(Settings.W) && canUseSecondAbility)
            UseSecondAbility();
        else if (Input.GetKeyDown(Settings.E) && canUseThirdAbility)
            UseThirdAbility();
        else if (Input.GetKeyDown(Settings.R) && canUseUltimate)
            UseUltimate();
    }

    public void EverySecond()
    {
        seconds += Time.deltaTime;
        if (seconds >= 1) // Tutaj wszystko co sekundę :P
        {
            Health += HealthRegen;
            Mana += ManaRegen;
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

    public override void Heal(float heal)
    {
        Health += heal;
    }

    public override void Die()
    {
        charactercontroller.transform.localRotation = new Quaternion(90, 0, 0, 90);
        RessurectTimer = 10 * (2 / Level);
        RessurectTimer = Mathf.Clamp(RessurectTimer, 10, 80);
        WaitForResurrect = true;
    }

    public virtual void Resurrect()
    {
        Health = MaxHealth;
        Mana = MaxMana;
        WaitForResurrect = false;
        Untargetable = false;

        //Potem się go przeniesie do bazy xD
    }

    public virtual void SwitchAttack()
    {
        if (AttackMode == AttackType.Melee)
            AttackMode = AttackType.Ranged;
        else
            AttackMode = AttackType.Melee;
    }

    public void MoveTo(Vector3 Point) => MoveTarget = Point;

    public void AttackMove(GameObject Target) => AttackTargetSet = Target;

    public void DoBasicAttack() => CurrentAnimation = AnimState.BasicAttack;

    public void Attack(GameObject Target, bool IsCrit)
    {
        float Damage = AttackDamage;
        if (IsCrit)
            Damage *= 2;

        charactercontroller.StartCoroutine(CalculateDamage(Target, DamageType.TrueDamage, Damage, IsCrit));
    }

    public IEnumerator CalculateDamage(GameObject Target, DamageType Type, float Damage, bool IsCrit)
    {
        yield return new WaitForSeconds((1 / AttackSpeed) / 3);
        Target?.GetComponent<CharacterController>().GetEntity().DoDamage(Damage, Type, IsCrit);
    }

    //Klasy abstrakcyjne

    public abstract void LoadBasicStats();

    public abstract void Animations();

    public abstract void CharacterUpdate();

    //Odpalanie umiejek
    public abstract void UseFirstAbility();
    public abstract void UseSecondAbility();
    public abstract void UseThirdAbility();
    public abstract void UseUltimate();

    //Opisy umiejek
    public abstract string PassiveDesc { get; }
    public abstract string FirstAbilityDesc { get; }
    public abstract string SecondAbilityDesc { get; }
    public abstract string ThirdAbilityDesc { get; }
    public abstract string UltimateDesc { get; }
}

public struct BasicRangeAttack
{
    GameObject prefab;
    float velocity;
}

public enum AttackType
{
    Melee,
    Ranged
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