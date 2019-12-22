using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DataManagement : NetworkBehaviour
{
    public MenuController MenuC;
    public NetworkController NetC;

    [ClientRpc(channel = 1)]
    public void RpcGameInfo(string GameName, string GamePass, string PlayerCount, bool Checkbox)
    {
        if (NetC.Connect && NetC.Hosting == false)
        {
            MenuC.GameName.text = GameName;
            MenuC.GamePass.text = GamePass;
            MenuC.PlayerSize.text = PlayerCount;
            // = GameMap
            MenuC.Check = Checkbox;
            MenuC.OnCheckboxClick();
        }
    }
}
