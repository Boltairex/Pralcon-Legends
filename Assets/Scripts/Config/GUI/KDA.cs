using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KDA : MonoBehaviour
{
    PlayerStats stats;

    void Start()
    {
        stats = GameObject.Find("PlayerStuff").GetComponent<PlayerStats>();
    }

    void Update()
    {
        gameObject.GetComponent<TextMeshProUGUI>().text = stats.K + "/" + stats.D + "/" + stats.A;
    }


}
