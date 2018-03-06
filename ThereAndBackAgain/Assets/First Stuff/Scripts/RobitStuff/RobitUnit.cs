using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobitUnit : MonoBehaviour {

	public List<Sprite> RobitSprites = new List<Sprite>();

	void Update()
	{
		
	}
	// Use this for initialization
	void Start()
	{
		GetComponent<SpriteRenderer>().sprite = RobitSprites[Random.Range(0, RobitSprites.Count)];
	}
}
