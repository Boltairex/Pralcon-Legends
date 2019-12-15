using UnityEngine;
using UnityEngine.UI;
           // V V V V V wpiszcie tu nazwe skryptu (czyli nazwe postaci i tyle)
public class Placeholder : MonoBehaviour
{
    GameObject GSet;
    PlayerStats Character;
    string Path;
    string Name;

    //Statystyki do dostosowania!
    public int AD = 0;               // Attack DMG
    public int AP = 0;               // Ability Power
    public float Armor = 0f;         // Armor points
    public float Resist = 0f;        // Resist points
    public int MHealth = 0;          // Max Health
    public int MMana = 0;            // Max Mana
    public float AT = 0f;            // Attack speed
    public float MV = 1.5f;          // Movement Speed (nie ruszajcie tego tylko)
    public float AR = 0f;            // Attack Range
    public float HR = 0f;            // Health Regen (0.5, 2.06 <= Przykładowe)
    public float MR = 0f;            // Mana Regen (0.5, 2.06 <= Przykładowe)
    public float LS = 0f;            // Lifesteal percentage
    public float CT = 0f;            // Critical Hit percentage

    void Start()
    {
        Character = GameObject.Find("PlayerStuff").GetComponent<PlayerStats>();
        GSet = GameObject.Find("GUIMenu");

        //DO ZMIANY W RAZIE TEMPLATE! w "Name" Wpisać nazwę postaci. Będzie ona wyświetlana.

        //Character.AddComponent<Placeholder>();
        Character.gameObject.AddComponent<PlaceholderSkills>();
        Path = "Champions/Placeholder/";
        Name = "Placeholder";

        //Wszystkie "Placeholder" zamienić na tekst z "Name". Nie ważne gdzie się znajdują, po prostu zamienić. Można do tego użyć narzędzia szybkiej zamiany w edytorze tekstu.
        //Path oznacza ścieżkę do tekstur postaci, oraz potrzebnych zasobów (skrypt do skilli, ewentualnie modele).

        Character.CharacterName = Name;
        Character.AD = AD;
        Character.AP = AP;
        Character.Armor = Armor;
        Character.Resist = Resist;
        Character.MHealth = MHealth;
        Character.MMana = MMana;
        Character.AT = AT;
        Character.MV = MV;
        Character.AR = AR;
        Character.HR = HR;
        Character.MR = MR;
        Character.LS = LS;
        Character.CT = CT;

        Passive();
        SkillQ();   
        SkillW();
        SkillE();
        SkillR();
    }

    // Możecie formatować tekst w Skillach, wpisując w string argumenty (każdy trzeba zakończyć):
    // <b> ... </b> - pogrubienie
    // <u> ... </n> - podkreślenie
    // <s> ... </s> - przekreślenie
    // <i> ... </i> - italica/kursywa
    // \n - wymuszenie przerzucenia do następnej linii tekstu
    // <color=nazwa/HSV> ... </color> - kolor
    // <size=liczba> ... </size> - wielkość (od 15 do 20 max)
    
    public void Passive()
    {
        GSet.GetComponent<GUISet>().Avatar.GetComponent<Image>().sprite = Resources.Load<Sprite>(Path +"Avatar");
        //CZĘŚĆ DOSTOSOWANIA
        GSet.GetComponent<GUISet>().sizeP = 1; // Maksymalnie do dwóch oznacza ona wielkość okna opisu
        string desc= //Opis na dole
        "<color=red><b>Placeholder</b></color> \n" +
        "Pasywa: Sprawdzanie tekstu" +
        "";

        //KONIEC DOSTOSOWYWANIA
        GSet.GetComponent<GUISet>().PassiveD = desc;
    }

    public void SkillQ()
    {
        GSet.GetComponent<GUISet>().QSkill.GetComponent<Image>().sprite = Resources.Load<Sprite>(Path + "QSkill");
        //CZĘŚĆ DOSTOSOWANIA
        GSet.GetComponent<GUISet>().sizeQ = 1; // Maksymalnie do dwóch oznacza ona wielkość okna opisu
        string desc = //Opis na dole
        "Q: Szara masa" +
        "";

        //KONIEC DOSTOSOWYWANIA
        GSet.GetComponent<GUISet>().SkillQD = desc;
    }

    public void SkillW()
    {
        GSet.GetComponent<GUISet>().WSkill.GetComponent<Image>().sprite = Resources.Load<Sprite>(Path + "WSkill");
        //CZĘŚĆ DOSTOSOWANIA
        GSet.GetComponent<GUISet>().sizeW = 1; // Maksymalnie do dwóch oznacza ona wielkość okna opisu
        string desc = //Opis na dole
        "W: Szarżujące miensko" +
        "";

        //KONIEC DOSTOSOWYWANIA
        GSet.GetComponent<GUISet>().SkillWD = desc;
    }

    public void SkillE()
    {
        GSet.GetComponent<GUISet>().ESkill.GetComponent<Image>().sprite = Resources.Load<Sprite>(Path + "ESkill");
        //CZĘŚĆ DOSTOSOWANIA
        GSet.GetComponent<GUISet>().sizeE = 1.5f; // Maksymalnie do dwóch oznacza ona wielkość okna opisu
        string desc = //Opis na dole
        "E: False mist \n"+
        "Jest to umiejętność tylko pasywna, powiązana ze stackami zbieranymi przez Cage'a. Każdy stack to jeden procent szans na to, że cage zautoattackuje 2 cele naraz, przy czym tylko jeden będzie ewentualnie zadawać obrażenia przeciwnikowi, kiedy reszta przeleci przez przeciwnika, jakby nic nie trafiły, dając możliwość do kompilacji z q. Przekroczenie barriery 100%, pozwala na szansę zaatakowania 3 celów naraz. Hipotetycznie więc mówiąc, mając 1000% Cage zaatakowałby 10 przeciwników naraz, ale tylko jednemu będzie zadawać obrażenia";
        //Taka wielkość jest optymalna, nie przekraczać jej lepiej.

        //KONIEC DOSTOSOWYWANIA
        GSet.GetComponent<GUISet>().SkillED = desc;
    }

    public void SkillR()
    {
        GSet.GetComponent<GUISet>().RSkill.GetComponent<Image>().sprite = Resources.Load<Sprite>(Path + "RSkill");
        //CZĘŚĆ DOSTOSOWANIA
        GSet.GetComponent<GUISet>().sizeR = 1; // Maksymalnie do dwóch oznacza ona wielkość okna opisu
        string desc = //Opis na dole
        "R: Klikanie jak głupi" +
        "";

        //KONIEC DOSTOSOWYWANIA
        GSet.GetComponent<GUISet>().SkillRD = desc;
    }

}
