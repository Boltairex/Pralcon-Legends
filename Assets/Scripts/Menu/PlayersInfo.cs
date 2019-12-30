using UnityEngine;
using UnityEngine.Networking;

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

    [SyncVar] public bool Team;

    public bool Ready;
    public bool Init;

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
        {
            NetR.LocalPlayer = gameObject;
        }
        else
        {
            Ready = false;
        }
        NetR.Range = 0;
        OnRefreshBarsCount();
    }

    private void OnDestroy() //Usuwanie gracza
    {
        NetR.Range = 0;
        OnRefreshBarsCount();
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

    public void OnRefreshBarsCount()
    {
        Players = GameObject.FindGameObjectsWithTag("PlayerBar");
        GameObject[] PlayersINF = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < Players.Length; i++)
        {
            Destroy(Players[i]);
            PlayersINF[i].GetComponent<PlayersInfo>().Ready = false;

        }
        NetR.Range = 0;
    }

    public void Update()
    {
        if (!Ready && !isLocalPlayer)
        {
            Ready = true;
            if (Name == null || Avatar == null)
            {
                Name = Menu.DSName;
                Avatar = Menu.DSAvatar;
            }
            NetR.CreatePlayer(gameObject);
        }

        if (Bar == null)
        {
            Ready = false;
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
}
