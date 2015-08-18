using UnityEngine;
using System;

public class TileController : MonoBehaviour {
    public Tile.State state {
        get { return tile.currentState; }
        set { tile.currentState = value; }
    }
    public Tile.Type type {
        get { return tile.type; }
        set { tile.type = value; }
    }

    public float speed;

    private Tile tile;
    private Renderer renderer;

    private void Start () {
        tile = new Tile();
        renderer = GetComponent<Renderer>();
        Debug.Assert(renderer != null, "Error, TileController.cs - Could not find renderer on " + gameObject.name);
    }

    private void Update () {
        Move();
    }

    private void Move() {
        state = Tile.State.Moving;

        var deltaSpeed = speed * Time.deltaTime;

        var movement = -Vector3.up * deltaSpeed;

        var layer = gameObject.layer;
        gameObject.layer = 2; // Set this gameObjects layer to IgnoreRaycast.

        RaycastHit hit;
        if (Physics.Raycast(transform.position, -Vector3.up, out hit, deltaSpeed + renderer.bounds.extents.y)) {
            Debug.DrawRay(transform.position, movement);
            state = Tile.State.Static;

            var renderer1 = hit.collider.GetComponent<Renderer>();
            Debug.Assert(renderer1 != null, "Error, TileController.cs - Could not find renderer on " + hit.collider.name);

            var y0 = transform.position.y;
            var y1 = hit.collider.transform.position.y;

            var length = y0 - y1;
            var travelDistance = length - (renderer.bounds.extents.y + renderer1.bounds.extents.y);

            movement.y -= travelDistance;
        }

        transform.position += movement;

        gameObject.layer = layer; // Set this gameObjects layer to its default layer.
    }
}
