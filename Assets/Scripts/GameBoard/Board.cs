using UnityEngine;
using System.Collections.Generic;

public class Board : MonoBehaviour {
    public int width;
    public int height;
    public GameObject[] tiles;
    public GameObject floor;

    private Transform background;

    private void Start () {
        var backgroundGo = GameObject.FindWithTag("Background");
        Debug.Assert(backgroundGo != null, "Error, Board.cs - Could not find object tagged with Background");
        background = backgroundGo.transform;
        var scale = background.localScale;
        scale.x = width;
        scale.y = height;
        background.localScale = scale;

        try {
            var floorGo = (GameObject)Instantiate(floor, Vector3.zero, Quaternion.identity);
            var floorCollider = floorGo.GetComponent<Collider>();
            Debug.Assert(floorCollider != null, "Error, Board.cs - Could not find Collider on Floor object");
            var backgroundRenderer = background.GetComponent<Renderer>();
            Debug.Assert(backgroundRenderer != null, "Error, Board.cs - Could not find Renderer on Background object");
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

    private void PopulateBoard() {
        for(int y = 0; y < height; ++y) {
            GenerateRow(y);
        }
    }

    private void GenerateRow(float y) {
        var availableIndices = new List<int>();
        for(var i = 0; i < tiles.Length; ++i) {
            availableIndices.Add(i);
        }

        var lastIndex = -1;
        var removedIndex = -1;
        var sameIndexCounter = 0;
        for(var x = 0; x < width; ++x) {
            var index = availableIndices[Random.Range(0, availableIndices.Count - 1)];
            if(index == lastIndex) {
                sameIndexCounter++;
            }

            lastIndex = index;

            // Always be sure to only instantiate two of the same object in a row
            // by set aside indices that has occured twice
            if (sameIndexCounter == 1) {
                removedIndex = lastIndex;
                availableIndices.Remove(lastIndex);
                sameIndexCounter = 0;
            }
            
            try {
                var tile = (GameObject)Instantiate(tiles[index], new Vector3(x, y, 0), Quaternion.identity);
                var tileCollider = tile.GetComponent<Collider>();
                Debug.Assert(tileCollider != null, "Error, Board.cs - Could not find Collider on Tile object");
                var backgroundRenderer = background.GetComponent<Renderer>();
                Debug.Assert(backgroundRenderer != null, "Error, Board.cs - Could not find Renderer on Background object");
                var offset = tileCollider.bounds.extents - backgroundRenderer.bounds.extents;
                tile.transform.position += offset;
            }
            catch(System.InvalidCastException) {
                Debug.LogError("Error, Board.cs - Could not instantiate Tile GameObject");
            }

            // Add the removedIndex to the availableIndices list when a diffirent index has been used
            if (removedIndex != -1 && removedIndex != lastIndex && availableIndices.Contains(removedIndex) == false) {
                availableIndices.Add(removedIndex);
            }
        }
    }
}
