using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System.Collections;

public class DataManagement : NetworkBehaviour
{
    //Skrypt wysyłający z serwera do klienta
    public MenuController MenuC;
    public NetworkController NetC;
    public NetworkContainer NetR;

    public GameObject[] Players;

    [Header("Menu")]
    [SyncVar] public string FirstTeamName;
    [SyncVar] public string SecondTeamName;
    [SyncVar] public Color FirstTeamColor;
    [SyncVar] public Color SecondTeamColor;
    [SyncVar] public string GameName;
    [SyncVar] public string GamePass;
    [SyncVar] public string PlayerMax;
    [SyncVar] public bool Check;
    public Texture2D recAv;
    //[Header("Lobby")]

    void Update()
    {
        if (NetC.Hosting && SceneManager.GetActiveScene().name == "Menu")
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
        else if (!NetC.Hosting && SceneManager.GetActiveScene().name == "Menu")
        {
            MenuC.Team1.text = FirstTeamName;
            MenuC.Team2.text = SecondTeamName;
            MenuC.FirstColour = FirstTeamColor;
            MenuC.SecondColour = SecondTeamColor;
            MenuC.GameName.text = GameName;
            MenuC.GamePass.text = GamePass;
            MenuC.PlayerSize.text = PlayerMax;
            MenuC.Check = Check;
            if (MenuC.First.color != FirstTeamColor || MenuC.Second.color != SecondTeamColor)
            {
                MenuC.TeamSynchro();
            }
        }
    }

    [ClientRpc]
    public void RpcTeamSynchro(bool GetTeam, GameObject GetPlayer)
    {
        GetPlayer.GetComponent<PlayersInfo>().Team = GetTeam;
    }

    [ClientRpc]
    public void RpcResendAvatar(byte[] SendAvatar, GameObject Owner)
    {
        if(SendAvatar != null || Owner != null)
        {
            recAv = new Texture2D(1, 1);
            recAv.LoadImage(SendAvatar);
            Owner.GetComponent<PlayersInfo>().Avatar = Sprite.Create(recAv, new Rect(new Rect(0.0f, 0.0f, recAv.width, recAv.height)), new Vector2(0.5f, 0.5f), 100.0f);
        }
    }

    public void Identity()
    {
        StartCoroutine(SearchAwait());
        for (int i = 0; i < Players.Length; i++)
        {
            Players[i].GetComponent<PlayersInfo>().PIdentity = i;
        }
    }

    public IEnumerator SearchAwait()
    {
        yield return Players = GameObject.FindGameObjectsWithTag("Player");
    }
}
