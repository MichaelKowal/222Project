using UnityEngine;
using System;
using System.Collections.Generic;

 		//Allows us to use Lists.
using Random = UnityEngine.Random;

 		//Tells Random to use the Unity Engine random number generator.

namespace Completed
{
	
	public class BoardManager : MonoBehaviour
	{
		// Using Serializable allows us to embed a class with sub properties in the inspector.
		[Serializable]
		public class Count
		{
			public int minimum;
			//Minimum value for our Count class.
			public int maximum;
			//Maximum value for our Count class.
			
			
			//Assignment constructor.
			public Count (int min, int max)
			{
				minimum = min;
				maximum = max;
			}
		}

		
		public int columns = 8;
		//Number of columns in our game board.
		public int rows = 8;
		//Number of rows in our game board.
		public Count wallCount = new Count (5, 9);
		//Lower and upper limit for our random number of walls per level.
		public Count foodCount = new Count (1, 5);
		//Lower and upper limit for our random number of food items per level.
		public GameObject exit;
		//Prefab to spawn for exit.
		public GameObject[] floorTiles;
		//Array of floor prefabs.
		public GameObject[] wallTiles;
		//Array of wall prefabs.
		public GameObject[] foodTiles;
		//Array of food prefabs.
		public GameObject[] enemyTiles;
		//Array of enemy prefabs.
		public GameObject[] outerWallTiles;
		//Array of outer tile prefabs.
		
		private Transform boardHolder;
		//A variable to store a reference to the transform of our Board object.
		private List <Vector3> gridPositions = new List <Vector3> ();
		//A list of possible locations to place tiles.
		
		
		//Clears our list gridPositions and prepares it to generate a new board.
		void InitialiseList ()
		{
			//Clear our list gridPositions.
			gridPositions.Clear ();
			
			//Loop through x axis (columns).
			for (int x = 0; x < columns; x++) {
				//Within each column, loop through y axis (rows).
				for (int y = 0; y < rows; y++) {
					//At each index add a new Vector3 to our list with the x and y coordinates of that position.
					gridPositions.Add (new Vector3 (x, y, 0f));
				}
			}
		}
		
		
		//Sets up the outer walls and floor (background) of the game board.
		void BoardSetup ()
		{
			//Graeme : This matrix is to set values for the maze creation algorithm
			int[,] primGrid = new int[rows, columns];
			int countX = 0;
			int countY = 0;

			//Instantiate Board and set boardHolder to its transform.
			boardHolder = new GameObject ("Board").transform;

			//Loop along x axis, starting from -1 (to fill corner) with floor or outerwall edge tiles.
			for (int x = -1; x < columns + 1; x++) {
				//Loop along y axis, starting from -1 to place floor or outerwall tiles.
				for (int y = -1; y < rows + 1; y++) {
					//Choose a random tile from our array of floor tile prefabs and prepare to instantiate it.
					GameObject toInstantiate = floorTiles [Random.Range (0, floorTiles.Length)];

					//Graeme : Setting a value for each tile in the gameboard 
					primGrid [countX, countY] = Random.Range(0, 50);
					
					//Check if we current position is at board edge, if so choose a random outer wall prefab from our array of outer wall tiles.
					if (x == -1 || x == columns || y == -1 || y == rows) {
						toInstantiate = outerWallTiles [Random.Range (0, outerWallTiles.Length)];
					} 


//					//Graeme: I've added this else to fill the board with wall tiles, except the tiles the lock and player are spawned.
//					else if  ((x != 0 && y != rows-1) || (x != columns-1 && y != 0)){
//
//							toInstantiate = wallTiles [Random.Range (0, wallTiles.Length)];
//					}



					//Instantiate the GameObject instance using the prefab chosen for toInstantiate at the Vector3 corresponding to current grid position in loop, cast it to GameObject.
					GameObject instance =
						Instantiate (toInstantiate, new Vector3 (x, y, 0f), Quaternion.identity) as GameObject;
					
					//Set the parent of our newly instantiated object instance to boardHolder, this is just organizational to avoid cluttering hierarchy.
					instance.transform.SetParent (boardHolder);
				}
			}// end loops

			//Another set of nested For Loops to go through the grid and remove sections to create the maze.
			//Going back on this, Maybe just send the matrix to the Generate Maze Class.
		


		}

