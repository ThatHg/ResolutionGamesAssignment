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
    private Collider tileCollider;

    private void Start () {
        tile = new Tile();
        tileCollider = GetComponent<Collider>();
        Debug.Assert(tileCollider != null, "Error, TileController.cs - Could not find collider on " + gameObject.name);
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
        if (Physics.Raycast(transform.position, -Vector3.up, out hit, deltaSpeed + tileCollider.bounds.extents.y))
        {
            state = Tile.State.Static;
            
            var y0 = transform.position.y;
            var y1 = hit.collider.transform.position.y;

            var length = y0 - y1;
            var travelDistance = length - (tileCollider.bounds.extents.y + hit.collider.bounds.extents.y);

            resolvedMovement = -Vector3.up * travelDistance;
        }

        return resolvedMovement;
    }

    public void Remove() {
        Destroy(this);
    }
}
