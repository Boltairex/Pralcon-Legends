using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Placeholder : MonoBehaviour
{
    GameObject GSet;
    GameObject Character;
    string Path;
    string Name;

    void Start()
    {
        Character = GameObject.Find("PlayerStuff");
        GSet = GameObject.Find("GUIMenu");

        //DO ZMIANY W RAZIE TEMPLATE! w "Name" Wpisać nazwę postaci. Będzie ona wyświetlana.
        //Character.AddComponent<Placeholder>();
        Path = "Champions/Placeholder/";
        Name = "Placeholder";
        //Wszystkie "Placeholder" zamienić na tekst z "Name".
        //Path oznacza ścieżkę do tekstur postaci.

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
    // \n - wymuszenie przerzucenia do następnej linii
    // <color=nazwa/RGB> ... </color> - kolor
    // <size=liczba> - wielkość (od 5 do 10 max)
    
    public void Passive()
    {
        GSet.GetComponent<GUISet>().Avatar.GetComponent<Image>().sprite = Resources.Load<Sprite>(Path +"Avatar");
        //CZĘŚĆ DOSTOSOWANIA

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

        string desc = //Opis na dole
        "E: False mist \n"+
        "Jest to umiejętność tylko pasywna, powiązana ze stackami zbieranymi przez Cage'a. Każdy stack to jeden procent szans na to, że cage zautoattackuje 2 cele naraz, przy czym tylko jeden będzie ewentualnie zadawać obrażenia przeciwnikowi, kiedy reszta przeleci przez przeciwnika, jakby nic nie trafiły, dając możliwość do kompilacji z q. Przekroczenie barriery 100%, pozwala na szansę zaatakowania 3 celów naraz. Hipotetycznie więc mówiąc, mając 1000% Cage zaatakowałby 10 przeciwników naraz, ale tylko jednemu będzie zadawać obrażenia";

        //KONIEC DOSTOSOWYWANIA
        GSet.GetComponent<GUISet>().SkillED = desc;
    }

    public void SkillR()
    {
        GSet.GetComponent<GUISet>().RSkill.GetComponent<Image>().sprite = Resources.Load<Sprite>(Path + "RSkill");
        //CZĘŚĆ DOSTOSOWANIA

        string desc = //Opis na dole
        "R: Klikanie jak głupi" +
        "";

        //KONIEC DOSTOSOWYWANIA
        GSet.GetComponent<GUISet>().SkillRD = desc;
    }

}
