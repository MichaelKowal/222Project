using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tile : MonoBehaviour {

	public GridPoint GridPos { get; private set; }

	//where the tile is
	public Vector3 WorldPos
	{
		get
		{
			return new Vector3(transform.position.x + (GetComponent<SpriteRenderer>().bounds.size.x / 2), 
				transform.position.y + (GetComponent<SpriteRenderer>().bounds.size.y / 2));
		}
	}

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	//used by board manager to create tiles
	public void Setup(GridPoint gridPos, Vector3 worldPos, Transform parent)
	{
		this.GridPos = gridPos;
		transform.position = worldPos;

		transform.SetParent(parent);

		BoardManager.Instance.Tiles.Add( gridPos, this);

	}
}
