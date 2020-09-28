using UnityEngine;

public abstract class Entity
{
    public float Health
    {
        get => health;
        protected set
        {
            health = Mathf.Clamp(value,-1, MaxHealth);
            if (health <= 0)
                Die();
        }
    }

    public float Mana
    {
        get => mana;
        protected set
        {
            mana = Mathf.Clamp(value, 0, MaxMana);
        }
    }

    public ManaTypes ManaType { get; protected set; }

    public int AttackDamage { get; protected set; }
    public int AbilityPower { get; protected set; }
    public int CriticalChance { get; protected set; }

    public float MaxHealth { get; protected set; }
    public float MaxMana { get; protected set; }
    public float HealthRegen { get; protected set; }
    public float ManaRegen { get; protected set; }
    public float Armor { get; protected set; }
    public float MagicResist { get; protected set; }
    public float AttackRange { get; protected set; }
    public float CooldownReduction { get; protected set; }
    public float AttackSpeed { get; protected set; }
    public float MovementSpeed { get; protected set; }

    private float health { get; set; }
    private float mana { get; set; }

    private GameObject EntityObject;

    protected void SetEntityObject(GameObject G) => EntityObject = G;

    public void DoDamage(float dmg, DamageType type, bool IsCrit)
    {
        float _dmg = dmg;
        if (type == DamageType.AD)
        { _dmg = (dmg - Armor); }
        else if (type == DamageType.AP)
        { _dmg = (dmg - MagicResist); }
        
        _dmg = Mathf.Clamp(_dmg,0, 9999);

        Color C;
        if (!IsCrit)
            C = Color.white;
        else 
            C = Color.red;

        FloatingDamage.CreateFloatingText(EntityObject.transform.position, _dmg.ToString(), C);
        Health -= _dmg;
    }

    public abstract void Heal(float heal);

    public abstract void SwitchAttack();

    public abstract void Die();

    public abstract void Resurrect();
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

