using UnityEngine;

public class numDropIndicator : MonoBehaviour {

	private int levelDrops;

    public int LevelDrops
    {
        get { return levelDrops; }
    }
	public GameObject dropPrefab;
	private GameObject [] dropArray;

	// Use this for initialization
	void Start () {
		//SetTotalDropNumber(levelDrops);
		SetTotalDropNumber(levelDrops);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetTotalDropNumber(int numDrops)
	{
		levelDrops = numDrops;
		dropArray = new GameObject[numDrops];
		for (int i = 0; i<numDrops; i++) {
			int x = i % 15;
			int y = i / 15;
			GameObject newObject = (GameObject)Instantiate(dropPrefab, new Vector2((float)x * 0.2f - 8.0f, -2.0f - (0.25f * y)), Quaternion.identity);
			newObject.transform.localScale = new Vector2(0.2f,0.2f);
			newObject.transform.SetParent(this.transform);
			dropArray[i]=newObject;
		}
	}

	public void SetDropCount(int curCount)
	{
		for (int i = 0; i<levelDrops; i++) {
			GameObject anObject = dropArray[i];
			SpriteRenderer aRender = anObject.GetComponent<SpriteRenderer>();
			if (i<=curCount) {
				aRender.color = new Color(aRender.color.r, aRender.color.g, aRender.color.b, 1.0f);
			} else {
				aRender.color = new Color(aRender.color.r, aRender.color.g, aRender.color.b, 0.5f);
			}
		}
	}

	
}
