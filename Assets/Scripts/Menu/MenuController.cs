using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

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
    public Color FirstColour = new Color(1, 0, 0, 1);
    public Color SecondColour = new Color(0, 0, 1, 1);
    public TMP_InputField Team1;
    public TMP_InputField Team2;
    [Header("Inne")]
    public Sprite DSAvatar;
    public string DSName;
    public Animator anim;
    public NetworkController NetC;
    public DataManagement Data;
    public NetworkContainer NetR;
    #endregion List
    bool ArrowSide;
    public bool Check;
    bool Mode;
    bool Block;
    public bool Switch;
    bool Colour;
    public bool DevMode;

    PRDiscordRPC Discord;

    void Start()
    {
        anim.Play("Exit");
        Discord = gameObject.GetComponent<PRDiscordRPC>();
        First.color = FirstColour;
        Second.color = SecondColour;
    }

    void Update()
    {
        if (Discord.Nickname == "" && !DevMode)
        {
            DSName = "Connecting...";
            Dot.color = new Color32(255, 0, 0, 255);
        }
        else if (Discord.Nickname != "" && !DevMode && Discord.Nickname != DSName)
        {
            DSName = Discord.Nickname;
            Name.GetComponent<TextMeshProUGUI>().text = DSName;
        }
        else if (DevMode)
        {
            DSName = "Testowa nazwa";
            Name.GetComponent<TextMeshProUGUI>().text = DSName;
        }

        if (Discord.Avatar != null && !DevMode && Discord.Avatar != DSAvatar)
        {
            DSAvatar = Discord.Avatar;
            Avatar.GetComponent<Image>().sprite = DSAvatar;
        }
        else if (DevMode)
        {
            DSAvatar = Resources.Load<Sprite>("Checkoff");
            Avatar.GetComponent<Image>().sprite = DSAvatar;
        }

        if (DSName != "Connecting..." && NetC.Hosting == false && NetC.Connect == false)
        { Dot.color = new Color32(255, 255, 0, 255); }
        else if (NetC.Hosting == true || NetC.Connect == true)
        { Dot.color = new Color32(0, 255, 0, 255); }

        Discord.RoomName = GameName.text;
        Discord.MaxPlayers = int.Parse(PlayerSize.text);

        if (int.Parse(PlayerSize.text) > 10)
        { PlayerSize.text = "10"; }
        else if (PlayerSize.text == "")
        { PlayerSize.text = "2"; }

        if (SceneManager.GetActiveScene().name == "Menu")
        {
            if (Data == null)
            {
                Data = GameObject.Find("DataManager").GetComponent<DataManagement>();
            }

            if (NetC == null)
            {
                NetC = GameObject.Find("LobbyManager").GetComponent<NetworkController>();
            }

            if (NetR == null)
            {
                NetR = GameObject.Find("NetworkContainer").GetComponent<NetworkContainer>();
            }

            if (Input.GetKeyDown(KeyCode.Semicolon) || Input.GetKeyUp(KeyCode.Semicolon))
            {
                Port.ActivateInputField();
            }
        }
        if (NetC.Hosting == true || NetC.Connect == true)
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

        if (NetC.Hosting)
        {
            GameName.readOnly = false;
            GamePass.readOnly = false;
            PlayerSize.readOnly = false;
            Checkbox.GetComponent<Button>().interactable = true;
            Red.interactable = true;
            Green.interactable = true;
            Blue.interactable = true;
            Data.FirstTeamColor = FirstColour;
            Data.SecondTeamColor = SecondColour;
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

    public void OnCheckboxClick()
    {
        if (Check)
        { Check = false; }
        else if (!Check)
        { Check = true; }
    }

    public void SwitchFirst()
    {
        if (NetC.Hosting)
        {
            Switch = false;
            ChangeColor();
        }
    }
    public void SwitchSecond()
    {
        if (NetC.Hosting)
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
            Red.value = FirstColour.r;
            Green.value = FirstColour.g;
            Blue.value = FirstColour.b;
            print("Zmiana na " + FirstColour);
            Colour = true;
        }
        else //2 Team
        {
            Colour = false;
            ColorPicker.SetActive(true);
            Red.value = SecondColour.r;
            Green.value = SecondColour.g;
            Blue.value = SecondColour.b;
            print("Zmiana na "+ SecondColour);
            Colour = true;
        }
    }
    public void UpdateColor()
    {
        if (!Switch && Colour)
        {
            FirstColour = new Color(Red.value, Green.value, Blue.value, 1);
            First.color = FirstColour; //Ustawianie bloczku 1
            print("Zmiana na " + FirstColour);
        }
        else if (Switch && Colour)
        {
            SecondColour = new Color(Red.value, Green.value, Blue.value, 1);
            Second.color = SecondColour; //Ustawianie bloczku 2
            print("Zmiana na " + SecondColour);
        }
    }
    public void TeamSynchro()
    {
        First.color = FirstColour;
        Second.color = SecondColour;
    }
}