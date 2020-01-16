using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BarSync : MonoBehaviour
{
    public GameObject Owner;

    public Image Avatar;
    public TextMeshProUGUI Name;
    public RawImage Color;

    private void Start()
    {
        Owner.GetComponent<PlayersInfo>().Bar = gameObject;
    }

    public void Update()
    {
        if (Owner == null)
        {
            Destroy(gameObject);
            print("Nie wykryto Parenta sterującego");
        }
        else
        {
            Avatar.sprite = Owner.GetComponent<PlayersInfo>().PlayerAvatar;
            Name.text = Owner.GetComponent<PlayersInfo>().Name;
            Color.color = Owner.GetComponent<PlayersInfo>().TeamColor;
        }
    }
}
