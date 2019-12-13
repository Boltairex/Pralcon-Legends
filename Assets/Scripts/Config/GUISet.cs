using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GUISet : MonoBehaviour
{
    [Header("Do kolorów")]
    public GameObject Bar;
    public GameObject ItemFrames;
    public GameObject ChampBar;
    public GameObject ChampBarFront;
    GameObject CR;

    [Header("Do ustawienia grafik i opisów")]
    public GameObject Avatar;
    public GameObject QSkill;
    public GameObject WSkill;
    public GameObject ESkill;
    public GameObject RSkill;
    public GameObject DescIM;
    public GameObject Desc;

    public string PassiveD;
    public string SkillQD;
    public string SkillWD;
    public string SkillED;
    public string SkillRD;

    public string CursorOn;

    public bool IsOn;

    Scrollbar Red;
    Scrollbar Green;
    Scrollbar Blue;

    Animator anim;
    bool click;

    Color32 GUIColor;

    void Start()
    {
        Red = GameObject.Find("Red").GetComponent<Scrollbar>();
        Green = GameObject.Find("Green").GetComponent<Scrollbar>();
        Blue = GameObject.Find("Blue").GetComponent<Scrollbar>();

        CR = GameObject.Find("Color");

        anim = gameObject.GetComponent<Animator>();
        anim.Play("EscapeOFF");

        Avatar.GetComponent<Image>().color = new Color(255, 255, 255, 1);
        QSkill.GetComponent<Image>().color = new Color(255, 255, 255, 1);
        WSkill.GetComponent<Image>().color = new Color(255, 255, 255, 1);
        ESkill.GetComponent<Image>().color = new Color(255, 255, 255, 1);
        RSkill.GetComponent<Image>().color = new Color(255, 255, 255, 1);
    }

    void Update()
    {
        GUIColor = new Color(Red.value, Green.value, Blue.value, 1);

        if (Input.GetButtonDown("Cancel") && click == false)
        {
            click = true;
            anim.Play("EscapeON");
            
        }
        else if (Input.GetButtonDown("Cancel") && click == true)
        {
            click = false;
            anim.Play("EscapeOFF");
        }

        Bar.GetComponent<Image>().color = GUIColor;
        ItemFrames.GetComponent<Image>().color = GUIColor;
        ChampBar.GetComponent<Image>().color = GUIColor;
        ChampBarFront.GetComponent<Image>().color = GUIColor;
        CR.GetComponent<RawImage>().color = GUIColor;

        if (IsOn == true)
        {
            DescIM.SetActive(true);
            if (CursorOn == "Passive")
            {
                Desc.GetComponent<TextMeshProUGUI>().text = PassiveD;
            }

            else if (CursorOn == "Slot1C")
            {
                Desc.GetComponent<TextMeshProUGUI>().text = SkillQD;
            }

            else if (CursorOn == "Slot2C")
            {
                Desc.GetComponent<TextMeshProUGUI>().text = SkillWD;
            }

            else if (CursorOn == "Slot3C")
            {
                Desc.GetComponent<TextMeshProUGUI>().text = SkillED;
            }

            else if (CursorOn == "Slot4C")
            {
                Desc.GetComponent<TextMeshProUGUI>().text = SkillRD;
            }
        }
        else if (IsOn == false)
        {
            DescIM.SetActive(false);
        }
    }
}
