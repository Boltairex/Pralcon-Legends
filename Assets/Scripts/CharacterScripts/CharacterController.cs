using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour, IEntity
{
    public bool IsLocalController;

    protected bool isChampionInitialized;

    public float DebugGizmo = 2;

    public Animator Anim;
    protected Champion currChampion = new Characters.AeiaeiScript();

    protected int segments = 50;
    protected float radius = 2;
    protected LineRenderer line;

    void Awake()
    {
        line = gameObject.GetComponent<LineRenderer>();
        line.SetVertexCount(segments + 1);
        line.useWorldSpace = false;

        Anim = this.GetComponent<Animator>();
    }

    void CreatePoints()
    {
        float x, z;
        float angle = 20f;

        for (int i = 0; i < (segments + 1); i++)
        {
            x = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;
            z = Mathf.Cos(Mathf.Deg2Rad * angle) * radius;

            line.SetPosition(i, new Vector3(x, 0.5f, z));
            angle += (360f / segments);
        }
    }

    public GameObject LocalInstantiate(GameObject G) => Instantiate(G);

    public void DestroyMe(GameObject G) => Destroy(G);

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

    public void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(this.transform.position, DebugGizmo);
    }
}

public abstract class Champion : Entity
{
    #region Property
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
            Vector3 V = attackTarget.EntityObject.transform.position;
            V.y = 2.5f;
            return V;
        }
    }

    public Entity AttackTargetSet
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

    public virtual bool LockOperations
    {
        get
        {
            if ( IsGrabbed || AnimationRun || UsingSpell || IsDashing || AAState == BasicAttackState.ExitMove)
            { return true; }
            else
            { return false; }
        }
    }

    public float QAbilityCooldown { get => QInfo.BasicCooldown - (QInfo.Level - 1) * QInfo.CooldownPerLevel; }

    public float WAbilityCooldown { get => WInfo.BasicCooldown - (WInfo.Level - 1) * WInfo.CooldownPerLevel; }

    public float EAbilityCooldown { get => EInfo.BasicCooldown - (EInfo.Level - 1) * EInfo.CooldownPerLevel; }

    public float RAbilityCooldown { get => RInfo.BasicCooldown - (RInfo.Level - 1) * RInfo.CooldownPerLevel; }
    #endregion

    #region Fieldy

    //Fieldy
    protected Champion This;
    public CharacterController charactercontroller { get; set; }

    public AttackType AttackMode { get; protected set; }
    public AnimState CurrentAnimation { get; protected set; }
    public BasicAttackState AAState { get; private set; }

    public bool IsMoving { get; protected set; }
    public bool IsAttacking { get; protected set; }
    public bool IsDashing { get; protected set; }
    public bool AnimationRun { get; protected set; }
    public bool AACooldown { get; private set; }
    public bool NextAACrit { get; private set; }
    public bool WaitForResurrect { get; private set; }

    public bool CancelNextAutoattack { get; protected set; }
    public bool UsingSpell { get; protected set; }

    public float RessurectTimer { get; private set; }

    public ToPoint DashContainer { get; private set; }

    // Umiejętności

    public AbilityInfo QInfo;
    public AbilityInfo WInfo;
    public AbilityInfo EInfo;
    public AbilityInfo RInfo;

    protected List<SpellSystem> currentSpells = new List<SpellSystem>();

    #endregion

    //Prywatne lokalne fieldy

    private Entity attackTarget;

    private Vector3 moveTarget;
    private Vector3 targetDirection;
    private Vector3 newDirection;

    private float FirstAbilityCD;
    private float SecondAbilityCD;
    private float ThirdAbilityCD;
    private float UltimateCD;

    private float dashDistance = 0;

    private bool CanUseFirstAbility = false;
    private bool CanUseSecondAbility = false;
    private bool CanUseThirdAbility = false;
    private bool CanUseUltimate = false;

    private bool cancelAnimation;
    private bool debug = false;


    public Champion()
    {
        This = this;
        IsMoving = false;
        IsAttacking = false;
        AnimationRun = false;
        WaitForResurrect = false;
        Untargetable = false;
    }

    public override void OnStart()
    {
        base.SetEntityObject(charactercontroller.gameObject);

        CalculateCritChance();

        if (charactercontroller.IsLocalController)
            OnChampionStart();
        else
            Dummy = true;
    }

    public override void OnUpdate()
    {
        try
        {
            foreach (SpellSystem Spell in currentSpells)
            {
                Spell.OnUpdate();
                if (Spell.Destroy)
                {
                    Spell.OnDestroy();
                    currentSpells.Remove(Spell);
                }
            }
        }
        catch { }

        if (!WaitForResurrect) // Wszystko w czasie życia
        {
            this.Animations();
            if (!IsGrabbed)
            {
                if (!LockOperations)
                {
                    if (Input.GetMouseButton(1))
                    {
                        var entity = Cursor.HitObject.transform?.GetComponent<IEntity>().GetEntity();
                        if (entity != null && !entity.Untargetable && entity != this)
                            AttackMove(entity);
                        else
                            MoveTo(Cursor.WorldPointer);
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
                else if (UsingSpell)
                {
                    IsMoving = false;
                    IsAttacking = false;
                }

                if (CurrentAnimation == AnimState.BasicAttack && Input.GetMouseButtonDown(1))
                    CancelNextAutoattack = true;

                if (IsDashing) // Mechanika dashowania
                {
                    if (Vector3.Distance(charactercontroller.transform.position, DashContainer.Destination) <= 0.1f)
                        IsDashing = false;
                    else
                    {
                        dashDistance += Time.deltaTime * DashContainer.Speed;
                        if (dashDistance >= DashContainer.MaxDistance)
                            IsDashing = false;
                        else
                            charactercontroller.transform.position = Vector3.MoveTowards(charactercontroller.transform.position, DashContainer.Destination, Time.deltaTime * DashContainer.Speed);
                    }
                }

                if (IsAttacking && AttackTarget != null && CurrentAnimation != AnimState.BasicAttack)
                {
                    if (Vector3.Distance(charactercontroller.transform.position, AttackTarget) <= AttackRange && !AACooldown)
                    {
                        charactercontroller.transform.LookAt(AttackTarget);
                        DoBasicAttack();
                        IsAttacking = false;
                    }
                    else if (Vector3.Distance(charactercontroller.transform.position, AttackTarget) <= AttackRange && AACooldown)
                    {
                        charactercontroller.transform.LookAt(AttackTarget);
                    }
                    else
                        charactercontroller.transform.position = Vector3.MoveTowards(charactercontroller.transform.position, AttackTarget, (MovementSpeed * Time.deltaTime) / 40);
                }
                else if (IsMoving && MoveTarget != Vector3.zero)
                {
                    if (Vector3.Distance(charactercontroller.transform.position, MoveTarget) < 0.1f)
                        IsMoving = false;
                    else
                        charactercontroller.transform.position = Vector3.MoveTowards(charactercontroller.transform.position, MoveTarget, (MovementSpeed * Time.deltaTime) / 40);
                }

                if (AttackTargetSet != null)
                {
                    if (CurrentAnimation == AnimState.BasicAttack && Vector3.Distance(charactercontroller.transform.position, AttackTarget) > AttackRange || AttackTargetSet.Untargetable)
                        AAState = BasicAttackState.None;
                }
            } //Jeżeli nie jest się zgrabbowanym

            EverySecond();
            CharacterUpdate();
            AbilityController();

            if(AACooldown)
                AACooldownTimer();
            if (AAState != BasicAttackState.None)
                AATimer();
        }
        else //Wszystko podczas śmierci
        {
            RessurectTimer -= Time.deltaTime;
            if (RessurectTimer <= 0)
                Resurrect();
        } //Zwykły update

        if (debug)
            Debug.Log($"Animacja: {CurrentAnimation}, Atakowanie: {IsAttacking}, Poruszanie: {IsMoving}, Target: {MoveTarget}");
        if (Input.GetKeyDown(KeyCode.Z))
            Debug.Log($"{Vector3.Distance(charactercontroller.transform.position, Cursor.WorldPointer)},{Cursor.HitObject.collider?.name}");
        if (Input.GetKeyDown(KeyCode.X))
            Debug.Log($"{AttackTarget}");
    }

    //Mechaniki pasywne

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

    private float seconds;
    public void EverySecond()
    {
        seconds += Time.deltaTime;
        if (seconds >= 1) // Tutaj wszystko co sekundę :P
            OnRegeneration();
    }

    public void AbilityController()
    {
        if (FirstAbilityCD < 0 && !CanUseFirstAbility && QInfo.Level > 0)
            CanUseFirstAbility = true;
        else
            FirstAbilityCD -= Time.deltaTime;

        if (SecondAbilityCD < 0 && !CanUseSecondAbility && WInfo.Level > 0)
            CanUseSecondAbility = true;
        else
            SecondAbilityCD -= Time.deltaTime;

        if (ThirdAbilityCD < 0 && !CanUseThirdAbility && EInfo.Level > 0)
            CanUseThirdAbility = true;
        else
            ThirdAbilityCD -= Time.deltaTime;

        if (UltimateCD < 0 && !CanUseUltimate && RInfo.Level > 0)
            CanUseUltimate = true;
        else
            UltimateCD -= Time.deltaTime;

        if (!UsingSpell && !AnimationRun && CurrentAnimation != AnimState.QAbility && CurrentAnimation != AnimState.WAbility && CurrentAnimation != AnimState.EAbility && CurrentAnimation != AnimState.RAbility)
        {
            if (Input.GetKeyDown(Settings.Q) && CanUseFirstAbility)
            {
                CanUseFirstAbility = false;
                UseFirstAbility();
                FirstAbilityCD = QAbilityCooldown;
            }
            else if (Input.GetKeyDown(Settings.W) && CanUseSecondAbility)
            {
                CanUseSecondAbility = false;
                UseSecondAbility();
                SecondAbilityCD = WAbilityCooldown;
            }
            else if (Input.GetKeyDown(Settings.E) && CanUseThirdAbility)
            {
                CanUseThirdAbility = false;
                UseThirdAbility();
                ThirdAbilityCD = EAbilityCooldown;
            }
            else if (Input.GetKeyDown(Settings.R) && CanUseUltimate)
            {
                CanUseUltimate = false;
                UseUltimate();
                UltimateCD = RAbilityCooldown;
            }
        }
    }

    //Mechaniki aktywne

    public void RotateTowardCursor() => charactercontroller.transform.LookAt(Cursor.WorldPointer);

    private float localAACooldownTimer = 0;

    public void AACooldownTimer() // Zbijanie cooldownu
    {
        localAACooldownTimer -= Time.deltaTime;
        if (localAACooldownTimer <= 0)
            AACooldown = false;
    }

    public void DashToPoint(Vector3 point, float maxDistance, float speed)
    {
        IsDashing = true;
        DashContainer = new ToPoint
        {
            Destination = point,
            MaxDistance = maxDistance,
            Speed = speed,
        };
        dashDistance = 0;
    }

    private float AAProgressTime = 0;
    private float AAExitTime = 0;

    public void SetAASettings(float aaProgressTime, float aaExitTime)
    {
        if (AAState == BasicAttackState.None)
        {
            localAATimer = 0;
            AAProgressTime = aaProgressTime;
            AAExitTime = aaExitTime;
            AACooldown = true;
            localAACooldownTimer = 1 / AttackSpeed;

            AAState = BasicAttackState.InProgress;
        }
        else if (CancelNextAutoattack)
            CancelNextAutoattack = false;
    }

    private float localAATimer = 0;

    private void AATimer()
    {
        localAATimer += Time.deltaTime;

        if (CurrentAnimation != AnimState.BasicAttack && AAState != BasicAttackState.ExitMove && AAState != BasicAttackState.Attack) // Cancelowanie 
        {
            AAState = BasicAttackState.None;
            AACooldown = false;
        }
        else if (localAATimer >= AAProgressTime && AAState == BasicAttackState.InProgress)
            AAState = BasicAttackState.Attack;
        else if (AAState == BasicAttackState.Attack) // Jeżeli nie zcanceluje
        {
            Attack(AttackTargetSet, DamageType.AD, AttackDamage, NextAACrit);
            localAATimer = 0;
            AAState = BasicAttackState.ExitMove;
        }
        else if (localAATimer >= AAExitTime && AAState == BasicAttackState.ExitMove) // Odblokowanie
            AAState = BasicAttackState.None;
    }

    private void MoveTo(Vector3 Point) => MoveTarget = Point;

    private void DoBasicAttack() => CurrentAnimation = AnimState.BasicAttack;

    private void AttackMove(Entity Target) => AttackTargetSet = Target;

    private void Attack(Entity Target, DamageType Type, float Damage, bool IsCrit) => Target?.DoDamage(Damage, Type, IsCrit);

    //Mechaniki nadpisujące

    public override void Die()
    {
        charactercontroller.transform.localRotation = new Quaternion(90, 0, 0, 90);
        RessurectTimer = 10 * (2 / Level);
        RessurectTimer = Mathf.Clamp(RessurectTimer, 10, 80);
        WaitForResurrect = true;
    }

    public virtual void Resurrect()
    {
        OnResurrect();
        WaitForResurrect = false;
        //Potem się go przeniesie do bazy xD
    }

    public virtual void SwitchAttack()
    {
        if (AttackMode == AttackType.Melee)
            AttackMode = AttackType.Ranged;
        else
            AttackMode = AttackType.Melee;
    }

    public IEnumerator AnimationTime(float time)
    {
        UsingSpell = true;
        AnimationRun = true;
        yield return new WaitForSeconds(time);
        if (cancelAnimation)
            cancelAnimation = false;
        else
        {
            AnimationRun = false;
            UsingSpell = false;
        }
    }

    //Klasy abstrakcyjne

    public override void GrabbedSomeone()
    {
        UsingSpell = false;
        AnimationRun = false;
        cancelAnimation = true;
    }

    public abstract void OnChampionStart();

    public abstract void Animations();

    public abstract void CharacterUpdate();

    public abstract float QAbilityDmg { get; }
    public abstract float WAbilityDmg { get; }
    public abstract float EAbilityDmg { get; }
    public abstract float RAbilityDmg { get; }

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

public struct ToPoint
{
    public Vector3 Destination { get; set; }
    public float MaxDistance { get; set; }
    public float Speed { get; set; }
}

public struct AbilityInfo
{
    public float BasicPower { get; set; }
    public float BasicPowerPerLevel { get; set; }
    public DamageType Type { get; set; }

    public float Level { get; set; }
    public float MaxUpgradeLevel { get; set; }
    public float BasicCooldown { get; set; }
    public float CooldownPerLevel { get; set; }
}

public enum BasicAttackState
{
    InProgress,
    Attack,
    ExitMove,
    None
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