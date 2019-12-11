using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenCloseButton : MonoBehaviour
{
    public GameObject shoppanel;

    public bool isNearShop;

    void Update()
    {
        if (isNearShop == true && Input.GetButtonDown("ShopOpen"))
        {
            shoppanel.SetActive(true);
            GameObject.Find("Character").GetComponent<PlayerController>().enabled = false;
        }
        else if (isNearShop == false && Input.GetButtonDown("ShopOpen"))
        {
            shoppanel.SetActive(false);
            GameObject.Find("Character").GetComponent<PlayerController>().enabled = true;
        }
    }

    public void OpenShop()
    {
        if(isNearShop == true)
        {
            shoppanel.SetActive(true);
            GameObject.Find("Character").GetComponent<PlayerController>().enabled = false;
        }
    }

    public void CloseShop()
    {
        shoppanel.SetActive(false);
        GameObject.Find("Character").GetComponent<PlayerController>().enabled = true;
    }
}
