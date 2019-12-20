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
    Sprite Avatar;

    [SyncVar]
    string Name;
    bool Ready;
    bool Init;

    private void Start()
    {
        Menu = GameObject.Find("MenuController").GetComponent<MenuController>();
        NetC = GameObject.Find("LobbyManager").GetComponent<NetworkController>();
        if (isLocalPlayer)
        {
            Avatar = Menu.DSAvatar;
            Name = Menu.DSName;
            CmdInfoSync(Name, Avatar.texture.EncodeToJPG());
        }
    }

    private void OnDestroy()
    {
        Destroy(Bar);
    }

    [Command]
    void CmdInfoSync(string SendName, byte[] SendAvatar)
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
