﻿using UnityEngine;
using System.Collections;

public class GamePiece : MonoBehaviour {

    private int x;
    private int y;

    public int X
    {
        get { return x; }
        set {
            if (IsMovable()) {
                x = value;
            }
        }
    }

    public int Y
    {
        get { return y; }
        set {
            if (IsMovable()) {
                y = value;
            }
        }
    }

    private Grid.PieceType type;

    public Grid.PieceType Type
    {
        get { return type; }
    }

    private Grid grid;

    public Grid GridRef
    {
        get { return grid; }
    }

    private MovablePiece movableComponent;

    public MovablePiece MovableComponent
    {
        get { return movableComponent; }
    }

    private ColorPiece colorComponent;

    public ColorPiece ColorComponent
    {
        get { return colorComponent; }
    }

    public int GetColorNumber()
    {
        int retColor = -1;
        if (IsColored())
        {
            retColor = (int)(colorComponent.Color)+1;
        }

        return (retColor);
    }

    private ClearablePiece clearableComponent;
    public ClearablePiece ClearableComponent
    {
        get { return clearableComponent; }
    }
	void Awake()
	{
		movableComponent = GetComponent<MovablePiece> ();
		colorComponent = GetComponent<ColorPiece> ();
        clearableComponent = GetComponent<ClearablePiece>();
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Init(int _x, int _y, Grid _grid, Grid.PieceType _type)
	{
		x = _x;
		y = _y;
		grid = _grid;
		type = _type;
	}

	public bool IsMovable()
	{
		return movableComponent != null;
	}

	public bool IsColored()
	{
		return colorComponent != null;
	}

    public bool IsClearable()
    {
        return clearableComponent != null;
    }
    
    public void OnMouseDown()
    {
        
        grid.handleMouseDown(this.X, this.Y);
    }

    public void OnMouseEnter()
    {
        grid.handleMouseEnter(this.X, this.Y);
        //grid.highlightColumn(this.X);
    }

    public void OnMouseExit()
    {

    }

    public void OnMouseUp()
    {
    }

}
