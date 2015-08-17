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

    private Tile tile;

    private void Start () {
        
    }

    private void Update () {
        
    }

    private void OnCollisionStay() {
        state = Tile.State.Static;
    }
}
