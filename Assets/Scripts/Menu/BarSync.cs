using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BarSync : MonoBehaviour
{
    public NetworkContainer Owner;

    public Image Avatar;
    public TextMeshProUGUI Name;
    public RawImage Color;

    private void Start()
    {
        Owner.LocalPlayerBar = gameObject.GetComponent<BarSync>();
    }

    public void Update()
    {
        Avatar.sprite = Owner.Avatar;
        Name.text = Owner.Name;
        if (!Owner.Team)
        { Color.color = Owner.FirstTeam; }
        else
        { Color.color = Owner.SecondTeam; }
    }
}
