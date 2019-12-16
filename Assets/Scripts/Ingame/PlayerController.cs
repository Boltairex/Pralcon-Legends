using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    Camera cam;

    Ray ray;
    RaycastHit hit;
    OpenCloseButton ocb;

    private void Start()
    {
        cam = GameObject.Find("MainCamera").GetComponent<Camera>();
        ocb = GameObject.Find("ButtonHandler").GetComponent<OpenCloseButton>();
    }
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