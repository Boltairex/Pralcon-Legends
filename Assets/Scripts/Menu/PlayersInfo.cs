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

    [SyncVar]
    public string Name;
    public bool Ready;
    bool Init;

    private void Start()
    {
        GameObject[] Players = GameObject.FindGameObjectsWithTag("PlayerBar");

        Menu = GameObject.Find("MenuController").GetComponent<MenuController>();
        NetC = GameObject.Find("LobbyManager").GetComponent<NetworkController>();
        for (int i = 0; i < Players.Length; i++)
        {
            Destroy(Players[i]);
        }

        if (isLocalPlayer)
        {
            NetC.LocalPlayer = GetComponent<PlayersInfo>();
            Avatar = Menu.DSAvatar;
            Name = Menu.DSName;
            CmdInfoSync(Name, Avatar.texture.EncodeToJPG());
        }
        else
        {
            NetC.LocalPlayer.CmdInfoSync(NetC.LocalPlayer.Name, NetC.LocalPlayer.Avatar.texture.EncodeToJPG());
        }
    }

    private void OnDestroy()
    {
        Destroy(Bar);
    }

    [Command]
    public void CmdInfoSync(string SendName, byte[] SendAvatar)
    {
        Name = SendName;
        RpcInfoSync(SendAvatar);
    }

    [ClientRpc]
    void RpcInfoSync(byte[] GetAvatar)
    {
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
    }
}
