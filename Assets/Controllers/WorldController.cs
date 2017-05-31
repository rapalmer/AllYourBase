using UnityEngine;
using System.Collections;

public class WorldController : MonoBehaviour
{

    public static WorldController Instance { get; protected set; }

	// The only tile sprite we have right now, so this
	// it a pretty simple way to handle it.
	public Sprite FloorSprite;

	// The world and tile data
	public World World { get; protected set; }

	// Use this for initialization
	void Start () {
	    if (Instance != null)
	    {
	        Debug.LogError("There can only be one! ..WorldController.");
	    }
	    Instance = this;

        // Create a world with Empty tiles
	    World = new World();

		// Create a GameObject for each of our tiles, so they show visually. (and redunt reduntantly)
		for (var x = 0; x < World.Width; x++) {
			for (var y = 0; y < World.Height; y++) {
				
                // Get the tile data
				var tileData = World.GetTileAt(x, y);

				// This creates a new GameObject and adds it to our scene.
			    var tileGo = new GameObject {name = "Tile_" + x + "_" + y};
			    tileGo.transform.position = new Vector3( tileData.X, tileData.Y, 0);
                tileGo.transform.SetParent(transform, true);

				// Add a sprite renderer, but don't bother setting a sprite
				// because all the tiles are empty right now.
				tileGo.AddComponent<SpriteRenderer>();

				// Use a lambda to create an anonymous function to "wrap" our callback function
				tileData.RegisterTileTypeChangedCallback( (tile) => { OnTileTypeChanged(tile, tileGo); } );
			}
		}

        // Shake things up, for testing.
	    World.RandomizeTiles();
	}

	// Update is called once per frame
	void Update () {

	}

	// This function should be called automatically whenever a tile's type gets changed.
	void OnTileTypeChanged(Tile tileData, GameObject tileGo)
	{
	    switch (tileData.Type)
	    {
	        case Tile.TileType.Floor:
	            tileGo.GetComponent<SpriteRenderer>().sprite = FloorSprite;
	            break;
	        case Tile.TileType.Empty:
	            tileGo.GetComponent<SpriteRenderer>().sprite = null;
	            break;
	        default:
	            Debug.LogError("OnTileTypeChanged - Unrecognized tile type.");
	            break;
	    }
	}

    /// <summary>
    /// Gets the tile at the given coordinates
    /// </summary>
    /// <param name="coord">Coordinates in Vector3</param>
    /// <returns>The tile at the given coordinates</returns>
    public Tile GetTileAtWorldCoord(Vector3 coord)
    {
        var x = Mathf.FloorToInt(coord.x);
        var y = Mathf.FloorToInt(coord.y);
        return World.GetTileAt(x, y);
    }
}
