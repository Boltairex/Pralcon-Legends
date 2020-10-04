using UnityEngine;

public class Ignite : Modifiers
{
    public Ignite(Entity Target, int Power, int Loops, float Delay) : base(true, false, true, ModifierType.Offensive, Power, Target, Loops, Delay) { }

    float localTimer;
    float currentloop = 1;

    public void OnUpdate()
    {
        localTimer += Time.deltaTime;
        if (localTimer >= Delay && currentloop <= Loops)
            OnUse();
        else if (currentloop > Loops)
            Target.DeleteModifier(this);
    }

    public override void OnUse()
    {
        currentloop++;
        Target.DoDamage(Power, DamageType.TrueDamage, false);
    }
}