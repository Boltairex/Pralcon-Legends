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
    DataManagement Data;

    public GameObject[] Players;

    public GameObject Bar;
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
        Data = GameObject.Find("DataManager").GetComponent<DataManagement>();
        Name = Menu.DSName;
        Avatar = Menu.DSAvatar;
        if (isLocalPlayer)
        { NetR.LocalPlayer = gameObject; }

        Players = GameObject.FindGameObjectsWithTag("PlayerBar");
        for (int i = 0; i < Players.Length; i++)
        {
            Destroy(Players[i]);
        }
        NetR.Range = 0;

        if (!isLocalPlayer)
        {
            NetR.LocalPlayer.GetComponent<PlayersInfo>().Ready = false;
        }
    }

    private void OnDestroy() //Usuwanie gracza
    {
        Destroy(Bar);
        CmdRefreshBars();
    }

    public void Update()
    {
        if (!Ready && !isLocalPlayer)
        {
            if (Name == null || Avatar == null)
            {
                Name = Menu.DSName;
                Avatar = Menu.DSAvatar;
            }
            NetR.CreatePlayer(gameObject);
            Ready = true;
        }

        if (!isLocalPlayer)
        {
            if (!Team)
            {
                TeamColor = Menu.FirstColour;
            }
            else
            {
                TeamColor = Menu.SecondColour;
            }
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

    [Command]
    public void CmdTeamSynchro(bool SendTeam, GameObject SendPlayer)
    {
        if (!isLocalPlayer)
        {
            Team = SendTeam;
        }
        Data.RpcTeamSynchro(SendTeam, SendPlayer);
    }

    [Command]
    public void CmdRefreshBars()
    {
        RpcRefreshBars();
    }

    [ClientRpc]
    public void RpcRefreshBars()
    {
        NetR.Range = 0;
        Ready = false;
    }
}
