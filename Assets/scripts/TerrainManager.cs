using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainManager : MonoBehaviour {

	public 	Sprite[] 	TileableSprites;
	public 	float 		HorizontalRedrawDistance = 2;
	public 	float 		VerticalRedrawDistance = 4;
	public 	int 		Key = 1;
	public 	Transform 	Player;
	public 	GameObject	TilePrefab;

	private	int 				Cols = 25;
	private	int 				Rows = 18;
	private Vector3 			offset;
	private TileManager[,]	 	_tiles;
	private IEnumerable<Marker>	_markers;


	// Use this for initialization
	void Start () {
		initMap ();
		RedrawMap();
	}


	void Update () {
		Vector2 DistanceFromMapCentre = GetDistance(Player.position, transform.position);

		if (DistanceFromMapCentre.x > HorizontalRedrawDistance ||
			DistanceFromMapCentre.y > VerticalRedrawDistance) {
			RedrawMap();
		}
	}


	void initMap () {
	// Init our map by creating a pool of tiles

		offset = new Vector3 (0 - Cols / 2, 0 - Rows / 2, 0);
		_tiles = new TileManager[Cols, Rows];
		GameObject tilePrefab;

		for (int x = 0; x < Cols; x++) {
			for (int y = 0; y < Rows; y++) {
				tilePrefab = GameObject.Instantiate(TilePrefab, transform.position, Quaternion.identity) as GameObject;
				tilePrefab.transform.parent = transform;

				TileManager tile = tilePrefab.GetComponent<TileManager>();
				tile.init(x + (int)offset.x, y + (int)offset.y);
				_tiles[x,y] = tile;
			}
		}
	}

	private Vector2 GetDistance (Vector3 pointA, Vector3 pointB) {
	// Returns distance as a Vector2 given to Vector3s (Vector3s used for
	// game objects transforms)

		Vector2 distance = new Vector2();

		// Save the distances between the x and y coords
		distance.x = pointA.x - pointB.x;
		distance.y = pointA.y - pointB.y;

		// Flip negative values into positive ones
		distance.x = distance.x > 0 ? distance.x : -distance.x;
		distance.y = distance.y > 0 ? distance.y : -distance.y;

		return distance;
	}


	void RedrawMap() {
	// Redraws our map to the screen. Used when our player gets close to the
	// edge of the currently drawn map

		// Move the transform of this TerrainManager to the same position as
		// the player. This allows us to redraw the map at a different location
		// of the noise map
		transform.position = new Vector3 (
			(int)Player.position.x,
			(int)Player.position.y,
			transform.position.z );

		for (int x = 0; x < Cols; x++) {
			for (int y = 0; y < Rows; y++) {
				TileManager tile = _tiles[x,y];
				int newX = (int)gameObject.transform.position.x + x + (int)offset.x;
				int newY = (int)gameObject.transform.position.y + y + (int)offset.y;
				if (x == 0 && y == 0){
					Debug.Log(transform.position);
				}
				tile.SetWorldPos(newX, newY);
			}
		}

		Debug.Log("Redrawing Complete");
	}
}
