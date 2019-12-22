using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    #endregion List
    bool ArrowSide;
    public bool Check;
    bool Mode;
    bool Block;
    bool Switch;

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
        if (Discord.Nickname == "")
        {
            DSName = "Connecting...";
            Dot.color = new Color32(255, 0, 0, 255);
        }
        else
        {
            DSName = Discord.Nickname; 
        }

        if (DSName != "Connecting..." && NetC.Hosting == false && NetC.Connect == false)
        { Dot.color = new Color32(255, 255, 0, 255); }
        else if (NetC.Hosting == true || NetC.Connect == true)
        { Dot.color = new Color32(0, 255, 0, 255); }

        Name.GetComponent<TextMeshProUGUI>().text = DSName;
        DSAvatar = Discord.Avatar;
        Avatar.GetComponent<Image>().sprite = DSAvatar;
        Discord.RoomName = GameName.text;
        Discord.MaxPlayers = int.Parse(PlayerSize.text);

        First.color = new Color(Red.value, Green.value, Blue.value, 1);

        if (Avatar.GetComponent<Image>().sprite != null)
        { Avatar.GetComponent<Image>().color = new Color(1, 1, 1, 1); }

        if (Input.GetKeyDown(KeyCode.Semicolon) || Input.GetKeyUp(KeyCode.Semicolon))
        {
            Port.ActivateInputField();
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

        if (int.Parse(PlayerSize.text) > 10)
        { PlayerSize.text = "10"; }
        else if (PlayerSize.text == "")
        { PlayerSize.text = "2"; }

        if (NetC.Hosting)
        {
            GameName.readOnly = false;
            GamePass.readOnly = false;
            PlayerSize.readOnly = false;
            Checkbox.GetComponent<Button>().interactable = true;
            Red.interactable = true;
            Green.interactable = true;
            Blue.interactable = true;
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

        if (Input.GetKeyDown(KeyCode.R) && NetC.Hosting)
        {
            SyncInfo();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ColorPicker.SetActive(false);
        }
    }

    public void RightMenu()
    {
        if (ArrowSide)
        {
            anim.Play("MenuOpen");
            ArrowSide = false;
        }
        else if (!ArrowSide)
        {
            anim.Play("MenuClose");
            ArrowSide = true;
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
        {
            Check = false;
            Checkbox.GetComponent<Image>().sprite = Resources.Load<Sprite>("CheckOn");
        }
        else if (!Check)
        {
            Check = true;
            Checkbox.GetComponent<Image>().sprite = Resources.Load<Sprite>("CheckOff");
        }
    }
    public void SyncInfo()
    {
        Data.RpcGameInfo(GameName.text, GamePass.text, PlayerSize.text, Checkbox);
        Data.RpcTeamSynchronise(Team1.text, Team2.text, FirstColour, SecondColour);
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
            ColorPicker.SetActive(true);
            Red.value = FirstColour.r;
            Green.value = FirstColour.g;
            Blue.value = FirstColour.b;
            First.color = FirstColour;
        }
        else //2 Team
        {
            ColorPicker.SetActive(true);
            Red.value = SecondColour.r;
            Green.value = SecondColour.g;
            Blue.value = SecondColour.b;
            Second.color = SecondColour;
        }
    }
    public void UpdateColor()
    {
        if (!Switch)
        {
            FirstColour = new Color(Red.value, Green.value, Blue.value, 1);
            First.color = FirstColour;
        }
        else
        {
            SecondColour = new Color(Red.value, Green.value, Blue.value, 1);
            Second.color = SecondColour;
        }
    }
}

