using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ColorPiece : MonoBehaviour {

	public enum ColorType
	{
        ONE,
        TWO,
        THREE,
        FOUR,
        FIVE,
        SIX,
        SEVEN,
        EGG,
        EGGCRACKED,
        COUNT
    };

	[System.Serializable]
	public struct ColorSprite
	{
		public ColorType color;
		public Sprite sprite;
	};

	public ColorSprite[] colorSprites;

	private ColorType color;

	public ColorType Color
	{
		get { return color; }
		set { SetColor (value); }
	}

	public int NumColors
	{
		get { return colorSprites.Length; }
	}

	private ColorType hiddenColor;
	public ColorType HiddenColor{
		set {hiddenColor = value;}
		get {return hiddenColor;}
	}

	private SpriteRenderer sprite;
	private Dictionary<ColorType, Sprite> colorSpriteDict;

	void Awake()
	{
		sprite = transform.Find ("piece").GetComponent<SpriteRenderer> ();

		colorSpriteDict = new Dictionary<ColorType, Sprite> ();

		for (int i = 0; i < colorSprites.Length; i++) {
			if (!colorSpriteDict.ContainsKey (colorSprites [i].color)) {
				colorSpriteDict.Add (colorSprites [i].color, colorSprites [i].sprite);
			}
		}
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SetColor(ColorType newColor)
	{
		color = newColor;

		if (colorSpriteDict.ContainsKey (newColor)) {
			sprite.sprite = colorSpriteDict [newColor];
		}

/*
		if (newColor == ColorType.EGG) {
			hiddenColor = (ColorPiece.ColorType)Random.Range(0, NumColors - 2);
		} else if (newColor == ColorType.EGGCRACKED) {
			hiddenColor = (ColorPiece.ColorType)Random.Range(0, NumColors - 2);
		}
*/	
	}
}
