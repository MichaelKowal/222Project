using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;

/*
 * This class is responsible for generating all of the tiles that represent the board.
 */
public class BoardManager : Singleton<BoardManager>
{

	[SerializeField]
	private int columns = 10;
	[SerializeField]
	private int rows = 10;

	//lets you move to off screen sections of map
	[SerializeField]
	private CameraMove cameraMovement;

	[SerializeField]
	private Transform boardParent;

	public Dictionary<GridPoint, Tile> Tiles { get; set; }
	private List<Vector2> gridPositions = new List<Vector2>();
	public GameObject  floorTile;
	public GameObject  wallTile;
	public GameObject  outerWallTile;

	public float TileSize
	{
		get { return floorTile.GetComponent<SpriteRenderer>().sprite.bounds.size.x; }
	}

	//Don't see a reason for this. positions can be inferred from row and column number or by looking at tiles
	void InitializeList() {
		gridPositions.Clear ();

		for(int x=0; x < columns-1; x++) {
			for(int y=0; y < rows-1; y++) {
				gridPositions.Add(new Vector3(x,y));
			}
		}
	}

	// creates grid (including outer walls) in columns top to bottom from left to right
	void Boardsetup() {

		Tiles = new Dictionary<GridPoint, Tile>();

		float tileSize = TileSize;
		//Some problems with maximizing on play but not in build
		Vector3 worldStart = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height));
		// boardHolder = new GameObject ("Board").transform;

		for(int x=-1; x < columns+1; x++) {
			for(int y=-1; y < rows+1; y++) {
				GameObject toInstantiate = floorTile; 
				if (x == -1 || y == -1 || x == columns || y == rows) {
					toInstantiate = outerWallTile;
				}
				Tile instance = Instantiate(toInstantiate.GetComponent<Tile>());
				;
				instance.GetComponent<Tile>().Setup(new GridPoint(x, y),
					new Vector3(worldStart.x + 1 + (tileSize * x), worldStart.y - 1 - (tileSize * y)),
					boardParent);
				// Tiles.Add(new GridPoint(x, y), instance);

				//instance.transform.SetParent (boardHolder);
			}
		}

		Vector3 bottomRightTile = Tiles[new GridPoint(columns, rows)].transform.position;

		cameraMovement.SetCamLimit(new Vector3(bottomRightTile.x + tileSize, bottomRightTile.y - tileSize));
		cameraMovement.SetCamLimit(new Vector3(worldStart.x + 1 + tileSize + tileSize * columns, worldStart.y - 1 - tileSize - tileSize * rows));

	}

	public void SetupScene(int level) {
		Boardsetup ();
		InitializeList ();
		//LayoutObjectAtRandom (wallTiles, wallCount.minimum, wallCount.maximum);
		//Instantiate (exit, new Vector3(columns-1, rows-1, 0f), Quaternion.identity);
	}


	void LayoutObjectAtPlace(GameObject placableObject, int x, int y) {
		Vector2 position = new Vector2 (x, y);
		Instantiate (placableObject, position, Quaternion.identity);
	}
}
