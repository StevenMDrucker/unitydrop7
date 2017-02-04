using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundPiece : MonoBehaviour {
    private int x;
    private int y;

    public int X
    {
        get { return x; }
    }

    public int Y
    {
        get { return y; }
    }

    private FlashablePiece flashableComponent;

    public FlashablePiece FlashableComponent
    {
        get { return flashableComponent; }
    }

    public bool IsFlashable()
    {
        return flashableComponent != null;
    }


    private Grid grid;

    public Grid GridRef
    {
        get { return grid; }
    }



    void Awake()
    {
        flashableComponent = GetComponent<FlashablePiece>();
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Init(int _x, int _y, Grid _grid)
    {
        x = _x;
        y = _y;
        grid = _grid;
    }


    public void OnMouseDown()
    {
        // grid.addToColumn(this.X, this.Y);
        grid.handleMouseDown(this.X, this.Y);
    }

    public void OnMouseEnter()
    {
//       grid.highlightColumn(this.X);
        grid.handleMouseEnter(this.X, this.Y);
    }

    public void OnMouseExit()
    {

    }

    public void OnMouseUp()
    {
    }

}
