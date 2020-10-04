using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpellSystem
{
    public GameObject Spell;
    public bool Destroy = false;

    public readonly SpellInfo Package;

    Entity Hit;

    float distance = 0;
    float localtimer = 0;
    float delay = 0;

    bool IsReturning = false;
    bool Lock = false;
    bool DontCheck = false;

    private List<Collider> Hitted = new List<Collider>();
    List<Entity> AppliedDamage = new List<Entity>();

    /// <summary>
    /// Zawsze wymagany jest Delay, Owner i Type (SpellType). Jeżeli ScanDistance nie zostanie ustawiony, przybierze wartość 0.5f.
    /// </summary>
    /// <param name="package"></param>
    /// <param name="spell"></param>
    public SpellSystem(SpellInfo package, GameObject spell)
    {
        Package = package;
        if (spell != null)
        {
            Spell = spell;
            Spell.transform.position = Package.Owner.EntityObject.transform.localPosition;
            Spell.transform.LookAt(Package.Target);
        }

        if (Package.ScanDistance == 0)
            Package.ScanDistance = 0.5f;
    }

    public void OnUpdate()
    {
        try
        {
            if (Package.Delay < delay)
            {
                if (Package.Type == SpellType.DamageInCircle)
                {
                    if (!DontCheck)
                        Hitted = Physics.OverlapSphere(Package.Owner.EntityObject.transform.position, Package.ScanDistance, Cursor.This.layer2).ToList();

                    TypeDamageInCircle();
                }
                else
                {
                    if (!DontCheck)
                        Hitted = Physics.OverlapSphere(Spell.transform.position, Package.ScanDistance, Cursor.This.layer2).ToList();

                    Spell.SetActive(true);

                    if (IsReturning && Vector3.Distance(Package.Owner.EntityObject.transform.position, Spell.transform.position) < 0.2f)
                        Destroy = true;

                    if (distance <= Package.MaxDistance)
                        distance += Package.Speed * Time.deltaTime;
                    else
                        IsReturning = true;

                    if (!IsReturning)
                        Spell.transform.position += Spell.transform.forward * Package.Speed * Time.deltaTime;

                    switch (Package.Type)
                    {
                        case SpellType.Throw:
                            TypeThrow();
                            break;
                        case SpellType.ThrowAndReturn:
                            TypeThrowAndReturn();
                            break;
                        case SpellType.ThrowAndGrab:
                            TypeThrowAndGrab();
                            break;
                        case SpellType.ThrowAndExplode:
                            TypeThrowAndExplode();
                            break;
                    }
                }
            }
            else
                delay += Time.deltaTime;
        }
        catch (Exception E) { Debug.Log(E); }
    }

    void TypeDamageInCircle() // Wymaga: ActivateTime, Power1, DType, // Opcjonalne: Modifier, CanTargetMinion, CanTargetTower, IsCrit,
    {
        if (localtimer < Package.ActivateTime)
        {
            string s = "";
            localtimer += Time.deltaTime;
            foreach (Collider coll in Hitted)
            {
                s += $"{coll.name}, ";
                if (coll.gameObject != Package.Owner.EntityObject.gameObject
                    && (coll.GetComponent<IEntity>()?.GetEntity() is Champion
                    || coll.GetComponent<IEntity>()?.GetEntity() is Minion && Package.CanTargetMinion
                    || coll.GetComponent<IEntity>()?.GetEntity() is Tower && Package.CanTargetTower))
                {
                    Hit = coll.GetComponent<IEntity>().GetEntity();
                    if (!AppliedDamage.Contains(Hit))
                    {
                        Hit.DoDamage(Package.Power1, Package.DType, Package.IsCrit, true);
                        if (Package.ApplyModifier != null)
                            Hit.AddModifier(Package.ApplyModifier);
                        AppliedDamage.Add(Hit);
                    }
                }
                Debug.Log(s);
            }
        }
        else
        {
            DontCheck = true;
            Destroy = true;
        }
    }

    void TypeThrow()
    {
        if (Hit == null)
        {
            foreach (Collider coll in Hitted)
            {
                if (coll.gameObject != Package.Owner.EntityObject.gameObject
                    && (coll.GetComponent<IEntity>()?.GetEntity() is Champion
                    || coll.GetComponent<IEntity>()?.GetEntity() is Minion && Package.CanTargetMinion
                    || coll.GetComponent<IEntity>()?.GetEntity() is Tower && Package.CanTargetTower))
                {
                    DontCheck = true;
                    Hit = coll.GetComponent<IEntity>()?.GetEntity();
                    Hit.DoDamage(Package.Power1, Package.DType, Package.IsCrit, true);
                    if (Package.ApplyModifier != null)
                        Hit.AddModifier(Package.ApplyModifier);
                    break;
                }
            }
        }
        else
            Destroy = true;
    }

    void TypeThrowAndReturn()
    {
        foreach (Collider coll in Hitted)
        {
            if (coll.gameObject != Package.Owner.EntityObject.gameObject
                && (coll.GetComponent<IEntity>()?.GetEntity() is Champion
                || coll.GetComponent<IEntity>()?.GetEntity() is Minion && Package.CanTargetMinion
                || coll.GetComponent<IEntity>()?.GetEntity() is Tower && Package.CanTargetTower))
            {
                Hit = coll.GetComponent<IEntity>().GetEntity();
                if (!AppliedDamage.Contains(Hit))
                {
                    Hit.DoDamage(Package.Power1, Package.DType, Package.IsCrit, true);
                    if (Package.ApplyModifier != null)
                        Hit.AddModifier(Package.ApplyModifier);
                    AppliedDamage.Add(Hit);
                }
            }
        }

        if (IsReturning)
        {
            Spell.transform.LookAt(Package.Owner.EntityObject.transform);
            Spell.transform.position += Spell.transform.forward * Package.ReturnSpeed * Time.deltaTime;
        }
    }

    void TypeThrowAndGrab()
    {
        if (Hit == null)
        {
            foreach (Collider coll in Hitted)
            {
                if (coll.gameObject != Package.Owner.EntityObject.gameObject && (coll.GetComponent<IEntity>()?.GetEntity() is Champion
                    || coll.GetComponent<IEntity>()?.GetEntity() is Minion && Package.CanTargetMinion))
                {
                    DontCheck = true;
                    Hit = coll.GetComponent<IEntity>()?.GetEntity();
                    if (Package.ApplyModifier != null)
                        Hit.AddModifier(Package.ApplyModifier);
                    Hit.DoDamage(Package.Power1, Package.DType, Package.IsCrit, true);
                    Hit.SetGrab(true);
                    IsReturning = true;
                    break;
                }
            }
        }
        else
        {
            Hit.EntityObject.transform.position = Spell.transform.position;

            localtimer += Time.deltaTime;
            if (localtimer > Package.ActivateTime || Vector3.Distance(Spell.transform.position,Package.Owner.EntityObject.transform.position) < 2.5f)
            {
                Hit.SetGrab(false);
                Package.Owner.GrabbedSomeone();
                Destroy = true;
            }
        }

        if (IsReturning)
        {
            Spell.transform.LookAt(Package.Owner.EntityObject.transform);
            Spell.transform.position += Spell.transform.forward * Package.ReturnSpeed * Time.deltaTime;
        }
    }

    void TypeThrowAndExplode()
    {

    }

    public void OnDestroy()
    {
        if (Hit != null && Package.Type == SpellType.ThrowAndGrab)
            Hit.SetGrab(false);
        if (Spell != null)
            GameObject.Destroy(Spell);
    }
}

public enum SpellType
{
    DamageInCircle,
    Throw,
    ThrowAndReturn,
    ThrowAndGrab,
    ThrowAndExplode,
}

public struct SpellInfo
{
    public Modifiers ApplyModifier;
    public Entity Owner;
    public SpellType Type;
    public DamageType DType;
    public Vector3 Target;
    public float ScanDistance;
    public float Delay;
    public float MaxDistance;
    public float Power1;
    public float Power2;
    public float ExplodeDistance;
    public float Speed;
    public float ActivateTime;
    public float ReturnSpeed;
    public bool IsCrit;
    public bool CanTargetTower;
    public bool CanTargetMinion;
}