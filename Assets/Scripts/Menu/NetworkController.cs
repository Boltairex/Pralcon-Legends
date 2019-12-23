using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;

public class NetworkController : NetworkManager
{
    public MenuController MenuC;
    public GameObject BarPref;
    public DataManagement Data;
    public MenuGUI MenuG;
    public PRDiscordRPC Discord;

    #region List
    [HideInInspector] public PlayersInfo LocalPlayer;
    [HideInInspector] public int Range = 0;
    [HideInInspector] public bool Hosting = false;
    [HideInInspector] public bool Connect = false;
    public GameObject PlayerJoin;
    public bool PTeam;
    public Color FirstTeam;
    public Color SecondTeam;
    public string Team1;
    public string Team2;
    #endregion List

    void Update()
    {
        if (Hosting || Connect)
        {
            Discord.InLobby = true;
        }
        else
        {
            Discord.InLobby = false;
        }
    }

    public void CreateBar(string name, Sprite PImage, GameObject owner)
    {
        PlayerJoin = Instantiate(BarPref);
        PlayerJoin.transform.SetParent(MenuC.Content.transform);
        PlayerJoin.GetComponent<RectTransform>().anchoredPosition = new Vector3(-210f, 90f + -140 * Range, 0);
        Range++;
        PlayerJoin.transform.localScale = new Vector3(1, 1, 1);
        PlayerJoin.GetComponent<BarSync>().Owner = owner;
        PlayerJoin.GetComponentInChildren<Image>().sprite = PImage;
        PlayerJoin.GetComponentInChildren<TextMeshProUGUI>().text = name;
        owner.GetComponent<PlayersInfo>().Bar = PlayerJoin;
        PlayerJoin.GetComponent<BarSync>().NetC = gameObject.GetComponent<NetworkController>();
    }

    public void ServerStart()
    {
        if (!Hosting && !Connect && MenuC.DSName != "Connecting...")
        {
            if (MenuC.Port.text == "" || MenuC.Port.text == null)
            { networkPort = 7777; }
            else
            { networkPort = int.Parse(MenuC.Port.text); }

            networkAddress = "localhost";

            Connect = false;
            Hosting = true;
            StartHost();
            MenuG.ListClose();
        }
    }

    public void ServerConnection()
    {
        if (!Hosting && !Connect && MenuC.DSName != "Connecting...")
        {
            if (MenuC.Port.text == "" || MenuC.Port.text == null)
            { networkPort = 7777; }
            else
            { networkPort = int.Parse(MenuC.Port.text); }

            networkAddress = MenuC.IP.text;
            if (MenuC.IP.text == "") { networkAddress = "localhost"; }

            Connect = true;
            Hosting = false;
            StartClient();
            MenuG.ListClose();
        }
    }

    public void CloseConnection()
    {
        if (Hosting || Connect)
        {
            if (!Hosting) { StopClient(); }
            else { StopHost(); }
        }
    }

    public void ChangeTeam()
    {
        if (!PTeam)
        {
            PTeam = true;
            if (PlayerJoin != null)
            {
                PlayerJoin.GetComponent<PlayersInfo>().CmdInfoSender(PTeam);
            }
        }
        else if (PTeam)
        {
            PTeam = false;
            if (PlayerJoin != null)
            {
                PlayerJoin.GetComponent<PlayersInfo>().CmdInfoSender(PTeam);
            }
        }
    }
    public void LobbyRun()
    {
        FirstTeam = MenuC.FirstColour;
        SecondTeam = MenuC.SecondColour;
        Team1 = MenuC.Team1.text;
        Team2 = MenuC.Team2.text;
        ServerChangeScene("Lobby");
    }
    public void GameRun()
    {
        ServerChangeScene("Game");
    }
}
