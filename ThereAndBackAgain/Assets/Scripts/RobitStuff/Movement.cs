using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Movement : MonoBehaviour {

	public float moveSpeed = 1.5f;
	private Vector3 target;
	private Tile tile;
	private int counter = 0;

	// Use this for initialization
	void Start ()
	{
		
	}

	// Update is called once per frame
	void Update ()
	{
		counter++;
		if (counter == 60)
		{
			target = new Vector3 (transform.position.x + moveSpeed, transform.position.y);
			Move ();
			counter = 0;
		}
	}

	void Move () {
		Vector3 int_target = new Vector3 ((int)Math.Floor(target.x), (int)Math.Floor(target.y), target.z);
		transform.position = Vector3.MoveTowards(transform.position, int_target, moveSpeed * Time.deltaTime);
	}
}
