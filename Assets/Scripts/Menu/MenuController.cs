using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static Dictionary;


public class MenuController : MonoBehaviour
{
    #region List
    [Header("Główne")]
    public GameObject Content;
    [Header("Lista")]
    public TMP_InputField IP;
    public TMP_InputField Port;
    [Header("Prawa Ramka")]
    public GameObject Game;
    public GameObject GameOpt;
    public GameObject LobbyOpt;
    public GameObject Lobby;
    public GameObject Checkbox;
    public TMP_InputField PlayerSize;
    public TMP_InputField GameName;
    public TMP_InputField GamePass;
    [Header("Menu")]
    public GameObject Buttons;
    public GameObject List;
    public Image Dot;
    public GameObject Avatar;
    public GameObject Name;
    public RawImage MenuTeamColor;
    [Header("TeamColors")]
    public Image First;
    public Image Second;
    public Slider Red;
    public Slider Green;
    public Slider Blue;
    public GameObject ColorPicker;
    public TMP_InputField Team1;
    public TMP_InputField Team2;
    [Header("Inne")]
    public Animator anim;
    public bool ArrowSide = false;
    public bool Check = false;
    public bool Mode = false;
    public bool Block = false;
    public bool Switch = false;
    public bool Colour = false;
    public bool DevMode = false;
    #endregion List

    void Start()
    {
        anim.Play("Exit");
        First.color = Data.FirstTeamColor;
        Second.color = Data.SecondTeamColor;
    }

