using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager> {

	private BoardManager boardScript;

	private int level = 1;

	void Awake() {
		boardScript = GetComponent<BoardManager>();
		InitGame ();
	}

	// Use this for initialization
	void InitGame() {
		boardScript.SetupScene (level);

	}
}
