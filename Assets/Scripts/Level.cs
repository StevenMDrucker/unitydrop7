using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour {

	public enum LevelType {
		SCORELEVEL,
		OBSTACLELEVEL,
		FORMATION
	};

	protected LevelType type;
	public Grid grid;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
