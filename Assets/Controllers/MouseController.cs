using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseController : MonoBehaviour
{
    public GameObject circleCursorPrefab;

    Tile.TileType buildModeTile = Tile.TileType.Floor;

    // World position of mouse frame current/last
    Vector3 _currFramePosition;
    Vector3 _lastFramePosition;

    // World position for start of drag
    Vector3 _dragStartPosition;

    List<GameObject> _draggedOverObjects;

	// Use this for initialization
	void Start () {
		_draggedOverObjects = new List<GameObject>();
	}
	
	// Update is called once per frame
    void Update()
    {
        _currFramePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _currFramePosition.z = 0;

        //UpdateCursor();
        UpdateDragging();
        UpdateCameraMovement();

        _lastFramePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _lastFramePosition.z = 0;
    }

    //void UpdateCursor()
    //{
    //    // Update cursor position
    //    var tileUnderMouse = WorldController.Instance.GetTileAtWorldCoord(_currFramePosition);
    //    if (tileUnderMouse != null)
    //    {
    //        CursorCircle.SetActive(true);
    //        var cursorPosition = new Vector3(tileUnderMouse.X, tileUnderMouse.Y, 0);
    //        CursorCircle.transform.position = cursorPosition;
    //    }
    //    else
    //    {
    //        CursorCircle.SetActive(false);
    //    }
    //}

    void UpdateDragging()
    {
        // If over a UI element.. CUT.. IT.. OUT
        if (EventSystem.current.IsPointerOverGameObject()){ return; }

        // Start of mouse drag
        if (Input.GetMouseButtonDown(0))
        {
            _dragStartPosition = _currFramePosition;
        }

        var startX = Mathf.FloorToInt(_dragStartPosition.x);
        var endX = Mathf.FloorToInt(_currFramePosition.x);
        var startY = Mathf.FloorToInt(_dragStartPosition.y);
        var endY = Mathf.FloorToInt(_currFramePosition.y);
        if (endX < startX)
        {
            var tmp = endX;
            endX = startX;
            startX = tmp;
        }
        if (endY < startY)
        {
            var tmp = endY;
            endY = startY;
            startY = tmp;
        }

        while (_draggedOverObjects.Count > 0)
        {
            var go = _draggedOverObjects[0];
            _draggedOverObjects.RemoveAt(0);
            SimplePool.Despawn(go);
        }

        if (Input.GetMouseButton(0))
        {
            // Display preview of drag
            for (var x = startX; x <= endX; x++) {
                for (var y = startY; y <= endY; y++) {
                    var t = WorldController.Instance.World.GetTileAt(x, y);
                    if (t != null)
                    {
                        var go = SimplePool.Spawn(circleCursorPrefab, new Vector3(x, y, 0), Quaternion.identity);
                        go.transform.SetParent(transform, true);
                        _draggedOverObjects.Add(go);
                    }
                }
            }
        }

        //End of mouse drag
        if (Input.GetMouseButtonUp(0))
        {
            // Loop through all tiles
            for (var x = startX; x <= endX; x++) {
                for (var y = startY; y <= endY; y++) {
                    var t = WorldController.Instance.World.GetTileAt(x, y);
                    if (t != null)
                    {
                        t.Type = buildModeTile;
                    }
                }
            }
        }
    }

    void UpdateCameraMovement()
    {
        // Handle screen dragging
        if (Input.GetMouseButton(1) || Input.GetMouseButton(2))
        {
            var diff = _lastFramePosition - _currFramePosition;
            Camera.main.transform.Translate(diff);
        }

        Camera.main.orthographicSize -= Camera.main.orthographicSize * Input.GetAxis("Mouse ScrollWheel");
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, 3f, 30f);
    }

    public void SetModeBuildFloor() { buildModeTile = Tile.TileType.Floor; }
    public void SetModeBulldoze() { buildModeTile = Tile.TileType.Empty; }
}
