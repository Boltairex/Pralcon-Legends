using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;

public class NetworkController : NetworkBehaviour
{
    public MenuController MenuC;
    public GameObject BarPref;
    public int Range = 0;

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
}
