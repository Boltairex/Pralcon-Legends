using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsInRange : MonoBehaviour
{
    [Header("Objects Storage")]
    public Transform minions;
    public Transform players, creatures, bosses;

    [Header("Objects Lists")]
    public List<GameObject> minionsList = new List<GameObject>();
    public List<GameObject> playerList = new List<GameObject>();
    public List<GameObject> creaturesList = new List<GameObject>();
    public List<GameObject> bossesList = new List<GameObject>();

    [Header("Config")]
    public int detectRange;
    public int autoAttackRange;

    void Update()
    {
        UpdateLists();
    }

    public void UpdateLists()
    {
        minionsList.Clear();
        playerList.Clear();
        creaturesList.Clear();
        bossesList.Clear();

        foreach (Transform m in minions)
        {
            if(Vector3.Distance(m.position, transform.position) * 100 <= detectRange) minionsList.Add(m.gameObject);
        }

        foreach (Transform p in players)
        {
            if (Vector3.Distance(p.position, transform.position) * 100 <= detectRange) playerList.Add(p.gameObject);
        }

        foreach (Transform c in creatures)
        {
            if (Vector3.Distance(c.position, transform.position) * 100 <= detectRange) creaturesList.Add(c.gameObject);
        }

        foreach (Transform b in bosses)
        {
            if (Vector3.Distance(b.position, transform.position) * 100 <= detectRange) bossesList.Add(b.gameObject);
        }
    }
}
