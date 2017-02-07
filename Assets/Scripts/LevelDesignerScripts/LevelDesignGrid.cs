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
//		GamePiece thePiece = pieces[x,y];

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

    public void saveLevel()
    {
        // get the levelNumber from InputField
        string levelName = LevelNumber.text;
        TextWriter tw = new StreamWriter(Application.persistentDataPath + "/levelInfo."+levelName);
       
        for (int x=0;x<xDim; x++) {
            for (int y=0; y<yDim; y++) {
                if (pieces[x,y].Type == PieceType.EMPTY) {
                    tw.Write(0);
                } else {
                    tw.Write(pieces[x,y].GetColorNumber());
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
        string levelName = LevelNumber.text;
        string astring = null;
        if (File.Exists( Application.persistentDataPath + "/levelInfo."+levelName)) {
            TextReader tr = new StreamReader(Application.persistentDataPath + "/levelInfo."+levelName);
            astring = tr.ReadToEnd();
        }
        if (astring != null) {
            string [] splitstring = astring.Split(',');

            for (int i = 0; i< splitstring.Length-1;i++) {
                ClearPiece(i / 7, i %7, 0);
                if (splitstring[i] == "0") {
                    SpawnNewPiece(i / 7, i%7, PieceType.EMPTY);
                } else {
                    int pieceNum = int.Parse(splitstring[i]);
                    GamePiece aPiece = SpawnNewPiece(i / 7, i % 7, PieceType.NORMAL);
                    aPiece.ColorComponent.SetColor((ColorPiece.ColorType)pieceNum-1);                     
                }
            }


        }
    }
	

}
