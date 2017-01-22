using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEndBlock : MonoBehaviour {

	bool triggered = false;

	void Update () {
		
		if(GameManager.Instance.player.transform.position.x >= transform.position.x && !triggered)
		{
			triggered = true;

			GameManager.Instance.EndGame ();
		}
	}
}
