using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BarSync : MonoBehaviour
{
    public PlayersInfo Owner;

    private void Start()
    {
        Owner.Bar = gameObject;
    }

    public void Update()
    {
        gameObject.GetComponentInChildren<Image>().sprite = Owner.Avatar;
        gameObject.GetComponentInChildren<TextMeshProUGUI>().text = Owner.name;
    }
}
