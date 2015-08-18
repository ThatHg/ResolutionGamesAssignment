using UnityEngine;

public class ObjectInCamera : MonoBehaviour {
    public Transform objectToEnclose;
    void Update () {
        var renderer = objectToEnclose.GetComponent<Renderer>();
        Debug.Assert(renderer != null, "Error, ObjectInCamera.cs - Could not find objects renderer");

        var radius = Mathf.Max(renderer.bounds.extents.y, renderer.bounds.extents.x) * 0.6f;
        var fieldOfView = Camera.main.fieldOfView;
        var z = radius / Mathf.Sin(fieldOfView);
        transform.position = new Vector3(0,0,z);
    }
}
