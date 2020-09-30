using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using System;
using Mirror;

public class ServersScript : MonoBehaviour
{
    public GameObject ServerPrefab;
    public RectTransform This;

    public int Count;

    public List<Server> ActiveServers = new List<Server>();

    public Server CurrentServer;

    protected ServerTabModes modes;
    public ServerTabModes Modes
    {
        get { return modes; }
        set
        {
            modes = value;
            ChangeMode();
        }
    }

    [Header("Server Head")]
    public GameObject ServerHead;

    public TMP_Text Name;
    public TMP_Text Players;
    public TMP_InputField Password;
    public TMP_Text IP;

    [Header("Direct")]
    public GameObject DirectC;

    public TMP_InputField DPort;
    public TMP_InputField DPassword;
    public TMP_InputField DIP;

    [Header("Host")]
    public GameObject HServerHead;

    public TMP_InputField HName;
    public TMP_InputField HPlayers;
    public TMP_InputField HPort;
    public TMP_InputField HPassword;

    float Y = 470;
    float y = 470;

    void Start()
    {
        ServerHead.SetActive(false);
        DirectC.SetActive(false);
        HServerHead.SetActive(false);

        Dictionary.ServS = this;

        if (This == null)
            This = this.transform.Find("Content").GetComponent<RectTransform>();

        This.transform.localPosition = new Vector2(0, y);

        UpdateList();
    }

    void ChangeMode()
    {
        ServerHead.gameObject.SetActive(false);
        DirectC.SetActive(false);
        HServerHead.SetActive(false);

        if (Modes != ServerTabModes.None)
        {
            Dictionary.MenuC.CurLayer = MenuController.Layers.ServerTab;

            if (Modes == ServerTabModes.ClickOnServer)
            {
                ServerHead.gameObject.SetActive(true);
            }
            else if (Modes == ServerTabModes.DirectConnect)
            {
                DirectC.SetActive(true);
            }
            else if (Modes == ServerTabModes.Host)
            {
                HServerHead.SetActive(true);
            }
        }
        else
            Dictionary.MenuC.CurLayer = MenuController.Layers.Servers;
    }

    void Update()
    {
        if (Dictionary.MenuC.CurLayer != MenuController.Layers.ServerTab)
        {
            Y += Input.GetAxis("Mouse ScrollWheel") * -340;
            Y = Mathf.Clamp(Y, 470, 3000);
        }

        y = Mathf.Lerp(y, Y, Time.deltaTime * 10);

        This.transform.localPosition = new Vector2(0, y);

        if (Input.GetKeyDown(KeyCode.Escape) && Dictionary.MenuC.CurLayer == MenuController.Layers.ServerTab && !Dictionary.ColorActive && !Dictionary.ESCTimer)
        {
            Modes = ServerTabModes.None;
            Dictionary.ESCTimer = true;
            Dictionary.MenuC.CurLayer = MenuController.Layers.Servers;
            CurrentServer = null;
        }
    }

    GameObject CreateServerObject(int I, string name, string IP, bool NeedPassword, int PlayerCount, int MaxP, int Port = 7777)
    {
        GameObject G = Instantiate(ServerPrefab);
        Server F = G.GetComponent<Server>();

        F.SendTo = this;
        F.Index = I;
        F.Name = name;
        F.IP = IP;
        F.NeedPassword = NeedPassword;
        F.PlayersC = PlayerCount;
        F.MaxP = MaxP;
        F.Port = Port;

        ActiveServers.Add(F);
        G.GetComponent<ServerPrefCont>().UpdateInfo(name, PlayerCount, MaxP, NeedPassword);
        return G;
    }

    void ResetServers()
    {
        foreach (Server s in ActiveServers)
        {
            Destroy(s.gameObject);
        }
        ActiveServers.Clear();
    }

    public void ServerFound()
    {
        print("Janek kurwa serwer mamy");
    }

    public void UpdateList()
    {
        ResetServers();

        for (int X = 0; X < 3; X++)
        {
            GameObject S = CreateServerObject(X, "Serwer Hydra Gaming", "1.0.0.0", true, 0, 10);
            S.transform.SetParent(This.transform);

            S.transform.localScale = new Vector3(2, 2, 1);
            S.transform.localPosition = new Vector2(0, (X * -70) + (-10 * X));
        }
        
    }

    public void ClickedOnServer(Server S)
    {
        Modes = ServerTabModes.ClickOnServer;

        CurrentServer = S;

        ServerHead.gameObject.SetActive(true);

        print(S.Name);
        Name.text = S.Name;
        Players.text = $"{S.PlayersC}/{S.MaxP}";
        IP.text = $"{S.IP}:{S.Port}";

        if (S.NeedPassword)
        {
            Password.gameObject.SetActive(true);
        }
        else
        {
            Password.gameObject.SetActive(false);
        }
    }

    public void TabDirectConnection()
    {
        Modes = ServerTabModes.DirectConnect;
    }

    public void TabHost()
    {
        Modes = ServerTabModes.Host;
    }

    public void Host()
    {
        ushort port = 0;
        ushort players = ushort.Parse(HPlayers.text);

        if (!String.IsNullOrWhiteSpace(HPort.text))
            port = ushort.Parse(HPort.text);

        Dictionary.NB.HostServer(HName.text, port, HPassword.text, players);
    }

    public void Join()
    {
        ushort _port = 0;

        if (!String.IsNullOrWhiteSpace(DPort.text))
            _port = ushort.Parse(DPort.text);
                
        Dictionary.NB.JoinServer(DIP.text, _port, DPassword.text);
    }

    public void Join(bool b)
    {
        Dictionary.NB.JoinServer(CurrentServer.IP, Convert.ToUInt16(CurrentServer.Port), Password.text, false);
    }

    public void CheckPlayersCount()
    {
        if (HPlayers.text != "")
        {
            int x = int.Parse(HPlayers.text);
            x = Mathf.Clamp(x, 2, 10);
            HPlayers.text = x.ToString();
        }
    }
}

public enum ServerTabModes
{
    ClickOnServer,
    DirectConnect,
    Host,
    None
}
