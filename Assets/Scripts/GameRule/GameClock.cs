using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameClock : MonoBehaviour
{
    public float GTime;
    public int GSTime;
    public int GMTime;
    public int GHTime;
    public int GTSpeed;
    public bool Generate;
    bool MTime;
    bool HTime;
    

    void Start()
    {
        GTSpeed = 1; // Prędkość działania gry
        GTime = 0f; // Czas realny od rozpoczęcia gry
        GSTime = 0; // Czas w sekundach
        GMTime = 0; // Czas w minutach
        GHTime = 0; // Czas w godzinach
    }

    void FixedUpdate()
    {
        if (GMTime >= 31 && !HTime)
        {
            HTime = true;
        }
        if (GMTime >= 60)
        {
            GMTime = 0;
        }
        if (GSTime >= 31 && !MTime)
        {
            MTime = true;
        }
        GTime += Time.deltaTime * GTSpeed;
        GSTime = (int)GTime % 60;
        if (GMTime <= 30 && HTime)
        {
            HTime = false;
            GHTime += 1;
        }
        if (GSTime <= 30 && MTime)
        {
            MTime = false;
            GMTime += 1;
            Generate = true;
        }
    }
}
