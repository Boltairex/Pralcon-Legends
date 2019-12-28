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
    public DataManagement Data;
    public MenuGUI MenuG;
    public PRDiscordRPC Discord;

    public bool Init = false;
    public bool Hosting = false;
    public bool Connect = false;
    public bool Online = false;

    void Update()
    {
        if (Hosting || Connect)
        {
            Discord.InLobby = true;
            Online = true;
        }
        else
        {
            Discord.InLobby = false;
            Online = false;
        }
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
            if (!Hosting)
            {
                Connect = false;
                Online = false;
                StopClient();
            }
            else
            {
                Hosting = false;
                Online = false;
                StopHost();
            }
        }
    }

    public void LobbyRun()
    {
        Init = true;
        ServerChangeScene("Lobby");
    }

    public void GameRun()
    {
        ServerChangeScene("Game");
    }
}
