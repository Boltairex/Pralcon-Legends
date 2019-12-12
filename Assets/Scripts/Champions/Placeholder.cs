using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Placeholder : MonoBehaviour
{
    GameObject GSet;
    string Path;
    string Name;

    void Start()
    {
        GSet = GameObject.Find("GUIMenu");
        //DO ZMIANY W RAZIE TEMPLATE!
        Path = "Champions/Placeholder/";
        Name = "Placeholder";
        //WSZYSTKIE "Placeholder" ZAMIENIĆ NA "Name"

        Passive();
        SkillQ();
        SkillW();
        SkillE();
        SkillR();
    }

    public void Passive()
    {
        GSet.GetComponent<GUISet>().Avatar.GetComponent<Image>().sprite = Resources.Load<Sprite>(Path +"Avatar");
        //CZĘŚĆ DOSTOSOWANIA
    }

    public void SkillQ()
    {
        GSet.GetComponent<GUISet>().QSkill.GetComponent<Image>().sprite = Resources.Load<Sprite>(Path + "QSkill");
        //CZĘŚĆ DOSTOSOWANIA
    }

    public void SkillW()
    {
        GSet.GetComponent<GUISet>().WSkill.GetComponent<Image>().sprite = Resources.Load<Sprite>(Path + "WSkill");
        //CZĘŚĆ DOSTOSOWANIA
    }

    public void SkillE()
    {
        GSet.GetComponent<GUISet>().ESkill.GetComponent<Image>().sprite = Resources.Load<Sprite>(Path + "ESkill");
        //CZĘŚĆ DOSTOSOWANIA
    }

    public void SkillR()
    {
        GSet.GetComponent<GUISet>().RSkill.GetComponent<Image>().sprite = Resources.Load<Sprite>(Path + "RSkill");
        //CZĘŚĆ DOSTOSOWANIA
    }

}
