using UnityEngine;

public class ObjectInCamera : MonoBehaviour {
    public Transform objectToEnclose;
    public float zoom = 0.6f;

    // This script is based on the formula from this website: http://stackoverflow.com/questions/21544336/how-to-position-the-camera-so-that-my-main-object-is-entirely-visible-and-fit-to
    void Update () {
        var renderer = objectToEnclose.GetComponent<Renderer>();
        Debug.Assert(renderer != null, "Error, ObjectInCamera.cs - Could not find objects renderer");
        var radius = Mathf.Max(renderer.bounds.extents.y, renderer.bounds.extents.x) * zoom;
        var fieldOfView = Camera.main.fieldOfView;
        var z = radius / Mathf.Sin(fieldOfView);
        transform.position = new Vector3(0,0,z);
    }
}
