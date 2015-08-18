using UnityEngine;
using System.Collections.Generic;

public class Board : MonoBehaviour {
    public int width;
    public int height;
    public GameObject[] tiles;
    public GameObject floor;

    private Renderer backgroundRenderer;
    private Dictionary<int, TileController> tileControllers = new Dictionary<int, TileController>();
    private GameObject floorObject;

    private void Start () {
        Initialize();
    }

    private void Initialize() {
        var backgroundGo = GameObject.FindWithTag("Background");
        Debug.Assert(backgroundGo != null, "Error, Board.cs - Could not find object tagged with Background");
        backgroundRenderer = backgroundGo.GetComponent<Renderer>();
        Debug.Assert(backgroundRenderer != null, "Error, Board.cs - Could not find Renderer on Background");
        backgroundRenderer.transform.localScale = new Vector3(width, height, 1);

        PopulateBoard();

        if(floorObject == null) {
            try
            {
                floorObject = (GameObject)Instantiate(floor, Vector3.zero, Quaternion.identity);
            }
            catch (System.InvalidCastException)
            {
                Debug.LogError("Error, Board.cs - Could not instantiate Floor GameObject");
            }
        }

        var floorCollider = floorObject.GetComponent<Collider>();
        Debug.Assert(floorCollider != null, "Error, Board.cs - Could not find Collider on Floor object");

        // Placement of floor were all tiles are going to rest on
        var offset = floorCollider.bounds.extents + backgroundRenderer.bounds.extents;
        offset.x = 0;
        offset.z = 0;
        floorObject.transform.position = -offset;
        floorObject.transform.localScale = new Vector3(width, 1, 1);
    }

    private void Update () {
        if(IsBoardStatic()) {
            HandleMouse();
            CheckBoard();
        }
    }

    private void CheckBoard() {
        for (var y = 0; y < height; ++y) {
            RemoveMatchedRow(y);
        }
    }

    private void HandleMouse() {
        if(Input.GetButtonDown("Fire1")) {
            RaycastHit hit;
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out hit)) {
                if(hit.collider.tag == "Tile") {
                    RemoveTile(hit.collider.gameObject);
                }
            }
        }
    }

    private void RemoveMatchedRow(float y) {
        var offset = Vector3.one * 0.5f - backgroundRenderer.bounds.extents;
        var tiles = new List<GameObject>();
        var lastName = "";
        for (var x = 0; x < width; ++x) {
            var tile = GetTileAt(x + offset.x, y + offset.y);
            if(tile == null) {
                RemoveMatched(ref tiles, 3);
                continue;
            }
            if (tile.name != lastName) {
                RemoveMatched(ref tiles, 3);
            }
            lastName = tile.name;
            tiles.Add(tile);
        }
        RemoveMatched(ref tiles, 3);
    }

    private void RemoveMatched(ref List<GameObject> tileList, int matchedRowLength) {
        if(tileList.Count < matchedRowLength) {
            tileList.Clear();
        }

        while (tileList.Count > 0) {
            RemoveTile(tileList[0]);
            tileList.RemoveAt(0);
        }
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
            tile.transform.parent = transform;
            var tileController = tile.GetComponent<TileController>();
            Debug.Assert(tileController != null, "Error, Board.cs - Could not find TileController on tile");
            tileControllers.Add(tileController.Id, tileController);
        }
        catch (System.InvalidCastException) {
            Debug.LogError("Error, Board.cs - Could not instantiate Tile GameObject");
        }
    }

    private void RemoveTile(GameObject tileObj){
        var tileController = tileObj.GetComponent<TileController>();
        Debug.Assert(tileController != null, "Error, Board.cs - Could not find TileController on tile");
        tileControllers.Remove(tileController.Id);
        Destroy(tileObj);
    }

    private GameObject GetTileAt(float x, float y) {
        var position = new Vector3(x,y,-10);
        RaycastHit hit;
        if (Physics.Raycast(position, Vector3.forward, out hit, 100)) {
            return hit.collider.gameObject;
        }
        return null;
    }

    private bool IsBoardStatic() {
        var isStatic = true;
        foreach(KeyValuePair<int, TileController> entry in tileControllers) {
            if(entry.Value.state == TileController.State.Moving) {
                isStatic = false;
                break;
            }
        }
        return isStatic;
    }

    public void ReBuildBoard() {
        foreach (KeyValuePair<int, TileController> entry in tileControllers) {
            Destroy(entry.Value.gameObject);
        }
        tileControllers.Clear();
        Initialize();
    }

    public void SetWidth(int boardWidth) {
        width = boardWidth;
    }

    public void SetHeight(int boardHeight) {
        height = boardHeight;
    }
}
