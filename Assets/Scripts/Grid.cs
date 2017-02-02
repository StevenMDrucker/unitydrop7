using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public struct Tuple
{
    public int x, y;

    public Tuple(int p1, int p2)
    {
        x = p1;
        y = p2;
    }
}


public class Grid : MonoBehaviour {

    public enum PieceType
    {
        EMPTY,
        NORMAL,
        COUNT,
    };

    [System.Serializable]
    public struct PiecePrefab
    {
        public PieceType type;
        public GameObject prefab;
    };

    public int xDim;
    public int yDim;
    public float fillTime;
    public PiecePrefab[] piecePrefabs;
    public GameObject backgroundPrefab;
    public GameObject scorePrefab;

    public GameObject LevelScoreText;

    private GamePiece nextPiece;
    private Dictionary<PieceType, GameObject> piecePrefabDict;

    private GamePiece[,] pieces;
    private BackgroundPiece[,] backgroundPieces;
    private bool isFilling = false;
    private bool isUpdating = false;
    private int chainLevel = 0;
    private long levelScore = 0L;
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

        StartCoroutine(Fill());

        GameObject nPiece = (GameObject)Instantiate(piecePrefabDict[PieceType.NORMAL], GetWorldPosition(3, -1), Quaternion.identity);
        nPiece.transform.parent = transform;
        nextPiece = nPiece.GetComponent<GamePiece>();
        nextPiece.Init(3, -1, this, PieceType.NORMAL);
        nextPiece.ColorComponent.SetColor((ColorPiece.ColorType)Random.Range(0, nextPiece.ColorComponent.NumColors - 1));
    }



    public bool AddBottomRow()
    {
        // check top row
        for (int x = 0; x < xDim; x++)
        {
            GamePiece piece = pieces[x, 0];
            if (piece.Type != PieceType.EMPTY)
            {
                // Game Over
                return (true);
            } else { 
                Destroy(pieces[x, 0].gameObject);
            }
        }
            
        for (int y = 0; y < yDim-1; y++)
        {
            for (int x = 0; x < xDim; x++)
            {                 
                pieces[x, y] = pieces[x, y+1];
                if (pieces[x, y + 1].IsMovable())
                {
                    pieces[x, y + 1].MovableComponent.Move(x, y, 0.2f);
                }
            }
        }
        for (int x = 0; x < xDim; x++)
        {
            GameObject newPiece = (GameObject)Instantiate(piecePrefabDict[PieceType.NORMAL], GetWorldPosition(x, yDim), Quaternion.identity);
            newPiece.transform.parent = transform;

            pieces[x, yDim-1] = newPiece.GetComponent<GamePiece>();
            pieces[x, yDim-1].Init(x, yDim, this, PieceType.NORMAL);
            pieces[x, yDim-1].MovableComponent.Move(x, yDim-1, 0.01f);
            pieces[x, yDim-1].ColorComponent.SetColor(ColorPiece.ColorType.EGG);

        }

        return (false);
    }

    public IEnumerator Fill()
    {
        isFilling = true;
        while (FillStep())
        {
            yield return new WaitForSeconds(fillTime);
        }
        isFilling = false;
    }

    public bool FillStep()
    {
        bool movedPiece = false;

        for (int y = yDim - 2; y >= 0; y--)
        {
            for (int x = 0; x < xDim; x++)
            {
                GamePiece piece = pieces[x, y];

                if (piece.IsMovable())
                {
                    GamePiece pieceBelow = pieces[x, y + 1];

                    if (pieceBelow.Type == PieceType.EMPTY)
                    {
                        Destroy(pieceBelow.gameObject);
                        piece.MovableComponent.Move(x, y + 1, fillTime);
                        pieces[x, y + 1] = piece;
                        SpawnNewPiece(x, y, PieceType.EMPTY);
                        movedPiece = true;
                    }
                }
            }
        }
        
        return movedPiece;
    }

	
	// Update is called once per frame
	void Update () {
		
	}

    public Vector2 GetWorldPosition(int x, int y)
    {
        return new Vector2((transform.position.x - xDim / 2.0f + x) * 1.1f,
            (transform.position.y + yDim / 2.0f - y) * 1.1f);
    }


    public Vector2 GetWorldPosition(float x, float y)
    {
        return new Vector2((transform.position.x - xDim / 2.0f + x) * 1.1f,
            (transform.position.y + yDim / 2.0f - y) * 1.1f);
    }

    public GamePiece SpawnNewPiece(int x, int y, PieceType type)
    {
        GameObject newPiece = (GameObject)Instantiate(piecePrefabDict[type], GetWorldPosition(x, y), Quaternion.identity);
        newPiece.transform.parent = transform;

        pieces[x, y] = newPiece.GetComponent<GamePiece>();
        pieces[x, y].Init(x, y, this, type);

        return pieces[x, y];
    }

    public void highlightBackground(BackgroundPiece aPiece)
    {
        // find which piece this is

        SpriteRenderer aRender = aPiece.GetComponent<SpriteRenderer>();
        Color newColor = new Color(1.0f, 0.0f, 0.0f, 0.5f);
        aRender.color = newColor;
        
    }

    public void highlightBackground(GamePiece aGamePiece)
    {
        // find which piece this is
        BackgroundPiece aPiece = backgroundPieces[aGamePiece.X, aGamePiece.Y];

        SpriteRenderer aRender = aPiece.GetComponent<SpriteRenderer>();
        Color newColor = new Color(1.0f, 0.0f, 0.0f, 0.5f);
        aRender.color = newColor;

    }
    public void highlightRemoved(bool[,] toHighlight)
    {
        for (int x = 0; x < this.xDim; x++)
        {
            for (int y = 0; y < this.yDim; y++)
            {
                if (toHighlight[x,y])
                {
                    FlashPiece(x, y);
                }
                /*
                BackgroundPiece aPiece = backgroundPieces[x, y];

                SpriteRenderer aRender = aPiece.GetComponent<SpriteRenderer>();
                Color newColor = new Color(0.0f, 0.0f, 0.0f, 0.5f);
                if (toHighlight[x,y])
                {
                    newColor = new Color(0.7f, 0.7f, 0.7f, 0.8f);
                } 
                aRender.color = newColor;
                */
            }
        }

    }
    public void highlightColumn(int xcol)
    {
        for (int x = 0; x<this.xDim; x++)
        {
            for (int y=0; y<this.yDim; y++)
            {
                if (x == xcol)
                {
                    HighlightPiece(x, y);
                } 
                else
                {
                    unHighlightPiece(x, y);
                }
            }
        }

        nextPiece.transform.position = GetWorldPosition(xcol, -1);        
    }


    public void addToColumn(int x, int y)
    {
        GamePiece pieceBelow = pieces[x, 0];

        chainLevel = 0;
        if (!isFilling && !isUpdating)
        {
            if (pieceBelow.Type == PieceType.EMPTY)
            {
                // hide preview piece
                nextPiece.gameObject.SetActive(false);
                Destroy(pieceBelow.gameObject);
                GameObject newPiece = (GameObject)Instantiate(piecePrefabDict[PieceType.NORMAL], GetWorldPosition(x, -1), Quaternion.identity);
                newPiece.transform.parent = transform;

                pieces[x, 0] = newPiece.GetComponent<GamePiece>();
                pieces[x, 0].Init(x, -1, this, PieceType.NORMAL);
                pieces[x, 0].MovableComponent.Move(x, 0, fillTime);
                pieces[x, 0].ColorComponent.SetColor(nextPiece.ColorComponent.Color);

                nextPiece.ColorComponent.SetColor((ColorPiece.ColorType)Random.Range(0, nextPiece.ColorComponent.NumColors - 1));
            }
            StartCoroutine(fillAndUpdate(chainLevel));
        }
    }



    public IEnumerator fillAndUpdate(int cl) {
        isUpdating = true;
        yield return Fill();
        //yield return new WaitForSeconds(0.5f);
        yield return updateBoard(++cl);
        isUpdating = false;
    }

    public IEnumerator addRow()
    {
        isUpdating = true;
        yield return (AddBottomRow());
        yield return new WaitForSeconds(0.5f);
        yield return updateBoard(0);
        isUpdating = false;
    }

    private int[,] assignClustersVertical()
    {
        int[,] clusters = new int[xDim, yDim];

        for (int row = 0; row < xDim; row++)
        {
            int col = 0;
            int colstart = 0;
            int curcount = 0;
            bool inCluster = false;
            while (col < 7)
            {
                GamePiece aPiece = pieces[row, col];
                if (aPiece.Type == PieceType.EMPTY)
                {
                    // cell is empy so either you haven't seen a cluster yet or you need to fill in
                    if (!inCluster)
                    {
                        col++;
                        colstart = col;
                        curcount = 0;
                    }
                    else
                    {
                        // you WERE in a cluster and now not, so you need to fill in the previous columns
                        for (int j = colstart; j < col; j++)
                        {
                            clusters[row, j] = curcount;
                        }
                        inCluster = false;
                        colstart = 0;
                        curcount = 0;
                    }
                }
                else
                {
                    // cell is NOT empty so either you were in a cluster and just continue or just enterring new cluster
                    if (!inCluster)
                    {
                        colstart = col;
                        inCluster = true;
                        curcount++;
                        col++;
                    }
                    else
                    {
                        curcount++;
                        col++;
                    }
                }
            }
            if (inCluster)
            {
                // in cluster at end so fill in values
                for (int j = colstart; j < 7; j++)
                {
                    clusters[row, j] = curcount;
                }
                curcount = 0;
                colstart = 0;
                inCluster = false;
            }
        }
        return (clusters);
    }

    private int[,] assignClustersHorizontal()
    {
        int[,] clusters = new int[xDim, yDim];

        for (int col = 0; col < yDim; col++)
        {
            int row = 0;
            int rowstart = 0;
            int curcount = 0;
            bool inCluster = false;
            while (row < 7)
            {
                GamePiece aPiece = pieces[row, col];
                if (aPiece.Type == PieceType.EMPTY)
                {
                    // cell is empy so either you haven't seen a cluster yet or you need to fill in
                    if (!inCluster)
                    {
                        row++;
                        rowstart = row;
                        curcount = 0;
                    }
                    else
                    {
                        // you WERE in a cluster and now not, so you need to fill in the previous columns
                        for (int j = rowstart; j < row; j++)
                        {
                            clusters[j, col] = curcount;
                        }
                        inCluster = false;
                        rowstart = 0;
                        curcount = 0;
                    }
                }
                else
                {
                    // cell is NOT empty so either you were in a cluster and just continue or just enterring new cluster
                    if (!inCluster)
                    {
                        rowstart = row;
                        inCluster = true;
                        curcount++;
                        row++;
                    }
                    else
                    {
                        curcount++;
                        row++;
                    }
                }
            }
            if (inCluster)
            {
                // in cluster at end so fill in values
                for (int j = rowstart; j < 7; j++)
                {
                    clusters[j, col] = curcount;
                }
                curcount = 0;
                rowstart = 0;
                inCluster = false;
            }
        }
        return (clusters);
    }

    private bool[,] flagForRemoval(int[,] clusters)
    {
        bool[,] toRemove = new bool[xDim, yDim];

        for (int row = 0; row < xDim; row++)
        {
            for (int col=0;col < yDim; col++)
            {
                GamePiece aPiece = pieces[row, col];
                if (aPiece.GetColorNumber() == clusters[row, col])
                {
                    toRemove[row,col] = true;
                } else
                {
                    toRemove[row, col] = false;
                }
            }
        }
        return (toRemove);
    }



    private bool[,] flagAllForRemoval(out bool[,] toHighlight)
    {
        bool[,] toRemove = new bool[xDim, yDim];
        toHighlight = new bool[xDim, yDim];
        int[,] horClusters = this.assignClustersHorizontal();
        int[,] verClusters = this.assignClustersVertical();
        bool[,] horFlagged = this.flagForRemoval(horClusters);
        bool[,] verFlagged = this.flagForRemoval(verClusters);


        bool[,] highlightHClusters = identifyHClustersWithRemoval(horFlagged, horClusters);
        bool[,] highlightVClusters = identifyVClustersWithRemoval(verFlagged, verClusters);

        for (int row = 0; row<xDim; row++)
        {
            for (int col=0; col<yDim; col++)
            {
                if (horFlagged[row,col] || verFlagged[row,col])
                {
                    toRemove[row, col] = true;
                } else
                {
                    toRemove[row, col] = false;
                }

                if (highlightHClusters[row,col] || highlightVClusters[row,col])
                {
                    toHighlight[row, col] = true;

                } else
                {
                    toHighlight[row, col] = false;
                }
            }
        }

        return (toRemove);
    }

    private bool[,] identifyVClustersWithRemoval(bool[,] toRemove, int[,] hClusters)
    {
        bool[,] identified = new bool[xDim, yDim];
        for (int row = 0; row < xDim; row++)
        {
            for (int col = 0; col < yDim; col++)
            {
                identified[row, col] = false;
            }
        }

        for (int row = 0; row < xDim; row++)
        {
            for (int col = 0; col < yDim; col++)
            {
                if (toRemove[row, col])
                {
                    int clusterNumber = hClusters[row, col];
                    // move downwards
                    int curcol = col;
                    while (curcol >= 0)
                    {
                        if (hClusters[row, curcol] == clusterNumber)
                        {
                            identified[row, curcol] = true;
                            curcol--;
                        }
                        else
                        {
                            break;
                        }
                    }
                    curcol = col;
                    while (curcol < 7)
                    {
                        if (hClusters[row, curcol] == clusterNumber)
                        {
                            identified[row, curcol] = true;
                            curcol++;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
        }
        return (identified);
    }
    private bool[,] identifyHClustersWithRemoval(bool[,] toRemove, int[,] vClusters)
    {
        bool[,] identified = new bool[xDim, yDim];
        for (int row = 0; row < xDim; row++)
        {
            for (int col = 0; col < yDim; col++)
            {
                identified[row, col] = false;
            }
        }

        for (int row = 0; row < xDim; row++)
        {
            for (int col = 0; col < yDim; col++)
            {
                if (toRemove[row, col])
                {
                    int clusterNumber = vClusters[row, col];
                    // move downwards
                    int currow = row;
                    while (currow >= 0)
                    {
                        if (vClusters[currow, col] == clusterNumber)
                        {
                            identified[currow, col] = true;
                            currow--;
                        }
                        else
                        {
                            break;
                        }
                    }
                    currow = row;
                    while (currow < 7)
                    {
                        if (vClusters[currow, col] == clusterNumber)
                        {
                            identified[currow, col] = true;
                            currow++;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
        }
        return (identified);
    }

    private bool removeFlagged(bool[,] toRemove, int chainlevel)
    {
        bool removed = false;
        for (int row = 0; row < xDim; row++)
        {
            for (int col=0;col<yDim; col++)
            {
                // GamePiece aPiece = pieces[row, col];
                if (toRemove[row,col])
                {
                    //                    Destroy(aPiece.gameObject);
                    bool didRemove = ClearPiece(row, col, chainlevel);
                    removed = true;
                }
            }
        }
        return (removed);
    }
    public bool FlashPiece(int row, int col)
    {
        if (backgroundPieces[row, col].IsFlashable() && !backgroundPieces[row, col].FlashableComponent.IsBeingFlashed)
        {
            backgroundPieces[row, col].FlashableComponent.Flash();
            return (true);
        }
        return (false);

    }


    public void HighlightPiece(int row, int col)
    {

        GameObject highl = backgroundPieces[row, col].gameObject.transform.FindChild("highlightsquare").gameObject;
        SpriteRenderer aRender = highl.GetComponent<SpriteRenderer>();
        Color newColor = new Color(1.0f, 1.0f, 1.0f, 0.3f);
        aRender.color = newColor; 
    }


    public void unHighlightPiece(int row, int col)
    {
        //        backgroundPieces[row, col].gameObject.transform.FindChild("highlightsquare").gameObject.SetActive(false);
        GameObject highl = backgroundPieces[row, col].gameObject.transform.FindChild("highlightsquare").gameObject;
        SpriteRenderer aRender = highl.GetComponent<SpriteRenderer>();
        Color newColor = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        aRender.color = newColor;

    }

    private long computeScore(int chainlevel)
    {
        long score = 3L * chainlevel * chainlevel;
        return (score);
    }
    public void AddScoreText(int x, int y, int colnumber, int chainLevel)
    {
        Color[] colorList = { Color.green, Color.yellow, new Color(1.0f, 0.6f, 0.0f), Color.red, new Color(0.5f, 0.0f, 0.5f), Color.cyan, Color.blue };
        GameObject newObject = (GameObject)Instantiate(scorePrefab, GetWorldPosition((float)x+1.5f, (float)y-.5f), Quaternion.identity);
        GameObject scoreText = newObject.transform.FindChild("Text").gameObject;
        UnityEngine.UI.Text thetext = scoreText.GetComponent<UnityEngine.UI.Text>();
        long localScore = computeScore(chainLevel);
        levelScore += localScore;    
        thetext.text = localScore.ToString();
        thetext.color = colorList[colnumber - 1];
        UnityEngine.UI.Text leveltext = LevelScoreText.GetComponent<UnityEngine.UI.Text>();
        leveltext.text = levelScore.ToString();

        newObject.transform.parent = transform;
    }

    public bool ClearPiece(int row, int col, int chainLevel)
    {
        if (pieces[row,col].IsClearable() && !pieces[row,col].ClearableComponent.IsBeingCleared)
        {
            AddScoreText(row, col, pieces[row, col].GetColorNumber(), chainLevel);
            pieces[row, col].ClearableComponent.Clear();

            SpawnNewPiece(row, col, PieceType.EMPTY);
            return(true);
        }
        return (false);
    }
    public IEnumerator updateBoard(int cl)
    {
        bool[,] toHighlight;
      
        bool didRemove = true;
                
        while (didRemove)
        {
            while (isFilling)
            {
                yield return new WaitForSeconds(0.01f);
            }
            bool[,] removeData = flagAllForRemoval(out toHighlight);
            highlightRemoved(toHighlight);
            popNeighbors(removeData);
            didRemove = removeFlagged(removeData,cl);
            if (didRemove)
            {
                yield return new WaitForSeconds(0.5f);
            }
            cl++;
            StartCoroutine(Fill());
        }
        nextPiece.gameObject.SetActive(true);
    }

    private List<Tuple> getNeighborList(int x, int y)
    {
        List<Tuple> retList = new List<Tuple>();
        if ( (x-1)>=0)
        {
            retList.Add(new Tuple(x - 1, y));
        }
        if ( (x+1) <7)
        {
            retList.Add(new Tuple(x + 1, y));
        }
        if ((y-1)>=0)
        {
            retList.Add(new Tuple(x, y-1));
        }
        if ((y+1)<7)
        {
            retList.Add(new Tuple(x, y + 1));
        }
        return (retList);
    }
        
    private void popNeighbors(bool[,] toRemove)
    {
        for (int r = 0; r<xDim; r++)
        {
            for (int c=0; c<yDim; c++)
            {
                if (toRemove[r,c])
                {
                    List<Tuple> neighbors = getNeighborList(r, c);
                    foreach(Tuple aNeighbor in neighbors)
                    {
                        GamePiece neighborPiece = pieces[aNeighbor.x, aNeighbor.y];
                        if (neighborPiece.Type != PieceType.EMPTY)
                        {
                            if (neighborPiece.ColorComponent.Color == ColorPiece.ColorType.EGG)
                            {
                                neighborPiece.ColorComponent.SetColor(ColorPiece.ColorType.EGGCRACKED);
                            }
                            else if (neighborPiece.ColorComponent.Color == ColorPiece.ColorType.EGGCRACKED)
                            {
                                neighborPiece.ColorComponent.SetColor((ColorPiece.ColorType)Random.Range(0, nextPiece.ColorComponent.NumColors - 2));
                            }
                        }
                    }
                }
            }
        }
    }    
    
}
