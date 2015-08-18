public class Tile {
    public enum State {
        InvalidState = 0,
        Static,
        Moving,
    };

    public State currentState { get; set; }

    public Tile(){
    }
}
