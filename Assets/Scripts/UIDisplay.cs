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
	
	 
	private Text dropsLeftText;
	private Text currentScoreText;
	private Text chainNumberText;
	private Text levelNoText;

	private numDropIndicator DropIndicator;

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void Awake() {
		GameObject dleft = this.transform.FindDeepChild("DropsLeft").gameObject;
		dropsLeftText = dleft.GetComponent<UnityEngine.UI.Text>();

		DropsLeftText = "";
		GameObject cscore = this.transform.FindDeepChild("CurrentScore").gameObject;
		currentScoreText = cscore.GetComponent<UnityEngine.UI.Text>();
		CurrentScoreText = "";


		GameObject cnumber = this.transform.FindDeepChild("ChainLevel").gameObject;
		chainNumberText = cnumber.GetComponent<UnityEngine.UI.Text>();
		ChainNumberText = "";

		GameObject levelno = this.transform.FindDeepChild("LevelNo").gameObject;
		levelNoText = levelno.GetComponent<UnityEngine.UI.Text>();
		LevelNoText = "";


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

	
	public void showGameOver(){
		GameOver theGameOver =  gameOver.GetComponent<GameOver>();

		if (theGameOver) {
			theGameOver.ShowWin();
		}
	}

}
