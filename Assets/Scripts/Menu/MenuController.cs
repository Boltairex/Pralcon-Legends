using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuController : MonoBehaviour
{
    public GameObject Avatar;
    public GameObject Name;
    PRDiscordRPC Discord;

    void Start()
    {
        Discord = gameObject.GetComponent<PRDiscordRPC>();
    }

    void Update()
    {
        Name.GetComponent<TextMeshProUGUI>().text = Discord.Nickname;
        Avatar.GetComponent<Image>().sprite = Discord.Avatar;
    }
}
