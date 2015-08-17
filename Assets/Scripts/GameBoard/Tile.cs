using UnityEngine;

public class Tile : MonoBehaviour {
    public enum State {
        InvalidState = 0,
        Static,
        Moving,
    };

    public enum Type {
        InvalidType = 0,
        Tile00,
        Tile01,
        Tile02,
        Tile03,
    };

    public Type type;

    public State currentState { get; private set; }

    private void Start() {
    
    }

    private void Update() {
    
    }
}
