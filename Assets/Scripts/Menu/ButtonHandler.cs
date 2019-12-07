using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonHandler : MonoBehaviour
{
    public GameObject main, options;
    bool isMainMenu = true;

    //Tutaj znajduja sie funkcje uzywane przez przyciski
    public void PlayButton()
    {
        SceneManager.LoadScene("Lobby", LoadSceneMode.Single);
    }

    public void OptionsButton()
    {
        if (isMainMenu == true)
        {
            main.SetActive(false);
            options.SetActive(true);
            isMainMenu = false;
        }  
        else
        {
            main.SetActive(true);
            options.SetActive(false);
            isMainMenu = true;
        }
    }

    public void Quit()
    {
        Application.Quit();
    }
}
