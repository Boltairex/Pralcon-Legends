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
    public GameObject Checkbox;
    public GameObject GameOpt;
    public GameObject LobbyOpt;
    public GameObject Lobby;
    [Header("Menu")]
    public GameObject Avatar;
    public GameObject Name;
    [Header("Inne")]
    public Sprite DSAvatar;
    public string DSName;

    #endregion List
    public Animator anim;

    bool ArrowSide;
    bool Check;
    bool Mode;

    PRDiscordRPC Discord;

    void Start()
    {
        anim.Play("Exit");
        Discord = gameObject.GetComponent<PRDiscordRPC>();
        InfoLobby();
    }

    void Update()
    {
        if (Discord.Nickname == "")
        {
            DSName = "Connecting...";
        }
        else
        {
            DSName = Discord.Nickname; 
        }

        Name.GetComponent<TextMeshProUGUI>().text = DSName;
        DSAvatar = Discord.Avatar;
        Avatar.GetComponent<Image>().sprite = DSAvatar;
        if (Avatar.GetComponent<Image>().sprite != null)
        { Avatar.GetComponent<Image>().color = new Color(1, 1, 1, 1); }

        if (Input.GetKeyDown(KeyCode.Semicolon) || Input.GetKeyUp(KeyCode.Semicolon))
        {
            Port.ActivateInputField();
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
}

