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

    [Header("Menu")]
    [SyncVar] public string FirstTeamName;
    [SyncVar] public string SecondTeamName;
    [SyncVar] public Color FirstTeamColor;
    [SyncVar] public Color SecondTeamColor;
    [SyncVar] public string GameName;
    [SyncVar] public string GamePass;
    [SyncVar] public string PlayerMax;
    [SyncVar] public bool Check;
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

    [ClientRpc]
    public void RpcTeamSynchro(bool GetTeam, GameObject GetPlayer)
    {
        GetPlayer.GetComponent<PlayersInfo>().Team = GetTeam;
    }

    [ClientRpc]
    public void RpcResendAvatar(byte[] SendAvatar, GameObject Owner)
    {
        Texture2D recAv = new Texture2D(1, 1);
        recAv.LoadImage(SendAvatar);
        Owner.GetComponent<PlayersInfo>().Avatar = Sprite.Create(recAv, new Rect(new Rect(0.0f, 0.0f, recAv.width, recAv.height)), new Vector2(0.5f, 0.5f), 100.0f);
    }
}
