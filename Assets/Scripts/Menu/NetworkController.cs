using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;

public class NetworkController : NetworkManager
{
    public MenuController MenuC;
    public GameObject BarPref;

    #region List
    [HideInInspector] public PlayersInfo LocalPlayer;
    [HideInInspector] public int Range = 0;
    [HideInInspector] public bool Hosting = false;
    [HideInInspector] public bool Connect = false;
    #endregion List

    void Start()
    {

    }

    public void CreateBar(string name, Sprite PImage, GameObject owner)
    {
        GameObject PlayerJoin = Instantiate(BarPref);
        PlayerJoin.transform.SetParent(MenuC.Content.transform);
        PlayerJoin.GetComponent<RectTransform>().anchoredPosition = new Vector3(-210f, 90f + -140 * Range, 0);
        Range++;
        PlayerJoin.transform.localScale = new Vector3(1, 1, 1);
        PlayerJoin.GetComponent<BarSync>().Owner = owner;
        PlayerJoin.GetComponentInChildren<Image>().sprite = PImage;
        PlayerJoin.GetComponentInChildren<TextMeshProUGUI>().text = name;
        owner.GetComponent<PlayersInfo>().Bar = PlayerJoin;
    }

    public void ServerStart()
    {
        if (!Hosting && !Connect)
        {
            if (MenuC.Port.text == "" || MenuC.Port.text == null)
            { networkPort = 7777; }
            else
            { networkPort = int.Parse(MenuC.Port.text); }

            networkAddress = "localhost";

            Connect = false;
            Hosting = true;
            StartHost();
        }
    }

    public void ServerConnection()
    {
        if (!Hosting && !Connect)
        {
            if (MenuC.Port.text == "" || MenuC.Port.text == null)
            { networkPort = 7777; }
            else
            { networkPort = int.Parse(MenuC.Port.text); }

            networkAddress = MenuC.IP.text;
            if (MenuC.IP.text == "") { networkAddress = "localhost"; }

            Connect = true;
            Hosting = false;
            StartClient();
        }
    }
}
