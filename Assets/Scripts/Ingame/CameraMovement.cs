using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    public float speed;
    public float cameraMargin;
    public static GameObject Character;

    public bool BlockedCamera;
    public Vector3 vec3correct = new Vector3(0,100,-70);
    Vector3 vec3;

    void Awake()
    {
        Application.targetFrameRate = 90;

        if (Character == null)
        {
            var o = GameObject.FindGameObjectsWithTag("Character");
            foreach (GameObject O in o)
            {
                if (O.GetComponent<CharacterController>().IsLocalController)
                    Character = O;
            }
        }
        gameObject.transform.rotation = Quaternion.Euler(55f, 0, 0);
        vec3 = Character.transform.position + vec3correct;
    }

    void Update()
    {
        if (BlockedCamera == false)
        {
            //Obliczenia kamery
            if (Input.mousePosition.x > Screen.width - cameraMargin)
                vec3.x += speed * Time.deltaTime;
            else if (Input.mousePosition.x < cameraMargin)
                vec3.x -= speed * Time.deltaTime;
            if (Input.mousePosition.y < cameraMargin)
                vec3.z -= speed * Time.deltaTime;
            else if (Input.mousePosition.y > Screen.height - cameraMargin)
                vec3.z += speed * Time.deltaTime;

            this.transform.position = vec3;

            if (Input.GetKeyDown(KeyCode.R))
                BlockedCamera = true;
        }
        else if (BlockedCamera == true && Character != null)
        {
            vec3.z = Character.transform.position.z - 10;
            vec3.x = Character.transform.position.x;
            this.transform.position = Character.transform.position + vec3correct;

            if (Input.GetKeyDown(KeyCode.R))
            {
                vec3 = Character.transform.position + vec3correct;
                BlockedCamera = false;
            }
        }
    }
}
