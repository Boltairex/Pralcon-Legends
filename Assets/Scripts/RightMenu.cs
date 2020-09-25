using UnityEngine;
using UnityEngine.UI;

public class RightMenu : MonoBehaviour
{
    [Header("GameSettings")]
    public GameObject GS;
    public Image FirstTeam;
    public Image SecondTeam;
    public ColorScript ColorS;
    public Button Disconnect;

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

    void Awake() => Dictionary.RMenu = this;

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && !Dictionary.ESCTimer && Dictionary.ColorActive)
        {
            Dictionary.ESCTimer = true;
            ColorS.gameObject.SetActive(false);
            Dictionary.ColorActive = false;
        }

        FirstTeam.color = Dictionary.TeamOneColor;
        SecondTeam.color = Dictionary.TeamTwoColor;    
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

    public void ActiveColors(bool t)
    {
        ColorS.gameObject.SetActive(true);
        ColorS.Team = t;
        Dictionary.ColorActive = true;
        ColorS.UpdateColors();
    }
}

public enum BarModes
{
    GameSettings,
    PlayerList
}