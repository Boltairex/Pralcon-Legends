public abstract class Modifiers
{
    public readonly bool CanTargetMinion;
    public readonly bool CanTargetTower;
    public readonly bool CanTargetChampion;

    public readonly ModifierType Modifier;

    public readonly int Power;
    public readonly int Loops;

    public readonly float Delay;

    public readonly Entity Target;

    public Modifiers(bool canTargetMinion, bool canTargetTower, bool canTargetChampion, ModifierType modifierType, int power, Entity target, int loops, float delay)
    {
        CanTargetMinion = canTargetMinion;
        CanTargetTower = canTargetTower;
        CanTargetChampion = canTargetChampion;
        Modifier = modifierType;
        Target = target;
        Power = power;
        Loops = loops;
        Delay = delay;
    }

    public abstract void OnUse();
}

public enum ModifierType
{
    Friendly,
    Offensive,
    Neutral
}

public interface IModifier
{
    Modifiers GetModifier();
}