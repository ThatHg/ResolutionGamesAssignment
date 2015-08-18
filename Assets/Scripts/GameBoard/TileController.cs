using UnityEngine;

public class TileController : MonoBehaviour {
    private static int ID = 0;
    public Tile.State state { get; set; }

    public int Id { get; private set; }
    public float speed;
    private Collider tileCollider;

    private void Awake () {
        tileCollider = GetComponent<Collider>();
        Debug.Assert(tileCollider != null, "Error, TileController.cs - Could not find collider on " + gameObject.name);
        Id = System.Threading.Interlocked.Increment(ref ID);
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
        if (Physics.Raycast(transform.position, -Vector3.up, out hit, deltaSpeed + tileCollider.bounds.extents.y)){
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
