using UnityEngine;
using System.Collections.Generic;

public class Board : MonoBehaviour {
    public int Width;
    public int Height;

    private List<TileController> tiles;

    private void Start () {
        PopulateBoard();
    }

    private void Update () {
    
    }

    public void PopulateBoard() {

    }
}
