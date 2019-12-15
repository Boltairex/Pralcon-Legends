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
    public GameObject FPS;

    public float sizeP;
    public float sizeQ;
    public float sizeW;
    public float sizeE;
    public float sizeR;
    public float size;

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

    [Header("Części cooldownowe")]
    public float PTime;
    public float QTime;
    public float WTime;
    public float ETime;
    public float RTime;

    public GameObject STimeGUIP;
    public GameObject STimeGUIQ;
    public GameObject STimeGUIW;
    public GameObject STimeGUIE;
    public GameObject STimeGUIR;

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
    }

    void Update()
    {
        if (PTime > 0)
        {
            float CD = Time.time - PTime;
            STimeGUIP.SetActive(true);
            Avatar.GetComponent<Image>().color = new Color32(200, 100, 100, 150);
            CD = -CD;
            STimeGUIP.GetComponent<TextMeshProUGUI>().text = CD.ToString("f1") + "s";
        }
        else if (PTime >= 0)
        {
            STimeGUIP.SetActive(false);
            Avatar.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        }

        if (QTime > 0)
        {
            float CD = Time.time - QTime;
            STimeGUIQ.SetActive(true);
            QSkill.GetComponent<Image>().color = new Color32(200, 100, 100, 150);
            CD = -CD;
            STimeGUIQ.GetComponent<TextMeshProUGUI>().text = CD.ToString("f1") + "s";
        }
        else if (QTime >= 0)
        {
            STimeGUIQ.SetActive(false);
            QSkill.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        }

        if (WTime > 0)
        {
            float CD = Time.time - WTime;
            STimeGUIW.SetActive(true);
            WSkill.GetComponent<Image>().color = new Color32(200, 100, 100, 150);
            CD = -CD;
            STimeGUIW.GetComponent<TextMeshProUGUI>().text = CD.ToString("f1") + "s";
        }
        else if (WTime >= 0)
        {
            STimeGUIW.SetActive(false);
            WSkill.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        }

        if (ETime > 0)
        {
            float CD = Time.time - ETime;
            STimeGUIE.SetActive(true);
            ESkill.GetComponent<Image>().color = new Color32(200, 100, 100, 150);
            CD = -CD;
            STimeGUIE.GetComponent<TextMeshProUGUI>().text = CD.ToString("f1") + "s";
        }
        else if (ETime >= 0)
        {
            STimeGUIE.SetActive(false);
            ESkill.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        }

        if (RTime > 0)
        {
            float CD = Time.time - RTime;
            STimeGUIR.SetActive(true);
            RSkill.GetComponent<Image>().color = new Color32(200, 100, 100, 150);
            CD = -CD;
            STimeGUIR.GetComponent<TextMeshProUGUI>().text = CD.ToString("f1") + "s";
        }
        else if (RTime >= 0)
        {
            STimeGUIR.SetActive(false);
            RSkill.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        }

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
                //DescIM.transform.localScale = new Vector3(1 * sizeP, 1 * sizeP, 1);
            }

            else if (CursorOn == "Slot1C")
            {
                Desc.GetComponent<TextMeshProUGUI>().text = SkillQD;
                //DescIM.transform.localScale = new Vector3(1 * sizeQ, 1 * sizeQ, 1);
            }

            else if (CursorOn == "Slot2C")
            {
                Desc.GetComponent<TextMeshProUGUI>().text = SkillWD;
                //DescIM.transform.localScale = new Vector3(1 * sizeW, 1 * sizeW, 1);
            }

            else if (CursorOn == "Slot3C")
            {
                Desc.GetComponent<TextMeshProUGUI>().text = SkillED;
                //DescIM.transform.localScale = new Vector3(1 * sizeE, 1 * sizeE, 1);
            }

            else if (CursorOn == "Slot4C")
            {
                Desc.GetComponent<TextMeshProUGUI>().text = SkillRD;
                //DescIM.transform.localScale = new Vector3(1 * sizeR, 1 * sizeR, 1);
            }
        }
        else if (IsOn == false)
        {
            DescIM.SetActive(false);
        }
    }
}
