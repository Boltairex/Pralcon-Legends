using UnityEngine;

public class Cursor : MonoBehaviour
{
    public RaycastHit Ray;
    public static Vector3 WorldPointer;
    //
    public LayerMask layer;

    Camera Cam;

    void Start()
    {
        Cam = this.GetComponent<Camera>();
    }

    void Update()
    {
        Ray ray = Cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out Ray, 1000f,layer))
        {
            WorldPointer = Ray.point;
            WorldPointer = new Vector3(WorldPointer.x, 1.5f, WorldPointer.z);
            Debug.DrawLine(Cam.transform.position, WorldPointer);
        }
    }
}