		//Random Board using Prim Maze Algorithm - the method is currently not used!
		void GeneratePrimMaze ()
		{
			//Instantiate Board and set boardHolder to its transform.
			boardHolder = new GameObject ("Board").transform;

			//Loop along x axis, starting from -1 (to fill corner) with floor or outerwall edge tiles.
			for (int x = 0; x < columns + 1; x++) {
				//Loop along y axis, starting from -1 to place floor or outerwall tiles.
				for (int y = 0; y < rows + 1; y++) {
					//Choose a random tile from our array of floor tile prefabs and prepare to instantiate it.
					GameObject toInstantiate = floorTiles [Random.Range (0, floorTiles.Length)];

					//Check if we current position is at board edge, if so choose a random outer wall prefab from our array of outer wall tiles.
					if (x == -1 || x == columns || y == -1 || y == rows) {
						
						toInstantiate = outerWallTiles [Random.Range (0, outerWallTiles.Length)];
					} 
					else {
						toInstantiate = wallTiles [Random.Range (0, wallTiles.Length)];
					}

					//Instantiate the GameObject instance using the prefab chosen for toInstantiate at the Vector3 corresponding to current grid position in loop, cast it to GameObject.
					GameObject instance =
						Instantiate (toInstantiate, new Vector3 (x, y, 0f), Quaternion.identity) as GameObject;

					//Set the parent of our newly instantiated object instance to boardHolder, this is just organizational to avoid cluttering hierarchy.
					instance.transform.SetParent (boardHolder);
				}
			}
		}
		
		
		//RandomPosition returns a random position from our list gridPositions.
		Vector3 RandomPosition ()
		{
			//Declare an integer randomIndex, set it's value to a random number between 0 and the count of items in our List gridPositions.
			int randomIndex = Random.Range (0, gridPositions.Count);
			
			//Declare a variable of type Vector3 called randomPosition, set it's value to the entry at randomIndex from our List gridPositions.
			Vector3 randomPosition = gridPositions [randomIndex];
			
			//Remove the entry at randomIndex from the list so that it can't be re-used.
			gridPositions.RemoveAt (randomIndex);
			
			//Return the randomly selected Vector3 position.
			return randomPosition;
		}
		
		
		//LayoutObjectAtRandom accepts an array of game objects to choose from along with a minimum and maximum range for the number of objects to create.
		//This would be the method to setup the wall tiles. 
		void LayoutObjectAtRandom (GameObject[] tileArray, int minimum, int maximum)
		{
			//Choose a random number of objects to instantiate within the minimum and maximum limits
			int objectCount = Random.Range (minimum, maximum + 1);
			
			//Instantiate objects until the randomly chosen limit objectCount is reached
			for (int i = 0; i < objectCount; i++) {
				//Choose a position for randomPosition by getting a random position from our list of available Vector3s stored in gridPosition
				Vector3 randomPosition = RandomPosition ();
				
				//Choose a random tile from tileArray and assign it to tileChoice
				GameObject tileChoice = tileArray [Random.Range (0, tileArray.Length)];
				
				//Instantiate tileChoice at the position returned by RandomPosition with no change in rotation
				Instantiate (tileChoice, randomPosition, Quaternion.identity);
			}
		}

		void LayoutTileIndexManyLoops (GameObject[] tileArray)
		{
			//Vector3[]maze = new Vector3[];
			//This is hardcoded and ugly as all hell.
//			Vector3 position1 = gridPositions [1];
			Vector3 positionTemp = gridPositions[1];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[5];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[6];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[7];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[9];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[10];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[11];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[12];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[14];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[16];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[18];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[22];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[29];
			positionTemp = gridPositions[5];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[31];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[32];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[33];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[34];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[36];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[37];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[39];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[41];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[43];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[44];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[51];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[54];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[55];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[56];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[60];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[61];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[62];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[64];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[66];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[67];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[73];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[74];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[77];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[79];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[84];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[85];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[87];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[88];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[91];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[92];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[94];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[95];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[96];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[97];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[99];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[112];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[113];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[114];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[115];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[116];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[117];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[119];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[121];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[122];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[123];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[124];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[125];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[127];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[136];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[144];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[145];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[146];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[147];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[148];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[151];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[153];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[155];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[157];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[161];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[166];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[167];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[168];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[170];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[171];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[172];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[174];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[176];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[178];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[183];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[187];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[189];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[195];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[196];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[198];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[200];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[201];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[202];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[203];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[204];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[206];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[207];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[208];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[209];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[211];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[217];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[226];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[228];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[230];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[231];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[232];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[233];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[234];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[235];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[236];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[238];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[239];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[243];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[248];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[256];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[258];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[259];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[260];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[261];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[263];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[265];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[266];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[267];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[268];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[269];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[271];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[281];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[288];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[289];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[290];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[291];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[292];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[293];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[294];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[298];
			LayoutTile (tileArray, positionTemp);

			LayoutTile (tileArray, positionTemp);LayoutTile (tileArray, positionTemp);
			
		}

