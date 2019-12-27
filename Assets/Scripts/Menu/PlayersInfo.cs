using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayersInfo : NetworkBehaviour
{
    MenuController Menu;
    NetworkController NetC;
    NetworkContainer NetR;
    LobbyController LobbyC;

    public GameObject Bar;
    public Sprite Avatar; 
    public RawImage Color;

    public bool Team;
    public bool Ready;
    public bool Init;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        Menu = GameObject.Find("MenuController").GetComponent<MenuController>();
        NetC = GameObject.Find("LobbyManager").GetComponent<NetworkController>();
        NetR = GameObject.Find("NetworkContainer").GetComponent<NetworkContainer>();
        NetR.LocalPlayer = gameObject.GetComponent<PlayersInfo>();
    }

    private void OnDestroy() //Usuwanie gracza
    {
        Destroy(Bar);
    }

    void Update()
    {
        if (!Ready && !isLocalPlayer)
        {
            NetR.BarSync(gameObject.GetComponent<PlayersInfo>());
            NetR.Players = GameObject.FindGameObjectsWithTag("Player");
            Ready = true;
        }

        if (Color == null && !isLocalPlayer)
        {
            Color = Bar.GetComponentInChildren<RawImage>();
        }
        else if (Color != null && !isLocalPlayer)
        {
            if (!Team) //Synchronizacja koloru w Barze
            {
                Color.color = Menu.FirstColour;
            }
            else if (Team)
            {
                Color.color = Menu.SecondColour;
            }
        }

        if (Menu.MenuTeamColor != null && isLocalPlayer)
        {
            Team = NetR.Team;
            if (!Team) //Kolor klienta
            {
                Menu.MenuTeamColor.color = Menu.FirstColour;
            }
            else if (Team)
            {
                Menu.MenuTeamColor.color = Menu.SecondColour;
            }
        }
    }
}
