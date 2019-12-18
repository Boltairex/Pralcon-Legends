using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuController : MonoBehaviour
{
    public GameObject Avatar;
    public GameObject Name;
    public GameObject Game;
    public GameObject Lobby;
    public GameObject GameOpt;
    public GameObject LobbyOpt;
    public GameObject Content;
    public GameObject Checkbox;

    public GameObject BarPref;

    public Animator anim;
    public int Range = 0;

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
        Name.GetComponent<TextMeshProUGUI>().text = Discord.Nickname;
        Avatar.GetComponent<Image>().sprite = Discord.Avatar;
        if (Avatar.GetComponent<Image>().sprite != null)
        { Avatar.GetComponent<Image>().color = new Color(1, 1, 1, 1); }

        if (Input.GetMouseButtonDown(1))
        { OnPlayerJoin(); }
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

    public void OnPlayerJoin()
    {
        GameObject PlayerJoin = Instantiate<GameObject>(BarPref);
        PlayerJoin.transform.SetParent(Content.transform);
        PlayerJoin.GetComponent<RectTransform>().anchoredPosition = new Vector3(-210f, 90f + -140 * Range, 0);
        Range++;
        PlayerJoin.transform.localScale = new Vector3(1, 1, 1);
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

