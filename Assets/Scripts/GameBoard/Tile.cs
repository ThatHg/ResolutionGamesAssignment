public class Tile {
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

    public Type type { get; set; }
    public State currentState { get; set; }

    public Tile(){ 

    }
}
