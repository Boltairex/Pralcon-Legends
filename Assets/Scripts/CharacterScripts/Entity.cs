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
        protected set => mana = Mathf.Clamp(value, 0, MaxMana);
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
            experience = Mathf.Clamp(value,0,99999);
            if (Experience >= ExperienceCap && Level < 26)
                Level++;
        }
    }

    public bool Untargetable { get; protected set; }
    public bool Team { get; protected set; } // 0 = pierwszy, 1 = drugi
    public ManaTypes ManaType { get; protected set; } // Nie ma wpływu na gameplay, tylko na wyświetlanie na UI :P

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
    public float ExperienceCap { get; private set; }

    //Prywatne zmienne lokalne

    private float experience = 0;
    private int level = 1;
    private float health;
    private float mana;
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

        Debug.Log($"{EntityObject.name}, {Health}");
    }

    public void GetExperience(float f) => Experience += f;

    //Klasy abstrajcyjne

    public abstract void OnStart(); // Każdy

    public abstract void OnUpdate(); // Każdy

    public abstract void Die(); // Każdy

    public abstract void Heal(float heal); // Miniony i Championy
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