using UnityEngine;

public class Cursor : MonoBehaviour
{
    public static Cursor This;

    public static Vector3 WorldPointer;
    public static RaycastHit HitObject;

    public static Material FriendlyOutline;
    public static Material EnemyOutline;
    public static Material NormalOutline;

    public LayerMask layer;
    public LayerMask layer2;

    public RaycastHit Ray;

    public Material friendlyoutline;
    public Material enemyoutline;
    public Material normaloutline;

    private Camera Cam;
    private RaycastHit NullHit;

    private GameObject lastObject;
    private SkinnedMeshRenderer lastObjectMesh;

    void Awake()
    {
        This = this;
        FriendlyOutline = friendlyoutline;
        EnemyOutline = enemyoutline;
        NormalOutline = normaloutline;
        Cam = this.GetComponent<Camera>();
    }

    void Update()
    {
        Ray ray = Cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out Ray, 1000f, layer))
        {
            WorldPointer = Ray.point;
            WorldPointer = new Vector3(WorldPointer.x, 2.5f, WorldPointer.z);
            Debug.DrawLine(Cam.transform.position, WorldPointer);
        }
        if (Physics.Raycast(ray, out Ray, 1000f, layer2))
            HitObject = Ray;
        else
            HitObject = NullHit;
        
    }
}