using UnityEngine;

public class Level : MonoBehaviour {

	public enum LevelType {
		SCORELEVEL,
		OBSTACLELEVEL,
		FORMATION
	};

	public LevelDescriptions levels;

	
	public GameObject levelButtonPrefab;

static public LevelInfo currentLevelInfo = new LevelInfo();
	GameObject[] leveObjectArray;
//	public Grid grid;
	// Use this for initialization
	void Start () {
		string myText = LoadResourceTextfile("levelInfo");
		levels = JsonUtility.FromJson<LevelDescriptions>(myText);


		leveObjectArray= new GameObject[levels.levelInfo.Length];
		for (int i = 0; i<levels.levelInfo.Length; i++) {
			int y = i % 15;
			int x = i / 15;
			GameObject newObject = (GameObject)Instantiate(levelButtonPrefab, new Vector2((float)x * 10.0f + 350.0f, -20.0f +  (float)y * -30.0f + 450.0f), Quaternion.identity);
			//newObject.transform.localScale = new Vector2(0.2f,0.2f);
			newObject.transform.SetParent(this.transform);
			leveObjectArray[i]=newObject;


			GameObject scoreText = newObject.transform.FindChild("Text").gameObject;
			UnityEngine.UI.Button theButton = newObject.GetComponent<UnityEngine.UI.Button>();

			theButton.onClick.AddListener (() => {HandleClick(y);});
        	UnityEngine.UI.Text thetext = scoreText.GetComponent<UnityEngine.UI.Text>();
			thetext.text = "Level " + i.ToString();
		}
	}
	
	public void HandleClick(int i)
	{
		currentLevelInfo.DropType = levels.levelInfo[i].DropType;
		currentLevelInfo.DropsPerRound = levels.levelInfo[i].DropsPerRound;
		currentLevelInfo.LevelType = levels.levelInfo[i].LevelType;
		currentLevelInfo.ScorePerRound = levels.levelInfo[i].ScorePerRound;

		UnityEngine.SceneManagement.SceneManager.LoadScene("scenes");
		Debug.Log("in handle Click "+i.ToString());
	}

	// Update is called once per frame
	void Update () {
		
	}

	public void startGameWithParameters(string aName)
	{
		Debug.Log("start game with string" + aName);
	}
	public static string LoadResourceTextfile(string path)
	{
        TextAsset targetFile = Resources.Load<TextAsset>(path);
 
        return targetFile.text;
	}
}

[System.Serializable]
public class LevelDescriptions
{
	
	public LevelInfo[] levelInfo;
}

[System.Serializable]
	public class LevelInfo
	{
		public string LevelType;
		public int  DropsPerRound;
		public int ScorePerRound;
		public string DropType;

	}