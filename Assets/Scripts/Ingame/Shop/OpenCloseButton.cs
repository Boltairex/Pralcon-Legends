using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenCloseButton : MonoBehaviour
{
    public GameObject shoppanel;

    bool isNearShop;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "ShopCollider")
            isNearShop = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "ShopCollider")
            isNearShop = false;        
    }

    public void OpenShop()
    {
        if(isNearShop == true)
        {
            shoppanel.SetActive(true);
            GameObject.Find("MainCamera").GetComponent<CameraMovement>().enabled = false;
            GameObject.Find("Character").GetComponent<PlayerController>().enabled = false;
        }
    }

    public void CloseShop()
    {
        shoppanel.SetActive(false);
        GameObject.Find("MainCamera").GetComponent<CameraMovement>().enabled = true;
        GameObject.Find("Character").GetComponent<PlayerController>().enabled = true;
    }
}
