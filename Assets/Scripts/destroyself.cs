﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyself : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void destroySelf()
    {
        Destroy(this.gameObject);
    }
}
