using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;
using System.Collections;
using static Dictionary;

public class DataManagement : NetworkBehaviour
{
    [Header("Menu")]
    [SyncVar] public string FirstTeamName;
    [SyncVar] public string SecondTeamName;
    [SyncVar] public Color FirstTeamColor;
    [SyncVar] public Color SecondTeamColor;
    [SyncVar] public string GameName;
    [SyncVar] public string GamePass;
    [SyncVar] public string PlayerMax;
    [SyncVar] public bool Check;
    //[Header("Lobby")]

    private void Start()
    {
        FirstTeamColor = new Color(1, 0, 0, 1);
        SecondTeamColor = new Color(0, 0, 1, 1);
    }

    void Update()
    {
        if (Hosting && SceneManager.GetActiveScene().name == "Menu" && Online)
        {
            FirstTeamName = MenuC.Team1.text;
            SecondTeamName = MenuC.Team2.text;
            FirstTeamColor = TeamOneColor;
            SecondTeamColor = TeamTwoColor;
            GameName = MenuC.GameName.text;
            GamePass = MenuC.GamePass.text;
            PlayerMax = MenuC.PlayerSize.text;
            Check = MenuC.Check;
        }
        else if (!Hosting && SceneManager.GetActiveScene().name == "Menu" && Online)
        {
            MenuC.Team1.text = FirstTeamName;
            MenuC.Team2.text = SecondTeamName;
            TeamOneColor = FirstTeamColor;
            TeamTwoColor = SecondTeamColor;
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
            Texture2D recAv = new Texture2D(1, 1);
            recAv.LoadImage(SendAvatar);
            Owner.GetComponent<PlayersInfo>().PlayerAvatar = Sprite.Create(recAv, new Rect(new Rect(0.0f, 0.0f, recAv.width, recAv.height)), new Vector2(0.5f, 0.5f), 100.0f);
        }
    }

    [ClientRpc]
    public void RpcIdentity()
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
