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
    public int LRange = 1;
    public int RRange = 1;
    public GameObject LeftBar;
    public GameObject RightBar;
    [Header("Czat")]
    public TMP_InputField ChatInput;
    public GameObject ChatList;
    [Header("Wybór Postaci")]
    public RawImage CenterCharacter;

    void Start()
    {
        Players = GameObject.FindGameObjectsWithTag("PlayerBar");
    }
    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < Players.Length; i++) //Odświeżanie listy graczy
        {
            PlayersInfo PLY = (Players[i].GetComponent<PlayersInfo>());
            if (Players[i].GetComponent<PlayersInfo>().Team == false)
            {
                CreateLeftBar(PLY.Name, PLY.Avatar, PLY.gameObject);
            }
            else
            {
                CreateRightBar(PLY.Name, PLY.Avatar, PLY.gameObject);
            }
        }
    }

    public void CreateLeftBar(string name, Sprite PImage, GameObject owner)
    {
        Player = Instantiate(LeftBar);
        Player.transform.SetParent(LeftBar.transform);
        Player.GetComponent<RectTransform>().anchoredPosition = new Vector3(-210f, 90f + -140 * LRange, 0);
        LRange++;
        Player.transform.localScale = new Vector3(1, 1, 1);
        Player.GetComponent<BarSync>().Owner = owner;
        Player.GetComponentInChildren<Image>().sprite = PImage;
        Player.GetComponentInChildren<TextMeshProUGUI>().text = name;
        owner.GetComponent<PlayersInfo>().Bar = Player;
        Player.GetComponent<BarSync>().NetC = gameObject.GetComponent<NetworkController>();
    }

    public void CreateRightBar(string name, Sprite PImage, GameObject owner)
    {
        Player = Instantiate(RightBar);
        Player.transform.SetParent(RightBar.transform);
        Player.GetComponent<RectTransform>().anchoredPosition = new Vector3(-210f, 90f + -140 * RRange, 0);
        RRange++;
        Player.transform.localScale = new Vector3(1, 1, 1);
        Player.GetComponent<BarSync>().Owner = owner;
        Player.GetComponentInChildren<Image>().sprite = PImage;
        Player.GetComponentInChildren<TextMeshProUGUI>().text = name;
        owner.GetComponent<PlayersInfo>().Bar = Player;
        Player.GetComponent<BarSync>().NetC = gameObject.GetComponent<NetworkController>();
    }
}
