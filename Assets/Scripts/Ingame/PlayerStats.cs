using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    GameClock GC;

    [Header("Statystyki")]
    public int AD; // Attack DMG
    public int AP; // Ability Power
    public float Armor; // Armor Percentage
    public float Resist; // Resist Percentage
    public int Health; // Health
    public int Mana;  // Mana
    public float MV; // Movement Speed
    public float AR; // Attack Range
    public float HR; // Health Regen
    public float MR; // Mana Regen
    public float ARP; // Armor penetration percentage
    public float REN; // Resistr penetration percentage

    [Header("Staty")]
    public int Mon; //Money
    public int K; // Kills
    public int D; // Deaths
    public int A; // Assists
    public int M; // Minions
    
    void Start()
    {
        gameObject.AddComponent<Placeholder>();
        GC = GameObject.Find("GameClock").GetComponent<GameClock>();
    }


    void Update()
    {
        
    }
}
