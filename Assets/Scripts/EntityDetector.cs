using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class EntityDetector : MonoBehaviour
{
    MeshRenderer Mesh;
    private void Start() => Mesh = this.GetComponent<MeshRenderer>();

    private void Update()
    {
        if (Cursor.HitObject.transform?.gameObject == this.gameObject)
            Mesh.sharedMaterials[1] = Cursor.FriendlyOutline;
        else if (Cursor.HitObject.transform?.gameObject != this.gameObject)
            Mesh.sharedMaterials[1] = null;
    }
}
