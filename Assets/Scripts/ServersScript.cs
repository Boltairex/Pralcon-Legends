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
    public TMP_InputField HIP;
    public TMP_InputField HPort;
    public TMP_InputField HPassword;
    public Image UPNP;
    bool IsUPNP;

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

    public void ChangeUPnP()
    {
        if (IsUPNP)
        {
            UPNP.sprite = Dictionary.CheckOff;
            IsUPNP = false;
        }
        else
        {
            UPNP.sprite = Dictionary.CheckOn;
            IsUPNP = true;
        }
    }

    void Update()
    {
        if (Dictionary.MenuC.CurLayer != MenuController.Layers.ServerTab)
        {
            Y += Input.GetAxis("Mouse ScrollWheel") * -200;
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
        for (int X = 0; X < Count; X++)
        {
            GameObject S = CreateServerObject(X, "Dupa", "1.0.0.0", true, 0, 10);
            S.transform.SetParent(This.transform);

            S.transform.localScale = new Vector3(2,2,1);
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
        try
        {
            Dictionary.NB.HostServer(HName.text,int.Parse(HPort.text),HPassword.text,int.Parse(HPlayers.text),false);
        }catch(Exception E) {print(E);}
    }

    public void Join()
    {
        Dictionary.NB.JoinServer(DIP.text,int.Parse(DPort.text),DPassword.text);
    }
    
    public void Join(bool b)
    {
        Dictionary.NB.JoinServer(CurrentServer.IP,CurrentServer.Port,Password.text,CurrentServer.NeedPassword);
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
