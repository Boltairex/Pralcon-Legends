using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class PlayersInfo : NetworkBehaviour
{
    MenuController Menu;
    NetworkController NetC;
    public GameObject Bar;
    public Sprite Avatar;
    public RawImage Color;
    PRDiscordRPC Discord;

    [SyncVar]
    public bool Team;
    public string Name;
    public bool Ready;
    bool Init;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        GameObject[] Players = GameObject.FindGameObjectsWithTag("PlayerBar");
        Menu = GameObject.Find("MenuController").GetComponent<MenuController>();
        NetC = GameObject.Find("LobbyManager").GetComponent<NetworkController>();
        Discord = GameObject.Find("MenuController").GetComponent<PRDiscordRPC>();
        Discord.Players = Players.Length + 1;
        for (int i = 0; i < Players.Length; i++) //Odświeżanie listy graczy
        {
            Destroy(Players[i]);
        }

        NetC.Range = 0;
        if (isLocalPlayer) //Synchronizowanie u klienta i update serwerowi
        {
            NetC.LocalPlayer = GetComponent<PlayersInfo>();
            Avatar = Menu.DSAvatar;
            Name = Menu.DSName;
            CmdInfoSync(Name, Avatar.texture.EncodeToJPG());
        }
        else //Synchronizowanie profilu klienta u graczy i update serwerowi
        {
            NetC.LocalPlayer.CmdInfoSync(NetC.LocalPlayer.Name, NetC.LocalPlayer.Avatar.texture.EncodeToJPG());
        }
    }

    private void OnDestroy() //Usuwanie gracza
    {
        Destroy(Bar);
    }

    [Command] //Wysyłanie z klienta do serwera (Wysyłanie danych z gracza)
    public void CmdInfoSync(string SendName, byte[] SendAvatar)
    {
        Name = SendName;
        RpcInfoSync(SendName, SendAvatar);
    }

    [ClientRpc] //Wysyłanie z serwera do klienta (Odbieranie danych u gracza)
    void RpcInfoSync(string GetName, byte[] GetAvatar)
    {
        Name = GetName;
        Texture2D recAv = new Texture2D(1, 1);
        recAv.LoadImage(GetAvatar);
        Avatar = Sprite.Create(recAv, new Rect(new Rect(0.0f, 0.0f, recAv.width, recAv.height)), new Vector2(0.5f, 0.5f), 100.0f);
        Ready = true;
        Init = false;
    }

    void Update()
    {
        if (!isLocalPlayer && Ready && !Init)
        {
            NetC.CreateBar(Name, Avatar, gameObject);
            Init = true;
        }

        if (Color == null && !isLocalPlayer)
        { Color = Bar.GetComponentInChildren<RawImage>(); }
        else if (Color != null && !isLocalPlayer)
        {
            if (!Team) //Synchronizacja koloru w Baru
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
            Team = NetC.PTeam;
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

    [Command] //Wysyłanie z klienta do serwera (Wysyłanie danych z gracza) SERVER ONLY
    public void CmdInfoSender(bool SendTeam)
    {
        Team = SendTeam;
        RpcInfoGet(SendTeam);
    }

    [ClientRpc]
    public void RpcInfoGet(bool GetTeam) //odbieranie
    {
        Team = GetTeam;
    }
}
