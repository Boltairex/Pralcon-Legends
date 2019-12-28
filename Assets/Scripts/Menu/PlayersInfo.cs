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

    public string Name;
    public Sprite Avatar;
    public Color TeamColor;

    public bool Ready;
    public bool Init;
    public bool Team;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        Menu = GameObject.Find("MenuController").GetComponent<MenuController>();
        NetC = GameObject.Find("LobbyManager").GetComponent<NetworkController>();
        NetR = GameObject.Find("NetworkContainer").GetComponent<NetworkContainer>();
        if (isLocalPlayer)
        { NetR.LocalPlayer = gameObject.GetComponent<PlayersInfo>(); }
    }

    private void OnDestroy() //Usuwanie gracza
    {
        Destroy(NetR.LocalPlayerBar);
    }

    void Update()
    {
        if (!Ready && !isLocalPlayer)
        {
            NetR.BarSync(gameObject.GetComponent<PlayersInfo>());
            NetR.Players = GameObject.FindGameObjectsWithTag("Player");
            Ready = true;
        }

        if (Menu.MenuTeamColor != null && isLocalPlayer)
        {
            Team = NetR.Team;
            if (!Team) //Kolor klienta
            {
                Menu.MenuTeamColor.color = Menu.FirstColour;
            }
            else
            {
                Menu.MenuTeamColor.color = Menu.SecondColour;
            }
        }
    }
}
