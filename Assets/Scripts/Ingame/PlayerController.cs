using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    Camera cam;

    Ray ray;
    RaycastHit hit;

    private void Start()
    {
        cam = GameObject.Find("MainCamera").GetComponent<Camera>();
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

        gameObject.GetComponent<NavMeshAgent>().speed = GameObject.Find("PlayerStuff").GetComponent<PlayerStats>().MV;
    }
}