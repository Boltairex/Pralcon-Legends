using System.Collections.Generic;
using UnityEngine;

public abstract class Entity
{
    #region Property

    public int CooldownReduction { get => Mathf.Clamp(cooldownreduction, 0, 40); }

    public int AttackDamage { get => Mathf.RoundToInt(AdditiveAttackDamage + basicAttackDamage + level * basicAttackDamagePerLevel); }

    public float AttackSpeed { get => AdditiveAttackSpeed + basicAttackSpeed + level * basicAttackSpeedPerLevel; }

    public float AttackRange { get => AdditiveAttackRange + basicAttackRange + level * basicAttackRangePerLevel; }

    public float HealthRegen { get => AdditiveHealthRegen + basicHealthRegen + level * basicHealthRegenPerLevel; }

    public float ManaRegen { get => AdditiveManaRegen + basicManaRegen + level * basicManaRegenPerLevel; }

    public float MovementSpeed { get => AdditiveMovementSpeed + basicMovementSpeed + level * basicMovementSpeedPerLevel; }

    public float MaxMana { get => AdditiveMaxMana + basicMaxMana + level * basicMaxManaPerLevel; }

    public float MaxHealth { get => AdditiveMaxHealth + basicMaxHealth + level * basicMaxHealthPerLevel; }

    public float Armor { get => AdditiveArmor + basicArmor + level * basicArmorPerLevel; }

    public float MagicResist { get => AdditiveMagicResist + basicMagicResist + level * basicMagicResistPerLevel; }

    public float Shield
    {
        get => shield;
        private set => shield = Mathf.Clamp(value, 0, 9999);
    }

    public float Health
    {
        get => health;
        private set
        {
            health = Mathf.Clamp(value, -1, MaxHealth);
            if (health <= 0 && !Dummy)
            {
                Die();
                Debug.Log($"{EntityObject.name} died");
                Untargetable = true;
            }
        }
    }

    public float Mana
    {
        get => mana;
        private set => mana = Mathf.Clamp(value, 0, MaxMana);
    }

    public int Level
    {
        get => level;
        private set
        {
            level = Mathf.Clamp(value, 1, 26);
            Experience -= ExperienceCap;
            ExperienceCap = level * 150f;
        }
    }

    public float Experience
    {
        get => experience;
        private set
        {
            experience = Mathf.Clamp(value, 0, 99999);
            if (Experience >= ExperienceCap && Level < 26)
                Level++;
        }
    }

    public IReadOnlyList<Modifiers> ModifiersList => currentmodifiers;

    #endregion

    #region Fieldy

    public bool Untargetable { get; protected set; }
    public bool Team { get; protected set; } // 0 = pierwszy, 1 = drugi
    public bool Dummy { get; protected set; }

    public ManaTypes ManaType { get; protected set; } // Nie ma wpływu na gameplay, tylko na wyświetlanie na UI :P

    public int CriticalChance { get; private set; }
    public int AbilityPower { get; private set; }

    public bool IsGrabbed { get; private set; }

    public GameObject EntityObject { get; private set; }

    //Modyfikowalne przez Modifiery

    public int AdditiveAttackDamage { get; protected set; }
    public float AdditiveMaxHealth { get; protected set; }
    public float AdditiveMaxMana { get; protected set; }
    public float AdditiveHealthRegen { get; protected set; }
    public float AdditiveManaRegen { get; protected set; }
    public float AdditiveArmor { get; protected set; }
    public float AdditiveMagicResist { get; protected set; }
    public float AdditiveAttackRange { get; protected set; }
    public float AdditiveAttackSpeed { get; protected set; }
    public float AdditiveMovementSpeed { get; protected set; }

    //Statystyki bazowe

    protected float basicMaxHealth;
    protected float basicMaxMana;
    protected float basicMovementSpeed;
    protected float basicArmor;
    protected float basicMagicResist;
    protected float basicAttackDamage;
    protected float basicAttackSpeed;
    protected float basicAttackRange;
    protected float basicHealthRegen;
    protected float basicManaRegen;

    //Zwiększanie o poziom

    protected float basicMaxHealthPerLevel;
    protected float basicMaxManaPerLevel;
    protected float basicMovementSpeedPerLevel;
    protected float basicArmorPerLevel;
    protected float basicMagicResistPerLevel;
    protected float basicAttackDamagePerLevel;
    protected float basicAttackSpeedPerLevel;
    protected float basicAttackRangePerLevel;
    protected float basicHealthRegenPerLevel;
    protected float basicManaRegenPerLevel;

    //Niemodyfikowalne przez postacie

    public float ExperienceCap { get; private set; }
    private List<Modifiers> currentmodifiers = new List<Modifiers>();

    #endregion

    //Zmienne lokalne

    private int cooldownreduction = 0;
    private int level = 1;
    private float experience = 0;
    private float health;
    private float mana;
    private float shield;

    protected void SetEntityObject(GameObject G) => EntityObject = G;

    public void DoDamage(float dmg, DamageType type, bool IsCrit, bool ShowText = true)
    {
        float _dmg = dmg;

        if (IsCrit)
            _dmg *= 2;

        if (type == DamageType.AD)
        { _dmg = (dmg - Armor); }
        else if (type == DamageType.AP)
        { _dmg = (dmg - MagicResist); }

        if (_dmg > 0 && shield > 0)
        {
            float _shield = shield;
            shield -= _dmg;
            _dmg = _shield;
        }

        _dmg = Mathf.Clamp(_dmg, 0, 9999);

        if (ShowText)
        {
            Color C;
            if (!IsCrit)
                C = Color.white;
            else
                C = Color.red;

            FloatingDamage.CreateFloatingText(EntityObject.transform.position, _dmg.ToString(), C);
        }

        Health -= _dmg;

        Debug.Log($"{EntityObject.name}, {Health}");
    }

    public void GetExperience(float f) => Experience += f; //Championy

    public void DeleteModifier(Modifiers modifier) => currentmodifiers.Remove(modifier); //Każdy

    public void AddModifier(Modifiers modifier) => currentmodifiers.Add(modifier); // Każdy

    public void SetGrab(bool t) => IsGrabbed = t;

    public void OnResurrect() //Championy
    {
        Untargetable = false;
        Health = MaxHealth;
        Mana = MaxMana;
    }

    public void OnRegeneration() //Championy 
    {
        Health += HealthRegen;
        Mana += ManaRegen;
    }

    public void Heal(float heal) //Miniony i Championy
    {
        Health += heal;
    }

    //Klasy abstrakcyjne
    public abstract void GrabbedSomeone(); // Miniony i Championy 

    public abstract void OnStart(); // Każdy

    public abstract void OnUpdate(); // Każdy

    public abstract void Die(); // Każdy
}

public interface IEntity
{
    Entity GetEntity();
}

public enum ManaTypes
{
    Mana,
    Stamina,
    Another,
    None
}

public enum DamageType
{
    AP,
    AD,
    TrueDamage
}