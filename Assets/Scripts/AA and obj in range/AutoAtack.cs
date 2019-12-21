using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoAtack : MonoBehaviour
{

    ObjectsInRange inRange;

    public GameObject target;

    void Start()
    {
        inRange = GetComponent<ObjectsInRange>();
    }


    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            target = null;

            Debug.Log("Clicked");

            if (Physics.Raycast(ray, out hit))
            {

                Debug.Log("Hitted");
                if (hit.transform.gameObject != gameObject)
                {
                    target = hit.transform.gameObject;
                    Debug.Log(target);
                }
            }


            //jesli target to enemy to ma wykonać AA, ale jesli jest po za zasięgiem to ma zacząć do niego iść. Należy pamiętać o tym że kliknięcie na mape w ceul poruszania się musi ustawiać target na null.
        }

    }
}
