using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoAtack : MonoBehaviour
{

    ObjectsInRange inRange;

    public GameObject target;

    public float fireRate, bulletSpeed;

    float cooldown;

    List<Bullet> bullets = new List<Bullet>();

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
                }
            }


            //Jesli jest po za zasięgiem to ma zacząć do niego iść.
        }

        UpdateShot();
    }

    public void UpdateShot()
    {
        if(target != null)
        {
            if(Vector3.Distance(target.transform.position, transform.position) * 100 <= inRange.autoAttackRange)
            {
                if(cooldown >= 1/fireRate)
                {
                    GameObject g = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    g.transform.localScale = Vector3.one * 0.1f;
                    bullets.Add(new Bullet(g, target));
                    cooldown = 0;
                }
                else
                {
                    cooldown += Time.fixedDeltaTime;
                }
            }
        }

        ProceedBullets();
    }

    void ProceedBullets()
    {
        for (int i = 0; i < bullets.Count; i++)
        {
            bullets[i].obj.transform.Translate(bullets[i].target.transform.position * bulletSpeed * Time.fixedDeltaTime);

            if(Vector3.Distance(bullets[i].obj.transform.position, bullets[i].target.transform.position) < 0.1f)
            {
                Destroy(bullets[i].obj);
                bullets.RemoveAt(i);

                //Zadawanie obrażeń
            }
        }
    }
}

public struct Bullet
{
    public GameObject obj;
    public GameObject target;

    public Bullet(GameObject _obj, GameObject _target)
    {
        obj = _obj;
        target = _target;
    }
}
