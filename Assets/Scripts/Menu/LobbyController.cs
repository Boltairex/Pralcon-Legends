using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;

public class LobbyController : MonoBehaviour
{
    [Header("Drużyny")]
    public GameObject FirstTeam;
    public GameObject SecondTeam;
    public GameObject[] Players;
    public GameObject LeftBar;
    public GameObject RightBar;
    [Header("Czat")]
    public TMP_InputField ChatInput;
    public GameObject ChatList;
    [Header("Wybór Postaci")]
    public RawImage CenterCharacter;
    [Header("Inne")]
    public NetworkController NetC;
    public DataManagement Data;
    public NetworkContainer NetR;

    void Start()
    {
        NetC = GameObject.Find("LobbyManager").GetComponent<NetworkController>();
        Data = GameObject.Find("DataManager").GetComponent<DataManagement>();
        NetR = GameObject.Find("NetworkContainer").GetComponent<NetworkContainer>();
        StartCoroutine(GetPlayers());
    }
    // Update is called once per frame
    void Update()
    {
        if (Players.Length >= 1 && Players.Length <= 10)
        {
            for (int i = 0; i < Players.Length; i++)
            {
                print(i + "Obecna, i " + Players.Length + "Maksymalna");
                if (!Players[i].GetComponent<PlayersInfo>().Team) //lewa strona
                {
                    GameObject LBar = Instantiate(LeftBar);
                    LBar.transform.SetParent(FirstTeam.transform);
                    Players[i].GetComponent<PlayersInfo>().Bar = LBar;
                }
                else if (Players[i].GetComponent<PlayersInfo>().Team) //prawa strona
                {
                    GameObject RBar = Instantiate(RightBar);
                    RBar.transform.SetParent(SecondTeam.transform);
                    Players[i].GetComponent<PlayersInfo>().Bar = RBar;
                }
            }
        }
    }
    IEnumerator GetPlayers()
    {
        yield return Players = Data.Players;
    }
}
