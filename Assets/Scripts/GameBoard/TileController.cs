using UnityEngine;

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
    private Renderer tileRenderer;

    private void Start () {
        tile = new Tile();
        tileRenderer = GetComponent<Renderer>();
        Debug.Assert(tileRenderer != null, "Error, TileController.cs - Could not find renderer on " + gameObject.name);
    }

    private void Update () {
        Move();
    }

    private void Move() {
        state = Tile.State.Moving;

        var deltaSpeed = speed * Time.deltaTime;

        transform.position += ResolvMovement(deltaSpeed);
    }

    private Vector3 ResolvMovement(float deltaSpeed) {
        var resolvedMovement = -Vector3.up * deltaSpeed;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, -Vector3.up, out hit, deltaSpeed + tileRenderer.bounds.extents.y))
        {
            state = Tile.State.Static;

            var collidedRenderer = hit.collider.GetComponent<Renderer>();
            Debug.Assert(collidedRenderer != null, "Error, TileController.cs - Could not find renderer on " + hit.collider.name);

            var y0 = transform.position.y;
            var y1 = hit.collider.transform.position.y;

            var length = y0 - y1;
            var travelDistance = length - (tileRenderer.bounds.extents.y + collidedRenderer.bounds.extents.y);

            resolvedMovement = -Vector3.up * travelDistance;
        }

        return resolvedMovement;
    }
}
