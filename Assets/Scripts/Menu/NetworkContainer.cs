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

    public GameObject BarPref;

    public bool Team;
    public bool Init;

    public GameObject[] Players;
    public BarSync LocalPlayerBar;
    public GameObject LocalPlayer;

    public int Range = 0;

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
        if (!Init && LocalPlayer != null)
        {
            LocalPlayer.name = "host";//LocalPlayer.GetComponent<PlayersInfo>().Name;
            LocalPlayer.GetComponent<PlayersInfo>().Name = MenuC.DSName;
            LocalPlayer.GetComponent<PlayersInfo>().Avatar = MenuC.DSAvatar;
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

        if(LocalPlayer != null)
        {
            LocalPlayer.GetComponent<PlayersInfo>().CmdTeamSynchro(Team, LocalPlayer);
        }
        else
        { LocalPlayer = GameObject.Find("host"); }
    }

    public void CreatePlayer(GameObject Owner)
    {
        GameObject Player = Instantiate(BarPref);
        Player.transform.SetParent(MenuC.Content.transform);
        Player.GetComponent<RectTransform>().anchoredPosition = new Vector3(-210f, 90f + -140 * Range, 0);
        Range++;
        Player.transform.localScale = new Vector3(1, 1, 1);
        Owner.GetComponent<PlayersInfo>().Bar = Player;
        Player.GetComponent<BarSync>().Owner = Owner;
    }
}
