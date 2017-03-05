using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;
public class Level : MonoBehaviour {

	public enum LevelType {
		SCORELEVEL,
		OBSTACLELEVEL,
		FORMATION
	};

	public LevelDescriptions levels;
	public BoardDescriptions boards;
	
	public bool[] LockedLevels;
	
	public GameObject levelButtonPrefab;


	static public LevelInfo currentLevelInfo = new LevelInfo();
	static public Dictionary<string,BoardInfo> boardMap;
	GameObject[] levelObjectArray;
//	public Grid grid;
	// Use this for initialization
	public int loadLevelsFromResourceFile() {
		string myText = LoadResourceTextfile("levelInfo");
		levels = JsonUtility.FromJson<LevelDescriptions>(myText);

		string myBoard = LoadResourceTextfile("boardInfo");
		boards = JsonUtility.FromJson<BoardDescriptions>(myBoard);
		boardMap = new Dictionary<string, BoardInfo>();
		foreach (BoardInfo aBoard in boards.boardInfo) {
			boardMap[aBoard.BoardName] = aBoard;
		}
		return(levels.levelInfo.Length);
	}
	public int loadLevelsFromSeperateFiles() {
		string[] levelFileNames = Directory.GetFiles(Application.persistentDataPath, "level.*");
		int startloc = levels.levelInfo.Length;
		int additionalLevels = levelFileNames.Length;
		
		System.Array.Resize(ref levels.levelInfo, startloc+additionalLevels);
//		levels.levelInfo = new LevelInfo[levelFileNames.Length];
		int count = 0;
		foreach (string afileName in levelFileNames) {
			TextReader tr = new StreamReader(afileName);
            string astring = tr.ReadToEnd();
			tr.Close();
			levels.levelInfo[startloc+count] = JsonUtility.FromJson<LevelInfo>(astring);
			count++;
		}


		LockedLevels = new bool[levels.levelInfo.Length];
		for (int i=0; i<levels.levelInfo.Length; i++) {
			LockedLevels[i] = false;
		}
		return(additionalLevels);
	}

	void Start () {
		int rlevels = loadLevelsFromResourceFile();
		loadLevelsFromSeperateFiles();	
		levelObjectArray= new GameObject[levels.levelInfo.Length];
		for (int i = 0; i<levels.levelInfo.Length; i++) {
			int y = i % 10;
			int x = i / 10;
			GameObject newObject = (GameObject)Instantiate(levelButtonPrefab, new Vector2((float)x * 330.0f + 220.0f, (float)y * -60.0f + 700.0f), Quaternion.identity);
			//newObject.transform.localScale = new Vector2(0.2f,0.2f);
			newObject.transform.SetParent(this.transform);
			levelObjectArray[i]=newObject;


			GameObject scoreText = newObject.transform.FindChild("Text").gameObject;
			UnityEngine.UI.Button theButton = newObject.GetComponent<UnityEngine.UI.Button>();

			theButton.onClick.AddListener (() => {HandleClick(y);});
        	UnityEngine.UI.Text thetext = scoreText.GetComponent<UnityEngine.UI.Text>();
			if (i>=rlevels) {
				thetext.text = "User Level: Description: " + levels.levelInfo[i].Description;	
			} else {
				thetext.text = "Level: Description: " + levels.levelInfo[i].Description;
			}
			// TODO SHOW WHICH LEVELS ARE LOCKED OR NOT!
 		}
	}
	
	public void HandleClick(int i)
	{
		PlayLevel(i);
	}

	public void PlayLevel(int i) {
	
		currentLevelInfo.DropType = levels.levelInfo[i].DropType;
		currentLevelInfo.DropsPerRound = levels.levelInfo[i].DropsPerRound;
		currentLevelInfo.LevelType = levels.levelInfo[i].LevelType;
		currentLevelInfo.ScorePerRound = levels.levelInfo[i].ScorePerRound;
		currentLevelInfo.TotalDropsPerLevel = levels.levelInfo[i].TotalDropsPerLevel;
		currentLevelInfo.StarScores = levels.levelInfo[i].StarScores;
		currentLevelInfo.Description = levels.levelInfo[i].Description;
		currentLevelInfo.AssociatedBoard = levels.levelInfo[i].AssociatedBoard;
		UnityEngine.SceneManagement.SceneManager.LoadScene("scenes");

	}

	public void toLevelDesigner()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene("LevelDesigner");
	}

	public void quit() {
		Application.Quit();
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
	public int levelNo;
	public string LevelType;
	public string Description;
	public int  DropsPerRound;
	public int ScorePerRound;
	public string DropType;
	public long [] StarScores;
	public int TotalDropsPerLevel;
	public string AssociatedBoard;

}

public class BoardDescriptions
{
	public BoardInfo[] boardInfo;

}

[System.Serializable]
public class BoardInfo {
	public string BoardName;
	public int []boardPieces;
}