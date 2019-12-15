using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    GameClock GC;
    void Start() => GC = GameObject.Find("GameClock").GetComponent<GameClock>();

    void Update()
    {
        string a;
        string b;
        if (GC.GSTime > 9)
        { a = null; }
        else
        { a = "0"; }

        if (GC.GMTime > 9)
        { b = null; }
        else
        { b = "0"; }

        if (GC.GHTime == 0)
        { gameObject.GetComponent<TextMeshProUGUI>().text = b + GC.GMTime + ":" + a + GC.GSTime; }
        else
        { gameObject.GetComponent<TextMeshProUGUI>().text = GC.GHTime + ":" + b + GC.GMTime + ":" + a + GC.GSTime; }

    }
}