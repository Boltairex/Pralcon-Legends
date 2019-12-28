using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkContainer : NetworkBehaviour
{
    //Skrypt wysyłający z klienta do serwera
    //Nie dawać SyncVarów

    public PRDiscordRPC Discord;
    public MenuController MenuC;
    public NetworkController NetC;
    public DataManagement Data;

    public bool Team;
    public string Name;
    public Sprite Avatar;
    public Color FirstTeam;
    public Color SecondTeam;

    public bool Init;

    public GameObject[] Players;
    public BarSync LocalPlayerBar;
    public PlayersInfo LocalPlayer;

    void Start()
    {
        /*
        Texture2D recAv = new Texture2D(1, 1);
        //recAv.LoadImage(GetAvatar);
        Avatar = Sprite.Create(recAv, new Rect(new Rect(0.0f, 0.0f, recAv.width, recAv.height)), new Vector2(0.5f, 0.5f), 100.0f);
        */
    }

    void Update()
    {
        if (LocalPlayer != null)
        {
            LocalPlayer.Name = Name;
            LocalPlayer.Avatar = Avatar;
        }
    }

    public void ChangeColor()
    {
        if (Team && NetC.Online)
        {
            Team = false;
        }
        else if (!Team && NetC.Online)
        {
            Team = true;
        }
        CmdTeamSynchro(Team);
    }

    public void RefreshLobby()
    {
        Players = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < Players.Length; i++) //Odświeżanie listy graczy
        {
            Destroy(Players[i]);
        }
        LocalPlayer.Ready = false;
    }

    [Command]
    public void CmdTeamSynchro(bool SendTeam)
    {
        SendTeam = Team;
        Data.RpcPlayerTeamSynchro(LocalPlayer.gameObject, SendTeam);
    }

    public void BarSync(PlayersInfo LP)
    {
        Data.CreatePlayer(gameObject.GetComponent<NetworkContainer>());
    }
}
