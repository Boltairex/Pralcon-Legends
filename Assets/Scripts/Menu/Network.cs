using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Network : MonoBehaviour
{
    public GameObject EIP;
    public GameObject EPort;
    Lidgren.Network.NetPeerConfiguration config;

    public void NetChange()
    {
        config = new Lidgren.Network.NetPeerConfiguration("RunUp");
        config.Port = int.Parse(EPort.GetComponent<TMP_InputField>().text);
        if (config.Port <= 0 || config.Port >= 51000)
        {
            EPort.GetComponent<TMP_InputField>().text = "7777";
        }
    }

    public void ServerStart()
    {
        config.EnableUPnP = true;
        Lidgren.Network.NetServer server = new Lidgren.Network.NetServer(config);
        server.Start();
    }

    public void LobbyRun()
    {
        SceneManager.LoadScene(2);
    }
}
