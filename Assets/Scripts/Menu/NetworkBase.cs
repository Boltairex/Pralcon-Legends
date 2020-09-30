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

        //try { HostServer("",7777,"",2); } catch(Exception e) { print(e); }
    }

    public void UpdatePlayersCount() => ConnectedServer.PlayersC = Dictionary.VC.P.Count;

    public void HostServer(string _sname, ushort _port, string _password, ushort _maxp)
    {
        if (!base.isClient)
        {
            NM.StartHost();

            ConnectedServer.Name = _sname;

            if (string.IsNullOrWhiteSpace(_password))
                ConnectedServer.NeedPassword = true;

            ConnectedServer.MaxP = NM.maxConnections = _maxp;
            
            NM.networkAddress = "localhost";

            ConnectedServer.IP = NM.networkAddress;

            ConnectedServer.Port = TT.port;

            this.Password = _password;    
        }
        else
            print("Serwer jest już włączony");
    }

    public void JoinServer(string _ip, ushort _port, string _password, bool _needPass = false)
    {
        NM.StartClient();

        TT.port = _port;
        NM.networkAddress = _ip;
    }

    public void Disconnect()
    {
        if(base.isServer)
            NM.StopServer();
        if(base.isClient)
            NM.StopClient();

        if(Dictionary.LocalPlayer.ID != 0)
            Dictionary.CT = ConnectionType.Connected;
        else
            Dictionary.CT = ConnectionType.Unconnected;
    }

    public void OnConnectedToServer()
    {
        Dictionary.MenuC.CurLayer = MenuController.Layers.Buttons;
        Dictionary.RMenu.GetComponent<GUIAnim>().Switch("In");
        Dictionary.CT = ConnectionType.Joined;
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
