using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    //Champion currentChampion = new AsheChampion();

    bool isChampionInitialized;
    Champion currChampion = new PlaceholderScript();

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
            currChampion.OnUpdate();
        }
    }
}

public abstract class Champion
{
    public CharacterController charactercontroller;

    public AttackType AttackMode { get; protected set;}

    public int AttackDamage { get; protected set;}
    public int AbilityPower { get; protected set;}

    public float Health { get; protected set;}
    public float Mana { get; protected set;}
    public float HealthRegen { get; protected set;}
    public float ManaRegen { get; protected set;}
    public float Armor { get; protected set;}
    public float MagicResist { get; protected set;}
    public float AttackRange { get; protected set;}
    public float CooldownReduction { get; protected set;}
    public float MovementSpeed { get; protected set;}

    private Vector3 movetarget;
    public Vector3 MoveTarget
    {
        get{return movetarget;}
        set{movetarget = value;
        IsMoving = true;}
    }

    public bool IsMoving;

    public void OnMove()
    {
        
    }

    public virtual void OnStart()
    {
        MovementSpeed = 340;
        // inicjalizacja, pobranie jakichś rzeczy
    }

    public virtual void OnUpdate()
    {
        Debug.Log("Lol");
        if (Input.GetMouseButtonDown(0))
            MoveTo(Cursor.WorldPointer);
        
        if(IsMoving)
        {
            charactercontroller.transform.position = Vector3.MoveTowards(charactercontroller.transform.position,MoveTarget,MovementSpeed / 6000);
            if(Vector3.Distance(charactercontroller.transform.position,MoveTarget) < 0.1f)
                IsMoving = false;
        }
    }

    public virtual void MoveTo(Vector3 Point) => MoveTarget = Point;
    /*
    public virtual void OnDamage(Damage damage)
    {

    }
    */
    //public abstract void OnAttack(Entity entity, Attack attack);
}

public enum AttackType
{
    Melee,
    Ranged,
    Switched
}