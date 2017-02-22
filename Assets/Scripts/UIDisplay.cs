using UnityEngine;
using UnityEngine.UI;

 public static class TransformDeepChildExtension
 {
     //Breadth-first search
     public static Transform FindDeepChild(this Transform aParent, string aName)
     {
         var result = aParent.Find(aName);
         if (result != null)
             return result;
         foreach(Transform child in aParent)
         {
             result = child.FindDeepChild(aName);
             if (result != null)
                 return result;
         }
         return null;
     }
 }

public class UIDisplay : MonoBehaviour {

	// Use this for initialization
	/***
	UI Display: 
		Drops Left in Level
		Current Score
		Star Count
		Chain Number
		Drop INdicator before Bumping Stage
	***/
	public GameObject NumDropsIndicator;
	public GameObject gameOver;
	public string DropsLeftText {
		get {return dropsLeftText.text;}
		set {
			dropsLeftText.text = value;
		}
	}

	public string CurrentScoreText {
		get {return currentScoreText.text;}
		set {
			currentScoreText.text = value;
		}
	}

	public string ChainNumberText {
		get {return chainNumberText.text;}
		set {
			chainNumberText.text = value;
		}
	}

	public string LevelNoText {
		get {return levelNoText.text;}
		set {
			levelNoText.text = value;
		}
	}
	
	public string GameTypeText {
		get {return gameTypeText.text;}
		set {
			gameTypeText.text = value;
		}
	}
	
	public string GameOverText {
		get {return gameOverText.text;}
		set {
			gameOverText.text = value;
		}
	}
	
	private Text finalScoreText;
	public string FinalScoreText {
		get {return finalScoreText.text;}
		set {
			finalScoreText.text = value;
		}
	}

	private Text starLevelText;
	public string StarLevelText {
		get {return starLevelText.text;}
		set {
			starLevelText.text = value;
		}
	}
	private Text dropsLeftText;
	private Text currentScoreText;
	private Text chainNumberText;
	private Text gameTypeText;

	private Text gameOverText;
	private Text levelNoText;

	private Grid grid;
	private numDropIndicator DropIndicator;

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Init(Grid aGrid)
	{
		grid = aGrid;
	}
	void Awake() {
		GameObject anObject;
		anObject = this.transform.FindDeepChild("DropsLeft").gameObject;
		dropsLeftText = anObject.GetComponent<UnityEngine.UI.Text>();

		DropsLeftText = "";
		anObject = this.transform.FindDeepChild("CurrentScore").gameObject;
		currentScoreText = anObject.GetComponent<UnityEngine.UI.Text>();
		CurrentScoreText = "";


		anObject = this.transform.FindDeepChild("ChainLevel").gameObject;
		chainNumberText = anObject.GetComponent<UnityEngine.UI.Text>();
		ChainNumberText = "";

		anObject = this.transform.FindDeepChild("LevelNo").gameObject;
		levelNoText = anObject.GetComponent<UnityEngine.UI.Text>();
		LevelNoText = "";

		anObject = this.transform.FindDeepChild("GameTypeText").gameObject;
		gameTypeText = anObject.GetComponent<UnityEngine.UI.Text>();
		GameTypeText = "";

		anObject = this.transform.FindDeepChild("GameOverText").gameObject;
		gameOverText = anObject.GetComponent<UnityEngine.UI.Text>();
		GameOverText = "";

		anObject = this.transform.FindDeepChild("FinalScore").gameObject;
		finalScoreText = anObject.GetComponent<UnityEngine.UI.Text>();
		FinalScoreText = "";

		anObject = this.transform.FindDeepChild("StarLevel").gameObject;
		starLevelText = anObject.GetComponent<UnityEngine.UI.Text>();
		StarLevelText = "";

		DropIndicator = NumDropsIndicator.GetComponent<numDropIndicator>();
		            


	}
	
	public void CrossFadeLevelText(float end, float duration) {
		chainNumberText.CrossFadeAlpha(end, duration, true);
	}

	public void SetTotalDrops(int numDrops)
	{
		DropIndicator.SetTotalDropNumber(numDrops);
	}

	public void SetDropCount(int numDrops)
	{
		DropIndicator.SetDropCount(numDrops);

	}

	
	public void showGameOver(bool win){
		GameOver theGameOver =  gameOver.GetComponent<GameOver>();

		FinalScoreText = grid.currentGameStats.currentScore.ToString();
		if (grid.currentGameStats.currentScore < Level.currentLevelInfo.StarScores[0]) {
			StarLevelText = "Stars: 0";
		} else if (grid.currentGameStats.currentScore < Level.currentLevelInfo.StarScores[1]) {
			StarLevelText = "Stars: 1";
		} else if (grid.currentGameStats.currentScore < Level.currentLevelInfo.StarScores[2]) {
			StarLevelText = "Stars: 2";
		} else {
			StarLevelText = "Stars: 3";
		}

		if (win) {
			GameOverText = "You Win!";
		} else {
			GameOverText = "Not this time...";
		}
		if (theGameOver) {
			theGameOver.ShowWin();
		}
	}

}
