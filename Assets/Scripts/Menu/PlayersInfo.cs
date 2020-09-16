using UnityEngine;
using Mirror;
using System.Collections;
using static Dictionary;


public class PlayersInfo : NetworkBehaviour
{
    public GameObject Bar;
    public Color TeamColor;

    public bool Ready = false;
    public bool Init = false;
    public bool Lock = false;

    [SyncVar] public int PIdentity = 0;
    [SyncVar] public string Name;
    [SyncVar] public bool Team;

    public Sprite PlayerAvatar;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        OnRefreshBarsCount();
        StartCoroutine(ClientUpdates());
        if (isLocalPlayer)
            gameObject.name = "LocalPlayer";
    }

    private void OnDestroy() //Usuwanie gracza, wywoływane przy rozłączeniu klienta
    {
        OnRefreshBarsCount();
        StartCoroutine(ClientUpdates());
    }

    public void DestroyBar()
    {
        Destroy(Bar.gameObject);
    }

    private void Update()
    {
        if (isLocalPlayer)
        {
            Name = Dictionary.Name;
            if (PlayerAvatar.name == "none")
                PlayerAvatar = Dictionary.Avatar;

            if (ByteAvatar == null && PlayerAvatar == Dictionary.Avatar && !Lock)
                ByteAvatar = Dictionary.Avatar.texture.EncodeToJPG();
                Lock = true;
            /*
            if (NetR.LocalPlayer == null)
                NetR.LocalPlayer = gameObject;
            */
            if (!Init)
                CmdAvatarSync(ByteAvatar, gameObject, Name);
                Init = true;
        }
        else if (!isLocalPlayer)
        {
            /*
            if (!Ready)
                NetR.CreatePlayer(gameObject);
                Ready = true;
            */
            if (Bar == null)
                Ready = false;

            if (!Team)
                TeamColor = TeamOneColor;
            else
                TeamColor = TeamTwoColor;
        }
    }

    public IEnumerator ClientUpdates()
    {
        yield return new WaitForSeconds(3f);
        CmdAvatarSync(ByteAvatar, gameObject, Dictionary.Name);
    }

    [Command]
    public void CmdTeamSynchro(bool SendTeam, GameObject SendPlayer)
    {
        if (!isLocalPlayer)
            Team = SendTeam;
        Data.RpcTeamSynchro(SendTeam, SendPlayer);
    }

    [Command]
    public void CmdAvatarSync(byte[] GetAvatar, GameObject GetOwner, string GetName)
    {
        Name = GetName;
        Data.RpcResendAvatar(GetAvatar, GetOwner);
    }
}