using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightMenu : MonoBehaviour
{
    [Header("GameSettings")]
    public GameObject GS;

    [Header("PlayerList")]
    public GameObject PL;

    protected BarModes modes = BarModes.PlayerList;
    public BarModes Modes
    {
        get { return modes; }
        set { modes = value;
        UpdateGUI();}
    }

    public void SwitchTo(int x)
    {
        if (x == 0)
            Modes = BarModes.GameSettings;
        else if(x == 1)
            Modes = BarModes.PlayerList;
    }

    void UpdateGUI()
    {
        if(Modes == BarModes.GameSettings)
        {
            GS.SetActive(true);
            PL.SetActive(false);
        }
        else if (Modes == BarModes.PlayerList)
        {   
            PL.SetActive(true);
            GS.SetActive(false);
        }
    }
}

public enum BarModes
{
    GameSettings,
    PlayerList
}