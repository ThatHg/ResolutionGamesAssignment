using UnityEngine;
using System.Collections.Generic;

public class Board : MonoBehaviour {
    public int width;
    public int height;
    public GameObject[] tiles;

    private Transform background;

    private void Start () {
        var backgroundGo = GameObject.FindWithTag("Background");
        Debug.Assert(backgroundGo != null, "Error, Board.cs - Could not find object tagged with Background");
        background = backgroundGo.transform;
        var scale = background.localScale;
        scale.x = width;
        scale.y = height;
        background.localScale = scale;

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
                var tileRenderer = tile.GetComponent<Renderer>();
                Debug.Assert(tileRenderer != null, "Error, Board.cs - Could not find Renderer on Tile object");
                var backgroundRenderer = background.GetComponent<Renderer>();
                Debug.Assert(tileRenderer != null, "Error, Board.cs - Could not find Renderer on Background object");
                var offset = tileRenderer.bounds.extents - backgroundRenderer.bounds.extents;
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
