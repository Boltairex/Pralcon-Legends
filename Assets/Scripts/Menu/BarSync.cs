using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class BarSync : MonoBehaviour
{
    public GameObject Owner;
    public NetworkController NetC;

    void Update()
    {
        gameObject.GetComponentInChildren<RawImage>();
    }
}
