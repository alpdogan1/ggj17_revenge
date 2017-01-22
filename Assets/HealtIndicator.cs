using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealtIndicator : MonoBehaviour {

	public Sprite filledHearth;
	public Sprite emptyHearth;

	public GameObject graphics;
	public SpriteRenderer[] hearts;

	public void Show(int lives)
	{
		graphics.SetActive (true);
		CancelInvoke ();
		Invoke ("Hide", 1f);

		for (int i = 0; i < hearts.Length; i++) 
		{
			if(i +1 <= lives)
			{
				hearts [i].sprite = filledHearth;
			}
			else 
			{
				hearts [i].sprite = emptyHearth;
			}
		}
	}

	public void Hide()
	{
		graphics.SetActive (false);

	}
}
