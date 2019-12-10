using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    public Camera cam;

    Ray ray;
    RaycastHit hit;
    OpenCloseButton ocb;

    private void Start()
    {
        ocb = GameObject.Find("ButtonHandler").GetComponent<OpenCloseButton>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                gameObject.GetComponent<NavMeshAgent>().SetDestination(hit.point);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "ShopCollider")
            ocb.isNearShop = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "ShopCollider")
            ocb.isNearShop = false;
    }
}