using UnityEngine;
using System.Collections.Generic;

public class Board : MonoBehaviour {
    public int width;
    public int height;
    public GameObject[] tiles;
    public GameObject floor;

    private Renderer backgroundRenderer;
    private List<TileController> tileControllers;

    private void Start () {
        tileControllers = new List<TileController>();

        var backgroundGo = GameObject.FindWithTag("Background");
        Debug.Assert(backgroundGo != null, "Error, Board.cs - Could not find object tagged with Background");
        backgroundRenderer = backgroundGo.GetComponent<Renderer>();
        Debug.Assert(backgroundRenderer != null, "Error, Board.cs - Could not find Renderer on Background");
        backgroundRenderer.transform.localScale = new Vector3(width, height, 1);

        try {
            var floorGo = (GameObject)Instantiate(floor, Vector3.zero, Quaternion.identity);
            var floorCollider = floorGo.GetComponent<Collider>();
            Debug.Assert(floorCollider != null, "Error, Board.cs - Could not find Collider on Floor object");
            var offset = floorCollider.bounds.extents + backgroundRenderer.bounds.extents;
            offset.x = 0;
            offset.z = 0;
            floorGo.transform.position -= offset;
            floorGo.transform.localScale = new Vector3(width, 1, 1);
        }
        catch(System.InvalidCastException) {
            Debug.LogError("Error, Board.cs - Could not instantiate Floor GameObject");
        }

        PopulateBoard();
    }

    private void Update () {
    
    }

    private void CheckRow() {

    }

    private void PopulateBoard() {
        for(int y = 0; y < height; ++y) {
            GenerateRow(y);
        }
    }

    private void GenerateRow(float y) {
        var controllerRandom = new ControlledRandom(tiles.Length-1, 1);
        for(var x = 0; x < width; ++x) {
            var offset = Vector3.one * 0.5f - backgroundRenderer.bounds.extents;
            AddTile(new Vector3(x, y, 0) + offset, tiles[controllerRandom.NextIndex()]);
        }
    }
    
    private void AddTile(Vector3 position, GameObject tileObject) {
        try {
            var tile = (GameObject)Instantiate(tileObject, position, Quaternion.identity);
            var tileController = tile.GetComponent<TileController>();
            Debug.Assert(tileController != null, "Error, Board.cs - Could not find TileController on tile");
            tileControllers.Add(tileController);
        }
        catch (System.InvalidCastException) {
            Debug.LogError("Error, Board.cs - Could not instantiate Tile GameObject");
        }
    }
}
