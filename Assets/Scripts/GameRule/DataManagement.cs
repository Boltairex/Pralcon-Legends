using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;

public class DataManagement : NetworkBehaviour
{
    //Skrypt wysyłający z serwera do klienta
    public MenuController MenuC;
    public NetworkController NetC;
    public NetworkContainer NetR;

    public GameObject[] Players;

    public GameObject BarPref;

    [Header("Menu")]
    [SyncVar] public string FirstTeamName;
    [SyncVar] public string SecondTeamName;
    [SyncVar] public Color FirstTeamColor;
    [SyncVar] public Color SecondTeamColor;
    [SyncVar] public string GameName;
    [SyncVar] public string GamePass;
    [SyncVar] public string PlayerMax;
    [SyncVar] public bool Check;
    [SyncVar] public int Range = 0;
    //[Header("Menu")]

    void Update()
    {
        if (!NetC.Hosting && SceneManager.GetActiveScene().name == "Menu")
        {
            MenuC.Team1.text = FirstTeamName;
            MenuC.Team2.text = SecondTeamName;
            MenuC.FirstColour = FirstTeamColor;
            MenuC.SecondColour = SecondTeamColor;
            MenuC.TeamSynchro();
            MenuC.GameName.text = GameName;
            MenuC.GamePass.text = GamePass;
            MenuC.PlayerSize.text = PlayerMax;
            MenuC.Check = Check;
        }
        else if (NetC.Hosting && SceneManager.GetActiveScene().name == "Menu")
        {
            FirstTeamName = MenuC.Team1.text;
            SecondTeamName = MenuC.Team2.text;
            FirstTeamColor = MenuC.FirstColour;
            SecondTeamColor = MenuC.SecondColour;
            GameName = MenuC.GameName.text;
            GamePass = MenuC.GamePass.text;
            PlayerMax = MenuC.PlayerSize.text;
            Check = MenuC.Check;
        }
    }

    public void CreatePlayer(PlayersInfo Owner)
    {
        GameObject Player = Instantiate(BarPref);
        Player.transform.SetParent(MenuC.Content.transform);
        Player.GetComponent<RectTransform>().anchoredPosition = new Vector3(-210f, 90f + -140 * Range, 0);
        Range++;
        Player.transform.localScale = new Vector3(1, 1, 1);
        Owner.Bar = Player;
        Player.GetComponent<BarSync>().Owner = Owner;
    }
}
