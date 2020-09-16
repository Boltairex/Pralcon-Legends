using UnityEngine;
using TMPro;
using UnityEngine.UI;
using static Dictionary;

public class LobbyController : MonoBehaviour
{
    [Header("Drużyny")]
    public GameObject FirstTeam;
    public GameObject SecondTeam;
    public GameObject LeftBar;
    public GameObject RightBar;
    public int LRange = 0;
    public int RRange = 0;
    [Header("Czat")]
    public TMP_InputField ChatInput;
    public GameObject ChatList;
    [Header("Wybór Postaci")]
    public RawImage CenterCharacter;
    [Header("Inne")]
    //public NetworkController NetC;
    public DataManagement Data;
    //public NetworkContainer NetR;

    void Start()
    {
        //NetC = GameObject.Find("LobbyManager").GetComponent<NetworkController>();
        Data = GameObject.Find("DataManager").GetComponent<DataManagement>();
        //NetR = GameObject.Find("NetworkContainer").GetComponent<NetworkContainer>();

        if (Players.Length >= 1 && Players.Length <= 10)
        {
            LobbyRestart();
        }
    }
    // Update is called once per frame
    void Update()
    {

    }

    public void LobbyRestart()
    {
        for (int i = 0; i < Players.Length; i++)
        {
            print(i + "Obecna, i " + Players.Length + "Maksymalna");
            if (!Players[i].GetComponent<PlayersInfo>().Team) //lewa strona
            {
                GameObject LBar = Instantiate(LeftBar, FirstTeam.transform);
                Players[i].GetComponent<PlayersInfo>().Bar = LBar;
                LRange++;
            }
            else if (Players[i].GetComponent<PlayersInfo>().Team) //prawa strona
            {
                GameObject RBar = Instantiate(RightBar, SecondTeam.transform);
                Players[i].GetComponent<PlayersInfo>().Bar = RBar;
                RRange++;
            }
        }
    }
}
