using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class FloatingDamage : MonoBehaviour
{
    public GameObject prefab;

    public static GameObject Prefab;
    public static List<GameObject> InfoText = new List<GameObject>();

    void Awake() => Prefab = prefab;

    public static void CreateFloatingText(Vector3 Pos, string Text, Color C)
    {
        GameObject Obj = Instantiate(Prefab);
        Pos.y = 6;
        Obj.transform.position = Pos;

        Obj.GetComponent<TMP_Text>().text = Text;
        Obj.GetComponent<TMP_Text>().color = C;

        InfoText.Add(Obj);
    }

    void Update()
    {
        try
        {
            foreach (GameObject O in InfoText)
            {
                if (O != null)
                {
                    O.transform.position += new Vector3(0, 0.1f, 0);
                    if (O.transform.position.y > 8)
                    {
                        InfoText.Remove(O);
                        Destroy(O);
                    }
                }
            }
        }
        catch { };
    }
}
