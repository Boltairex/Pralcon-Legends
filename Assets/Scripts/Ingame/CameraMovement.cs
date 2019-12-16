﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    //Wszystkie potrzebne wartosci, mozesz zrobic tak, aby byly ustawiane w void Start()
    public float speed;
    public float cameraMargin;
    public Camera cam;
    GameObject Character;
    //Wartosci sluzace do dzialan/ustawien NIE ZMIENIAC I NIE BAWIC SIE
    bool BlockedCamera;
    Vector3 vec3;

    void Start()
    {
        Character = GameObject.Find("Character");
        gameObject.transform.rotation = Quaternion.Euler(60f, 0, 0);
    }

    void Update()
    {
        if (BlockedCamera == false)
        {
            vec3 = cam.transform.position;

            //Obliczenia kamery
            if (Input.mousePosition.x > Screen.width - cameraMargin)
                vec3.x += speed * Time.deltaTime;
            if (Input.mousePosition.x < cameraMargin)
                vec3.x -= speed * Time.deltaTime;
            if (Input.mousePosition.y < cameraMargin)
                vec3.z -= speed * Time.deltaTime;
            if (Input.mousePosition.y > Screen.height - cameraMargin)
                vec3.z += speed * Time.deltaTime;

            cam.transform.position = vec3;

            if (Input.GetButtonDown("BlockCamera"))
                BlockedCamera = true;
        }
        else if (BlockedCamera == true && Character != null)

        {
            vec3.z = Character.transform.position.z - 10;
            vec3.x = Character.transform.position.x;
            cam.transform.position = vec3;

            if (Input.GetButtonDown("BlockCamera"))
                BlockedCamera = false;
        }
    }
}
