using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleControl : MonoBehaviour {


    // Use this for initialization
    public Grid grid;
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnMouseDown()
    {
   //     grid.AddBottomRow();
  //        StartCoroutine(grid.addRow());
           // grid.NumDropsIndicator.GetComponent<numDropIndicator>().SetDropCount(Random.Range(0,10));      

           if (this.name == "Hammer") {
            grid.enterHammerMode();
           } else if (this.name == "Bomb") {
            grid.enterBombMode();
           }
    }
}
