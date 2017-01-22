using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharBit : MonoBehaviour {
	
	public Sprite[] bitGraphics;

	public void ChooseBit(int index)
	{
		GetComponent<SpriteRenderer> ().sprite = bitGraphics [index];
	}

}
