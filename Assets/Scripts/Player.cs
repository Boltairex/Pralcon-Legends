using UnityEngine;
using Mirror;

public class Player : NetworkBehaviour
{
    [SyncVar] public ulong ID;
    [SyncVar] public string Nickname;
    public Sprite Avatar;

    public PlayerBarScript ConnectedBar;

    protected bool team;
    public bool Team
    {
        get { return team; }
        set
        {
            team = value;
            CmdUpdateColors(value);
        }
    }

    void Start()
    {
        if (base.isLocalPlayer)
            LoadVariables();
        else
        {
            if (ConnectedBar == null)
                ConnectedBar = Instantiate(Dictionary.PlayerBarPref).GetComponent<PlayerBarScript>();

            ConnectedBar.SetVariables(Nickname, "", Avatar);
        }

        Dictionary.VC.P.Add(this);
        Dictionary.NB.UpdatePlayersCount();
        Dictionary.NB.OnConnectedToServer();
    }

    void LoadVariables()
    {
        ID = Dictionary.LocalPlayer.ID;
        Nickname = Dictionary.LocalPlayer.Username;
        Avatar = Dictionary.LocalPlayer.Avatar;

        CmdGetAvatar(Dictionary.LocalPlayer.NetworkAvatar,Dictionary.LocalPlayer.ID);
    }

    [Command]
    public void CmdGetAvatar(byte[] avatar, ulong u) => RpcUpdateAvatar(avatar, u);

    [ClientRpc]
    public void RpcUpdateAvatar(byte[] avatar, ulong u)
    {
        if (Dictionary.LocalPlayer.ID != u)
        {
            Avatar = Dictionary.This.BytesToSprite(avatar);
            ConnectedBar.Avatar.sprite = Avatar;
        }
    }

    [Command]
    public void CmdUpdateColors(bool team)
    {
        Team = team;

        if (ConnectedBar != null)
        {
            if (Team)
                ConnectedBar.ColorChange(Dictionary.TeamTwoColor);
            else
                ConnectedBar.ColorChange(Dictionary.TeamOneColor);
        }
    }


    void OnDestroy()
    {
        Dictionary.VC.P.Remove(this);
        Destroy(ConnectedBar);
        Dictionary.NB.UpdatePlayersCount();
    }
}
