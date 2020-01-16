using Mirror;
using UnityEngine;
using static Dictionary;

public class NetworkController : NetworkManager
{
    void Update()
    {
        if (Hosting || Connect)
        {
            InLobby = true;
            Online = true;
        }
        else
        {
            InLobby = false;
            Online = false;
        }
    }

    public void ServerStart()
    {
        if (!Hosting && !Connect && Name != "Unconnected")
        {
            if (MenuC.Port.text == "" || MenuC.Port.text == null)
                GetComponent<TelepathyTransport>().port = 7777;
            else
                GetComponent<TelepathyTransport>().port = ushort.Parse(MenuC.Port.text);

            networkAddress = "localhost";

            Connect = false;
            Hosting = true;
            StartHost();
            MenuC.ListClose();

            DontDestroyOnLoad(Data.gameObject);
            DontDestroyOnLoad(NetR.gameObject);
        }
    }

    public void ServerConnection()
    {
        if (!Hosting && !Connect && Name != "Unconnected")
        {
            if (MenuC.Port.text == "" || MenuC.Port.text == null)
                GetComponent<TelepathyTransport>().port = 7777;
            else
                GetComponent<TelepathyTransport>().port = ushort.Parse(MenuC.Port.text);

            networkAddress = MenuC.IP.text;
            if (MenuC.IP.text == "")
                networkAddress = "localhost";

            Connect = true;
            Hosting = false;
            StartClient();
            MenuC.ListClose();

            DontDestroyOnLoad(Data.gameObject);
            DontDestroyOnLoad(NetR.gameObject);
        }
    }

    public void CloseConnection()
    {
        if (Hosting || Connect)
        {
            if (!Hosting)
            {
                Connect = false;
                Online = false;
                StopClient();
            }
            else
            {
                Hosting = false;
                Online = false;
                StopHost();
            }
        }
    }

    public void LobbyRun()
    {
        Data.RpcIdentity();
        ServerChangeScene("Lobby");
    }

    public void GameRun()
    {
        ServerChangeScene("Game");
    }
}
