using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunIndicator : MonoBehaviour {

//	public Sprite filledHearth;
//	public Sprite emptyHearth;

	public GameObject graphics;
//	public SpriteRenderer[] hearts;

	public void Show()
	{
		graphics.SetActive (true);

		Invoke ("Hide", .3f);
	}

	public void Hide()
	{
		graphics.SetActive (false);

	}
}
