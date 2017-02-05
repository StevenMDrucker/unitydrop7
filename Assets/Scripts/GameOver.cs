using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameOver : MonoBehaviour {

	public GameObject parentScreen;
	// Use this for initialization
	void Start () {
		parentScreen.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void ShowWin() {
		Animator animator = GetComponent<Animator>();
		parentScreen.SetActive(true);
		if (animator) {
			animator.Play("GameOverShow");
		}
	}
}
