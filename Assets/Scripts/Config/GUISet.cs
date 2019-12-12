using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUISet : MonoBehaviour
{
    [Header("Do kolorów")]
    public GameObject Bar;
    public GameObject ItemFrames;
    public GameObject ChampBar;
    public GameObject ChampBarFront;
    GameObject CR;

    [Header("Do ustawienia grafik")]
    public GameObject Avatar;
    public GameObject QSkill;
    public GameObject WSkill;
    public GameObject ESkill;
    public GameObject RSkill;

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
    }
}
