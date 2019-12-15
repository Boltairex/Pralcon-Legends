using UnityEngine;
using TMPro;

public class Creeps : MonoBehaviour
{
    PlayerStats stats;

    void Start() => stats = GameObject.Find("PlayerStuff").GetComponent<PlayerStats>();
    void Update() => gameObject.GetComponent<TextMeshProUGUI>().text = stats.M.ToString();
}
