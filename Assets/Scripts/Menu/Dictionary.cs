using UnityEngine;

public class Dictionary : MonoBehaviour
{
    //static public NetworkController NetC;
    //static public NetworkContainer NetR;
    static public DataManagement Data;
    //static public PRDiscordRPC Discord;
    static public MenuController MenuC;

    static public GameObject[] Players;

    static public string Name;
    static public Sprite Avatar;
    static public byte[] ByteAvatar;
    static public int MaxPlayers = 0;

    static public Color TeamOneColor;
    static public Color TeamTwoColor;

    static public bool Hosting = false;
    static public bool Connect = false;
    static public bool Online = false;

    static public string RoomName = "Default";
    static public string TeamName = "First";

    static public string FirstTeamName = "First";
    static public string SecondTeamName = "Second";

    static public bool Team = false;
    static public bool InLobby = false;

    private void Start()
    {
        TeamOneColor = new Color(1, 0, 0, 1);
        TeamTwoColor = new Color(0, 0, 1, 1);
        SetComponent();
    }

    private void SetComponent()
    {
        //NetC = GameObject.Find("NetworkController").GetComponent<NetworkController>();
        //NetR = GameObject.Find("NetworkContainer").GetComponent<NetworkContainer>();
        Data = GameObject.Find("DataManagement").GetComponent<DataManagement>();
        //Discord = GameObject.Find("NetworkController").GetComponent<PRDiscordRPC>();
        MenuC = GameObject.Find("MenuController").GetComponent<MenuController>();
    }

    public static void OnRefreshBarsCount()
    {
        Players = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < Players.Length; i++)
        {
            Players[i].GetComponent<PlayersInfo>().DestroyBar();
            Players[i].GetComponent<PlayersInfo>().Ready = false;
        }
        //NetR.Range = 0;
    }
}
