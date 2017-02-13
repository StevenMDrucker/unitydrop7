using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
public class LevelDesignGrid : Grid {
  
  

//    private GamePiece[,] pieces;
//	private BackgroundPiece[,] backgroundPieces;

	private int currentPiece =0;

	public GamePiece previewPiece;
    public InputField LevelNumber;
    public InputField boardName;
    public InputField LevelType;
    public InputField Description;
    public InputField DropsPerRound;
    public InputField ScorePerRound;
    public InputField StarScores;
    public InputField TotalDropsPerLevel;
    public InputField AssociatedBoard;

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

	public override void handleMouseDown(int x, int y)
    {
		GamePiece thePiece = pieces[x,y];
        int colorno = 0; 
        if (thePiece.Type == PieceType.NORMAL) {
            colorno = thePiece.GetColorNumber();
        }

        ClearPiece(x,y,0);
		if (currentPiece != 0) {
			// for not empty pieces, do this:
            ClearPiece(x,y,0);
			GameObject newPiece = (GameObject)Instantiate(piecePrefabDict[PieceType.NORMAL], GetWorldPosition(x, y), Quaternion.identity);
        	newPiece.transform.parent = transform;
			pieces[x, y] = newPiece.GetComponent<GamePiece>();
			pieces[x, y].Init(x, y, this, PieceType.NORMAL);
			pieces[x, y].ColorComponent.SetColor((ColorPiece.ColorType)currentPiece-1);
            if (colorno != 0 && colorno != 8 && colorno != 9) {
                if (currentPiece == 8 || currentPiece == 9) {
                    pieces[x,y].ColorComponent.HiddenColor = (ColorPiece.ColorType)colorno-1;
                }
            } else {
                pieces[x,y].ColorComponent.HiddenColor = ((ColorPiece.ColorType)Random.Range(0, pieces[x,y].ColorComponent.NumColors - 2));
            }
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

    public void save()
    {
        saveLevel();
        saveBoard();
    }

    public void saveLevel()
    {
        LevelInfo aLevel = new LevelInfo();
        aLevel.AssociatedBoard = AssociatedBoard.text;
        aLevel.Description = Description.text;
        aLevel.DropsPerRound = int.Parse( DropsPerRound.text);
        aLevel.levelNo = int.Parse(LevelNumber.text);
        aLevel.LevelType = LevelType.text;
        aLevel.DropType = "";
        aLevel.StarScores = new long[3];
        aLevel.StarScores[0] = 100L;
        aLevel.StarScores[1] = 200L;
        aLevel.StarScores[2] = 300L;
//        aLevel.StarScores = StarScores.text;
        aLevel.ScorePerRound = int.Parse(ScorePerRound.text);
        string jsonLevel = JsonUtility.ToJson(aLevel);
        TextWriter tw = new StreamWriter(Application.persistentDataPath + "/level."+aLevel.levelNo.ToString());
        tw.Write(jsonLevel);
        tw.Flush();
        tw.Close();
        
    }

    public void playLevel()
    {
        LevelInfo aLevel = new LevelInfo();
        aLevel.AssociatedBoard = AssociatedBoard.text;
        aLevel.Description = Description.text;
    
        aLevel.DropsPerRound = int.Parse( DropsPerRound.text);
        aLevel.levelNo = int.Parse(LevelNumber.text);
        aLevel.LevelType = LevelType.text;
        aLevel.StarScores = new long[3];
        aLevel.StarScores[0] = 100L;
        aLevel.StarScores[1] = 200L;
        aLevel.StarScores[2] = 300L;

    	Level.currentLevelInfo.DropType = aLevel.DropType;
		Level.currentLevelInfo.DropsPerRound = aLevel.DropsPerRound;
		Level.currentLevelInfo.LevelType = aLevel.LevelType;
		Level.currentLevelInfo.ScorePerRound = aLevel.ScorePerRound;
		Level.currentLevelInfo.TotalDropsPerLevel = aLevel.TotalDropsPerLevel;
		Level.currentLevelInfo.StarScores = aLevel.StarScores;
		Level.currentLevelInfo.Description = aLevel.Description;
		Level.currentLevelInfo.AssociatedBoard = aLevel.AssociatedBoard;
		UnityEngine.SceneManagement.SceneManager.LoadScene("scenes");
    }
    public void saveBoard()
    {
        // get the levelNumber from InputField
        string boardN = boardName.text;
        TextWriter tw = new StreamWriter(Application.persistentDataPath + "/board."+boardN);
       
        for (int x=0;x<xDim; x++) {
            for (int y=0; y<yDim; y++) {
                if (pieces[x,y].Type == PieceType.EMPTY) {
                    tw.Write(0);
                } else {
                    int colno = pieces[x,y].GetColorNumber();
                    if (colno == 8) {
                        tw.Write(100 + (int)(pieces[x,y].ColorComponent.HiddenColor+1));
                    } else if (colno == 9) {
                        tw.Write(200 + (int)(pieces[x,y].ColorComponent.HiddenColor+1));
                    } else {
                        tw.Write(colno);
                    }
                }
                if ((x < xDim) || (y < yDim)) {
                    tw.Write(",");
                }
            }
        }
        tw.Flush();
        tw.Close();

    }

    public void loadLevel()
    { 


        string boardN = boardName.text;
        this.load(boardN);
    }
	

}
