using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelDesignGrid : Grid {
  
  

//    private GamePiece[,] pieces;
//	private BackgroundPiece[,] backgroundPieces;

	private int currentPiece =0;

	public GamePiece previewPiece;

	// Use this for initialization
  void Start()
    {
        piecePrefabDict = new Dictionary<PieceType, GameObject>();

        for (int i = 0; i < piecePrefabs.Length; i++)
        {
            if (!piecePrefabDict.ContainsKey(piecePrefabs[i].type))
            {
                piecePrefabDict.Add(piecePrefabs[i].type, piecePrefabs[i].prefab);
            }
        }

        backgroundPieces = new BackgroundPiece[xDim, yDim];
        for (int x = 0; x < xDim; x++)
        {
            for (int y = 0; y < yDim; y++)
            {
                GameObject newObject = (GameObject)Instantiate(backgroundPrefab, GetWorldPosition(x, y), Quaternion.identity);
                backgroundPieces[x, y] =  newObject.GetComponent<BackgroundPiece>();
                backgroundPieces[x, y].Init(x, y, this);
                backgroundPieces[x, y].transform.parent = transform;
                /*
                SpriteRenderer aRender = backgroundPieces[x, y].GetComponent<SpriteRenderer>();
                Color newColor = new Color(0.0f, 0.0f, 0.0f, 1.0f);
                aRender.color = newColor;*/
            }
        }

        pieces = new GamePiece[xDim, yDim];
        for (int x = 0; x < xDim; x++)
        {
            for (int y = 0; y < yDim; y++)
            {
                SpawnNewPiece(x, y, PieceType.EMPTY);
            }
        }
        
        
  

    }


    public new void HighlightPiece(int row, int col)
    {
    }
    public new void  unHighlightPiece(int row, int col)
    {
    }
    public new void highlightColumn(int xcol)
    {     
    }

	public override void handleMouseDown(int x, int y)
    {
		GamePiece thePiece = pieces[x,y];

		if (currentPiece == 0) {
			ClearPiece(x,y,0);
		} else {
			ClearPiece(x,y,0);
			GameObject newPiece = (GameObject)Instantiate(piecePrefabDict[PieceType.NORMAL], GetWorldPosition(x, y), Quaternion.identity);
        	newPiece.transform.parent = transform;
			pieces[x, y] = newPiece.GetComponent<GamePiece>();
			pieces[x, y].Init(x, y, this, PieceType.NORMAL);
			pieces[x, y].ColorComponent.SetColor((ColorPiece.ColorType)currentPiece-1);
		}

    }

    public override void handleMouseEnter(int x, int y)
    {
      
    }
	public new GamePiece  SpawnNewPiece(int x, int y, PieceType type)
    {
        GameObject newPiece = (GameObject)Instantiate(piecePrefabDict[type], GetWorldPosition(x, y), Quaternion.identity);
        newPiece.transform.parent = transform;

        pieces[x, y] = newPiece.GetComponent<GamePiece>();
        pieces[x, y].Init(x, y, this, type);

        return pieces[x, y];
    }
	
	public void SetCurrentPiece(int curNo)
	{
		this.currentPiece = curNo;
		if (currentPiece == 0) {
			previewPiece.gameObject.SetActive(false);
		} else {
			previewPiece.gameObject.SetActive(true);
			previewPiece.ColorComponent.SetColor((ColorPiece.ColorType)currentPiece-1);
		}
	}
	// Update is called once per frame
	void Update () {
		
	}


	

}
