using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour {

	public 	TileType 		Type;
	public 	Vector2			Pos;
	
	private	Sprite			TileSprite;
	private SpriteRenderer 	renderer;

	public void init(int x, int y) {
	// Set the initial parameters of the Tile. Add sprite
	// renderer and set world x/y position.

		renderer = gameObject.AddComponent<SpriteRenderer>() as SpriteRenderer;
		Pos = new Vector2(x, y);
		SetWorldPos((int)Pos.x, (int)Pos.y);
		SetName();
	}

	public void SetWorldPos(int x, int y) {
	// Set the x/y position of the tile in the world and grab
	// relevant sprite

		Pos = new Vector2(x, y);
		transform.position = new Vector3(Pos.x, Pos.y, 0);
		SetTileType();
		SetName();
	}

	private void SetName() {
	// Updates the tile's name with it's current info

		gameObject.name = "Tile-" + Type.ToString() + " " + transform.position;
	}

	private void SetTileType() {
	// Sets the tiles type and sprite based off its x/y position
	// in the world

		int NumberOfTileTypes = Enum.GetNames(typeof(TileType)).Length;
		int tileTypeAsInt = PerlinNoiseMap.GetValueInRange ((int)Pos.x, (int)Pos.y, WorldConfig.Seed, NumberOfTileTypes);
		Type = (TileType)tileTypeAsInt;
		SetSprite();
	}

	public void SetSprite() {
	// sets the tile sprite based on it's current type

		switch (Type) {
			case TileType.Grass:
				renderer.sprite = Resources.Load<Sprite>("art/tiles/Grass");
				break;
			case TileType.Dessert:
				renderer.sprite = Resources.Load<Sprite>("art/tiles/Dessert");
				break;
			case TileType.PurpleRock:
				renderer.sprite = Resources.Load<Sprite>("art/tiles/Purple_Rock");
				break;
			case TileType.RedRock:
				renderer.sprite = Resources.Load<Sprite>("art/tiles/Red_Rock");
				break;
			default:
				renderer.sprite = Resources.Load<Sprite>("Grass");
				Debug.LogWarning("TileType of " + Type + " does not exist. Setting TileType to Grass");
				break;
		}
	}
}
