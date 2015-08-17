using UnityEngine;

public class Tile : MonoBehaviour {
    public enum State {
        InvalidState = 0,
        Static,
        Moving,
    };

    public State CurrentState { get; private set; }

    private void Start() {
    
    }

    private void Update() {
    
    }
}
