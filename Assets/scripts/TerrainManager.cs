using System.Collections;
using UnityEngine;

public class TerrainManager : MonoBehaviour {

	public 	Sprite[] 	TileableSprites;
	public 	int 		NumHorizontalTiles = 25;
	public 	int 		NumVerticalTiles = 25;
	public 	int 		Key = 1;
	public 	Transform 	Player;
	public 	float 		HorizontalRedrawDistance = 5;
	public 	float 		VerticalRedrawDistance = 5;

	private SpriteRenderer[,] _renderers;

	// Use this for initialization
	void Start () {
		var offset = new Vector3 (0 - NumHorizontalTiles / 2, 0 - NumVerticalTiles / 2, 0);
		_renderers = new SpriteRenderer[NumHorizontalTiles, NumVerticalTiles];

		for (int x = 0; x < NumHorizontalTiles; x++) {
			for (int y = 0; y < NumVerticalTiles; y++) {
				var tile = new GameObject ();
				tile.transform.position = new Vector3 (x, y, 0) + offset;
				_renderers[x,y] = tile.AddComponent<SpriteRenderer>();
				tile.name = "Terrain " + tile.transform.position;
				tile.transform.parent = transform;
			}
		}

		RedrawMap();
	}


	void Update () {
		Vector2 DistanceFromMapCentre = GetDistance(Player.position, transform.position);

		if (DistanceFromMapCentre.x > HorizontalRedrawDistance ||
			DistanceFromMapCentre.y > VerticalRedrawDistance) {
			RedrawMap();
		}
	}


	public Sprite GetSpriteFromMap(int x, int y) {
	// Returns a random Sprite. We use our X and Y coords against a Perlin Noise map
	// generated with the Key as a seed so the same random sprite is always returned
	// when the key is same

		return TileableSprites [RandomHelper.Range (x, y, Key, TileableSprites.Length)];
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

		Debug.Log(distance);

		return distance;
	}


	void RedrawMap() {
	// Redraws our map to the screen. Used when our player gets close to the
	// edge of the currently drawn map

		Debug.Log("Redrawing...");

		// Move the transform of this TerrainManager to the same position as
		// the player. This allows us to redraw the map at a different location
		// of the noise map
		transform.position = new Vector3(
			(int)Player.position.x,
			(int)Player.position.y,
			transform.position.z );

		for (int x = 0; x < NumHorizontalTiles; x++) {
			for (int y = 0; y < NumVerticalTiles; y++) {
				var spriteRenderer = _renderers[x,y];

				// We add the current x and y position to our position in the array.
				// This allows us to grab the number of tiles we want (NumHorizontalTiles) 
				// at our current position (int)transform.position.x
				spriteRenderer.sprite = GetSpriteFromMap(
					(int)transform.position.x + x,
					(int)transform.position.y + y );
			}
		}

		Debug.Log("Redrawing Complete");
	}
}
