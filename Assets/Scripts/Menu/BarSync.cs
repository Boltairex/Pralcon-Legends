using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class BarSync : MonoBehaviour
{
    public GameObject Owner;
    public NetworkController NetC;
    private void Start()
    {
        Owner.GetComponent<PlayersInfo>().Bar = gameObject;
    }
}
