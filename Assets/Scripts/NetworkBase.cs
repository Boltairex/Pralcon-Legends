using System;
using static Dictionary;

public class NetworkBase : Mirror.NetworkBehaviour
{
    public Server ConnectedServer;
    public string Password;

    void Start()
    {
        Dictionary.NB = this;
        ConnectedServer = this.GetComponent<Server>();
    }

    public void UpdatePlayersCount() => ConnectedServer.PlayersC = Dictionary.VC.P.Count;

    public void HostServer(string SName, int Port, string Password, int MaxP, bool IsUPnP = false)
    {
        if (!base.isServer)
        {
            ConnectedServer.Name = SName;

            if (Password != string.Empty)
                ConnectedServer.NeedPassword = true;

            ConnectedServer.MaxP = MaxP;
            NM.maxConnections = MaxP;

            NM.StartHost();

            if (Port != 0)
                TT.port = Convert.ToUInt16(Port);
            else
                TT.port = 7777;

            ConnectedServer.Port = TT.port;

            ConnectedServer.IP = NM.networkAddress;

            this.Password = Password;
        }
        else
            print("Serwer jest już włączony");
    }

    public void JoinServer(string IP, int Port, string Password, bool NeedPass = false)
    {
        NM.StartClient();

        TT.port = Convert.ToUInt16(Port);
        NM.networkAddress = IP;
    }

    public override void OnStopClient()
    {
        base.connectionToServer.Disconnect();
        base.connectionToServer.Dispose();
    }

    void OnConnectedToServer()
    {
        UpdatePlayersCount();
    }

    public bool AskForPassword(string Pass)
    {
        if (Pass == Password)
            return true;
        else
            return false;
    }

    private void OnApplicationQuit()
    {
        if (base.isServer)
            NM.StopServer();

        if (base.isClient)
            NM.StopClient();
    }
}
