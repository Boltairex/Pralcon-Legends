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

    [ClientRpc(channel = 1)]
    public void RpcTeamSynchronise(string Team1, string Team2, Color CTeam1, Color CTeam2)
    {
        if (NetC.Connect && NetC.Hosting == false)
        {
            MenuC.FirstColour = CTeam1;
            MenuC.SecondColour = CTeam2;
            MenuC.Team1.text = Team1;
            MenuC.Team2.text = Team2;
            MenuC.TeamSynchro();
        }
    }
}
