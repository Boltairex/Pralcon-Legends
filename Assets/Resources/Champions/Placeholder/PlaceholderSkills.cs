using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceholderSkills : MonoBehaviour
{
    GameObject Champion;
    GameObject MGUI;

    float CDP = 0;
    float CDQ = 0;
    float CDW = 0;
    float CDE = 0;
    float CDR = 0;

    public float Reduction = 1.45f; // Nie zmieniać

    public bool BlockQ;
    public bool BlockW;
    public bool BlockE;
    public bool BlockR;

    void Start()
    {
        MGUI = GameObject.Find("GUIMenu");
        Champion = GameObject.Find("Character");
    }

    void Update()
    {
        //Passive

        //Q
        if (Input.GetButtonDown("Q") && BlockQ == false || Input.GetButtonUp("Q") && BlockQ == false)
        {
            BlockQ = true;
            QSkill();
            CDQ = 48f / Reduction + Time.time;
            MGUI.GetComponent<GUISet>().QTime = CDQ;
        }

        else if (Time.time >= CDQ)
        {
            BlockQ = false;
            CDQ = 0;
            MGUI.GetComponent<GUISet>().QTime = CDQ;
        }
        //W
        if (Input.GetButtonDown("W") && BlockW == false || Input.GetButtonUp("W") && BlockW == false)
        {
            BlockW = true;
            WSkill();
            CDW = 3f / Reduction + Time.time;
            MGUI.GetComponent<GUISet>().WTime = CDW;
        }
        else if (Time.time >= CDW)
        {
            BlockW = false;
            CDW = 0;
            MGUI.GetComponent<GUISet>().WTime = CDW;
        }
        //E
        if (Input.GetButtonDown("E") && BlockE == false || Input.GetButtonUp("E") && BlockE == false)
        {
            BlockE = true;
            ESkill();
            CDE = 3f / Reduction + Time.time;
            MGUI.GetComponent<GUISet>().ETime = CDE;
        }
        else if (Time.time >= CDE)
        {
            BlockE = false;
            CDE = 0;
            MGUI.GetComponent<GUISet>().ETime = CDE;
        }
        //Ultimate
        if (Input.GetButtonDown("R") && BlockR == false || Input.GetButtonUp("R") && BlockR == false)
        {
            BlockR = true;
            RSkill();
            CDR = 3f / Reduction + Time.time;
            MGUI.GetComponent<GUISet>().RTime = CDR;
        }
        else if (Time.time >= CDR)
        {
            BlockR = false;
            CDR = 0;
            MGUI.GetComponent<GUISet>().RTime = CDR;
        }
    }

    public void PassiveSkill()
    {
        
    }

    public void QSkill()
    {

    }

    public void WSkill()
    {

    }

    public void ESkill()
    {

    }

    public void RSkill()
    {

    }
}