		void LayoutTileIndexOneLargeLoop (GameObject[] tileArray)
		{
			//Vector3[]maze = new Vector3[];
			//This is hardcoded and ugly as all hell.
			//			Vector3 position1 = gridPositions [1];
			Vector3 positionTemp = gridPositions[1];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[5];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[6];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[7];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[9];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[10];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[11];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[12];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[14];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[16];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[18];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[22];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[29];
			positionTemp = gridPositions[26];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[31];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[32];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[33];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[34];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[36];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[37];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[39];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[41];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[43];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[44];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[51];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[54];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[55];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[56];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[60];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[61];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[62];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[64];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[66];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[67];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[73];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[74];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[77];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[79];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[84];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[85];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[87];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[88];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[91];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[92];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[94];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[95];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[96];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[97];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[99];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[112];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[113];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[114];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[115];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[116];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[117];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[119];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[121];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[122];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[123];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[124];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[125];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[126];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[127];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[134];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[136];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[142];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[144];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[145];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[146];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[147];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[148];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[149];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[151];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[153];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[155];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[157];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[161];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[166];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[167];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[168];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[170];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[171];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[172];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[174];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[176];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[178];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[179];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[183];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[187];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[189];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[195];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[196];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[198];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[200];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[201];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[202];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[203];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[204];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[206];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[207];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[208];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[209];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[211];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[217];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[226];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[228];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[230];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[231];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[232];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[233];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[234];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[235];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[236];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[238];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[239];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[243];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[248];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[254];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[256];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[258];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[259];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[260];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[261];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[263];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[265];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[266];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[267];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[268];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[269];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[271];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[272];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[273];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[281];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[288];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[289];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[290];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[291];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[292];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[293];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[294];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[298];
			LayoutTile (tileArray, positionTemp);

			LayoutTile (tileArray, positionTemp);LayoutTile (tileArray, positionTemp);

		}

		void LayoutTileIndexNoLoops (GameObject[] tileArray)
		{
			//Vector3[]maze = new Vector3[];
			//This is hardcoded and ugly as all hell.
			//			Vector3 position1 = gridPositions [1];
			Vector3 positionTemp = gridPositions[1];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[5];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[6];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[7];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[9];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[10];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[11];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[12];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[14];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[16];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[18];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[22];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[29];
			positionTemp = gridPositions[26];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[31];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[32];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[33];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[34];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[36];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[37];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[39];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[41];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[43];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[44];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[51];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[54];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[55];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[56];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[60];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[61];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[62];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[64];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[66];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[67];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[73];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[74];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[77];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[79];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[84];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[85];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[87];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[88];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[91];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[92];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[94];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[95];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[96];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[97];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[99];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[112];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[113];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[114];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[115];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[116];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[117];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[119];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[121];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[122];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[123];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[124];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[125];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[126];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[127];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[134];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[136];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[142];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[144];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[145];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[146];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[147];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[148];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[149];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[151];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[153];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[155];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[157];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[161];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[166];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[167];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[168];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[170];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[171];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[172];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[174];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[176];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[178];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[179];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[183];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[187];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[189];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[195];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[196];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[198];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[200];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[201];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[202];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[203];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[204];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[206];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[207];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[208];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[209];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[211];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[217];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[226];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[228];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[230];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[231];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[232];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[233];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[234];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[235];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[236];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[238];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[239];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[243];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[248];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[254];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[256];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[258];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[259];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[260];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[261];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[262];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[263];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[265];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[266];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[267];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[268];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[269];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[271];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[272];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[273];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[281];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[288];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[289];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[290];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[291];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[292];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[293];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[294];
			LayoutTile (tileArray, positionTemp);
			positionTemp = gridPositions[298];
			LayoutTile (tileArray, positionTemp);

			LayoutTile (tileArray, positionTemp);LayoutTile (tileArray, positionTemp);

		}
		void LayoutTile (GameObject[] tileArray, Vector3 position)
		{

			//Choose a random tile from tileArray and assign it to tileChoice
			GameObject tileChoice = tileArray [Random.Range (0, tileArray.Length)];

			//Instantiate tileChoice at the position returned by RandomPosition with no change in rotation
			Instantiate (tileChoice, position, Quaternion.identity);
		}
		
		
		//SetupScene initializes our level and calls the previous functions to lay out the game board
		public void SetupScene (int level)
		{
			//Creates the outer walls and floor.
			BoardSetup ();
			
			//Reset our list of gridpositions.
			InitialiseList ();
			
			//Instantiate a random number of wall tiles based on minimum and maximum, at randomized positions.
			//This is the call that needs to change to another method that generates the maze.

			//Call if you want many loops in the maze
			LayoutTileIndexManyLoops (wallTiles);

			//Call if you want one large loop in the maze
			//LayoutTileIndexOneLargeLoop (wallTiles);

			//Call if you want no loops
			//LayoutTileIndexNoLoops (wallTiles);
			
			//Instantiate a random number of food tiles based on minimum and maximum, at randomized positions.
			LayoutObjectAtRandom (foodTiles, foodCount.minimum, foodCount.maximum);
			
			//Determine number of enemies based on current level number, based on a logarithmic progression
			int enemyCount = (int)Mathf.Log (level, 2f);
			
			//Instantiate a random number of enemies based on minimum and maximum, at randomized positions.
			LayoutObjectAtRandom (enemyTiles, enemyCount, enemyCount);
			
			//Instantiate the exit tile in the upper right hand corner of our game board
			Instantiate (exit, new Vector3 (columns - 1, rows - 1, 0f), Quaternion.identity);
		}
	}
}
