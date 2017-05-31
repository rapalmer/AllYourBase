using UnityEngine;
using System.Collections;

public class World {

	// A two-dimensional array to hold our tile data.
    readonly Tile[,] _tiles;

	// The tile width of the world.
	public int Width { get; protected set; }

	// The tile height of the world
	public int Height { get; protected set; }

	/// <summary>
	/// Initializes a new instance of the <see cref="World"/> class.
	/// </summary>
	/// <param name="width">Width in tiles.</param>
	/// <param name="height">Height in tiles.</param>
	public World(int width = 100, int height = 100) {
		Width = width;
		Height = height;

		_tiles = new Tile[Width,Height];

		for (var x = 0; x < Width; x++) {
			for (var y = 0; y < Height; y++) {
				_tiles[x,y] = new Tile(this, x, y);
			}
		}
		Debug.Log ("World created with " + (Width*Height) + " tiles.");
	}

	/// <summary>
	/// A function for testing out the system
	/// </summary>
	public void RandomizeTiles() {
		Debug.Log ("RandomizeTiles");
		for (var x = 0; x < Width; x++) {
			for (var y = 0; y < Height; y++)
			{
			    _tiles[x,y].Type = Random.Range(0, 2) == 0 ? Tile.TileType.Empty : Tile.TileType.Floor;
			}
		}
	}

	/// <summary>
	/// Gets the tile data at x and y.
	/// </summary>
	/// <returns>The <see cref="Tile"/>.</returns>
	/// <param name="x">The x coordinate.</param>
	/// <param name="y">The y coordinate.</param>
	public Tile GetTileAt(int x, int y) {
	    if (x <= Width && x >= 0 && y <= Height && y >= 0) return _tiles[x, y];
	    Debug.LogError("Tile ("+x+","+y+") is out of range.");
	    return null;
	}

}
