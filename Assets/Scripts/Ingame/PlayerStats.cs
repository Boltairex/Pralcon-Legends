using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    GameClock GC;
    GameObject Character;

    [Header("Statystyki")]
    public int AD; // Attack DMG
    public int AP; // Ability Power
    public float Armor; // Armor Percentage
    public float Resist; // Resist Percentage
    public int Health; // Health
    public int MHealth; // Max Helth
    public int Mana; // Mana
    public int MMana; // Max Mana
    public float AT; // Attack speed
    public float MV; // Movement Speed
    public float AR; // Attack Range
    public float HR; // Health Regen
    public float MR; // Mana Regen
    public float ARP; // Armor penetration percentage
    public float REN; // Resistr penetration percentage
    public float CDR; // Cooldown Reduction
    public float LS; // Lifesteal
    public float CT; // Critical Chance

    [Header("Staty")]
    public int Mon; //Money
    public int K; // Kills
    public int D; // Deaths
    public int A; // Assists
    public int M; // Minions

    public string CharacterName;
    
    void Start()
    {
        if (GameObject.Find("Character"))
        {
            Character = GameObject.Find("Character");
            Character.AddComponent<PlayerController>();
        }
        gameObject.AddComponent<Placeholder>();
        GC = GameObject.Find("GameClock").GetComponent<GameClock>(); 
    }
}
