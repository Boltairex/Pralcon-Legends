using System.Collections;
using System.Collections.Generic;
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

    PRDiscordRPC Discord;

    void Start()
    {
        anim.Play("Exit");
        Discord = gameObject.GetComponent<PRDiscordRPC>();
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
        }
        else
        {
            GameName.readOnly = true;
            GamePass.readOnly = true;
            PlayerSize.readOnly = true;
            Checkbox.GetComponent<Button>().interactable = false;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            SyncInfo();
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
    }
}

