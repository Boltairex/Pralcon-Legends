using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PlayersInfo : NetworkBehaviour
{
    MenuController Menu;
    NetworkController NetC;
    NetworkContainer NetR;
    LobbyController LobbyC;
    DataManagement Data;

    public GameObject Bar;
    public Sprite Avatar;
    public Color TeamColor;

    [SyncVar] public int PIdentity = 0;
    [SyncVar] public string Name;
    [SyncVar] public bool Team;

    GameObject[] Players;
    GameObject[] PlayersINF;


    byte[] ByteAvatar;
    public bool Ready = false;
    public bool Init = false;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        Menu = GameObject.Find("MenuController").GetComponent<MenuController>();
        NetC = GameObject.Find("LobbyManager").GetComponent<NetworkController>();
        NetR = GameObject.Find("NetworkContainer").GetComponent<NetworkContainer>();
        Data = GameObject.Find("DataManager").GetComponent<DataManagement>();

        StartCoroutine(ClientUpdates());

        if (isLocalPlayer)
        {
            ByteAvatar = Menu.DSAvatar.texture.EncodeToJPG();
            NetR.LocalPlayer = gameObject;
        }
        NetR.Range = 0;
        OnRefreshBarsCount();
    }

    private void OnDestroy() //Usuwanie gracza
    {
        NetR.Range = 0;
        OnRefreshBarsCount();
        StartCoroutine(ClientUpdates());
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
        StartCoroutine(PlayersUpdate());
        for (int i = 0; i < Players.Length; i++)
        {
            Destroy(Players[i]);
            PlayersINF[i].GetComponent<PlayersInfo>().Ready = false;
        }
        NetR.Range = 0;
    }

    public void Update()
    {
        if (!Ready)
        {
            if (!isLocalPlayer)
            {
                NetR.CreatePlayer(gameObject);
                Ready = true;
            }
        }

        if (!Init && isLocalPlayer)
        {
            CmdAvatarSync(ByteAvatar, gameObject, Name);
            Init = true;
        }

        if (Bar == null && !isLocalPlayer)
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

    [Command]
    public void CmdAvatarSync(byte[] GetAvatar, GameObject GetOwner, string GetName)
    {
        Name = GetName;
        Data.RpcResendAvatar(GetAvatar, GetOwner);
    }

    public IEnumerator ClientUpdates()
    {

        yield return new WaitForSeconds(3f);
        NetC.Players = GameObject.FindGameObjectsWithTag("Player").Length;
        CmdAvatarSync(ByteAvatar, gameObject, Name);
    }

    public IEnumerator PlayersUpdate()
    {
        yield return Players = GameObject.FindGameObjectsWithTag("PlayerBar");
        yield return PlayersINF = GameObject.FindGameObjectsWithTag("Player");
    }
}