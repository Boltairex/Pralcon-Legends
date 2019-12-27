using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LobbyController : MonoBehaviour
{
    [Header("Drużyny")]
    public GameObject FirstTeam;
    public GameObject SecondTeam;
    public GameObject[] Players;
    public GameObject Player;
    public GameObject LeftBar;
    public GameObject RightBar;
    [Header("Czat")]
    public TMP_InputField ChatInput;
    public GameObject ChatList;
    [Header("Wybór Postaci")]
    public RawImage CenterCharacter;
    [Header("Inne")]
    public NetworkController NetC;

    void Start()
    {
        NetC = GameObject.Find("LobbyManager").GetComponent<NetworkController>();
    }
    // Update is called once per frame
    void Update()
    {

    }
}