    void Update()
    {
        RoomName = GameName.text;

        if (MenuTeamColor != null)
        {
            if (!Team && Online) //Kolor klienta
                MenuTeamColor.color = TeamOneColor;
            else if (Team && Online)
                MenuTeamColor.color = TeamTwoColor;
            else if (!Online)
                MenuTeamColor.color = new Color(1,1,1,1);
        }

        if (Discord.Nickname == "" && !DevMode)
        {
            Dictionary.Name = "Unconnected";
            Name.GetComponent<TextMeshProUGUI>().text = Dictionary.Name;
            Dot.color = new Color32(255, 0, 0, 255);
        }
        else if (Discord.Nickname != "" && !DevMode && Discord.Nickname != Dictionary.Name)
        {
            Dictionary.Name = Discord.Nickname;
            Name.GetComponent<TextMeshProUGUI>().text = Dictionary.Name;
        }
        else if (DevMode)
        {
            Dictionary.Name = "Testowa nazwa";
            Name.GetComponent<TextMeshProUGUI>().text = Dictionary.Name;
        }

        if (Dictionary.Name == "Unconnected" && !DevMode)
        {
            Avatar.GetComponent<Image>().sprite = Resources.Load<Sprite>("Logo");
            print("Avatar sztuczny");
        }
        else if (Dictionary.Name != "Unconnected" && !DevMode)
        {
            Avatar.GetComponent<Image>().sprite = Dictionary.Avatar;
        }
        else if (DevMode)
        {
            Avatar.GetComponent<Image>().sprite = Resources.Load<Sprite>("Checkoff");
        }

        if (Dictionary.Name != "Unconnected" && Hosting == false && Connect == false)
            Dot.color = new Color32(255, 255, 0, 255);
        else if (Hosting == true || Connect == true)
            Dot.color = new Color32(0, 255, 0, 255);

        if (int.Parse(PlayerSize.text) > 10)
        {
            PlayerSize.text = "10";
            MaxPlayers = int.Parse(PlayerSize.text);
        }
        else if (PlayerSize.text == "")
        {
            PlayerSize.text = "2";
            MaxPlayers = int.Parse(PlayerSize.text);
        }
        else if (int.Parse(PlayerSize.text) < 2)
        {
            PlayerSize.text = "2";
            MaxPlayers = int.Parse(PlayerSize.text);
        } 

        if (Hosting == true || Connect == true)
        {
            if (!Block)
            {
                Game.GetComponent<Button>().interactable = true;
                Lobby.GetComponent<Button>().interactable = true;
                InfoLobby();
                anim.Play("MenuClose");
                ArrowSide = true;
                Block = true;
            }
        }
        else
        {
            Game.GetComponent<Button>().interactable = false;
            Lobby.GetComponent<Button>().interactable = false;
            GameOpt.SetActive(false);
            LobbyOpt.SetActive(false);
        }

        if (Hosting)
        {
            GameName.readOnly = false;
            GamePass.readOnly = false;
            PlayerSize.readOnly = false;
            Checkbox.GetComponent<Button>().interactable = true;
            Red.interactable = true;
            Green.interactable = true;
            Blue.interactable = true;
            Data.FirstTeamColor = TeamOneColor;
            Data.SecondTeamColor = TeamTwoColor;
        }
        else
        {
            GameName.readOnly = true;
            GamePass.readOnly = true;
            PlayerSize.readOnly = true;
            Checkbox.GetComponent<Button>().interactable = false;
            Red.interactable = false;
            Green.interactable = false;
            Blue.interactable = false;
        }

        if (Check)
        {
            Checkbox.GetComponent<Image>().sprite = Resources.Load<Sprite>("CheckOn");
        }
        else if (!Check)
        {
            Checkbox.GetComponent<Image>().sprite = Resources.Load<Sprite>("CheckOff");
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ColorPicker.SetActive(false);
        }

        if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.V) && !DevMode)
        {
            DevMode = true;
        }
        else if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.V) && DevMode)
        {
            DevMode = false;
        }
    }

    public void RightMenu()
    {
        if (ArrowSide)
        {
            anim.Play("MenuOpen");
            ArrowSide = false;
            ColorPicker.SetActive(false);
        }
        else if (!ArrowSide)
        {
            anim.Play("MenuClose");
            ArrowSide = true;
            ColorPicker.SetActive(false);
        }
    }

    public void InfoLobby()
    {
        GameOpt.SetActive(false);
        LobbyOpt.SetActive(true);
        Game.GetComponent<Image>().color = new Color32(170, 170, 170, 230);
        Lobby.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
    }

    public void InfoGame()
    {
        GameOpt.SetActive(true);
        LobbyOpt.SetActive(false);
        Lobby.GetComponent<Image>().color = new Color32(170, 170, 170, 230);
        Game.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
    }

    public void ListOpen()
    {
        Buttons.SetActive(false);
        List.SetActive(true);
    }

    public void ListClose()
    {
        Buttons.SetActive(true);
        List.SetActive(false);
    }

    public void OnCheckboxClick()
    {
        if (Check)
        { Check = false; }
        else if (!Check)
        { Check = true; }
    }

    public void SwitchFirst()
    {
        if (Hosting)
        {
            Switch = false;
            ChangeColor();
        }
    }
    public void SwitchSecond()
    {
        if (Hosting)
        {
            Switch = true;
            ChangeColor();
        }
    }
    public void ChangeColor()
    {
        if (!Switch) //1 Team
        {
            Colour = false;
            ColorPicker.SetActive(true);
            Red.value = TeamOneColor.r;
            Green.value = TeamOneColor.g;
            Blue.value = TeamOneColor.b;
            TeamOneColor.r = 1;
            print("Zmiana na " + TeamOneColor);
            Colour = true;
        }
        else //2 Team
        {
            Colour = false;
            ColorPicker.SetActive(true);
            Red.value = TeamTwoColor.r;
            Green.value = TeamTwoColor.g;
            Blue.value = TeamTwoColor.b;
            TeamTwoColor.r = 1;
            print("Zmiana na "+ TeamTwoColor);
            Colour = true;
        }
    }

    public void UpdateColor()
    {
        if (!Switch && Colour)
        {
            TeamOneColor = new Color(Red.value, Green.value, Blue.value, 1);
            First.color = TeamOneColor; //Ustawianie bloczku 1
            print("Zmiana na " + TeamOneColor + "UpdateColor 1");
        }
        else if (Switch && Colour)
        {
            TeamTwoColor = new Color(Red.value, Green.value, Blue.value, 1);
            Second.color = TeamTwoColor; //Ustawianie bloczku 2
            print("Zmiana na " + TeamTwoColor + "UpdateColor 2");
        }
    }
    public void TeamSynchro()
    {
        First.color = TeamOneColor;
        Second.color = TeamTwoColor;
        print("TeamSynchro");
    }
}