using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuGUI : MonoBehaviour
{
    public GameObject Buttons;
    public GameObject List;
    public GameObject IA;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            IA.SetActive(false);
        }
    }

    public void ListOpen()
    {
        Buttons.SetActive(false);
        List.SetActive(true);
        IA.SetActive(false);
    }

    public void ListClose()
    {
        Buttons.SetActive(true);
        List.SetActive(false);
    }

    public void InputArea()
    {
        IA.SetActive(true);
    }
}
