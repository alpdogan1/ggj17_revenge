using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OurCamera : MonoBehaviour {

	[SerializeField]Vector3 _targetPos;
	[SerializeField]float _lerpSpeed = 4;

	void Update () 
	{


		float offset = Camera.main.orthographicSize * .66f;
		_targetPos = new Vector3( GameManager.Instance.player.transform.position.x
			, GameManager.Instance.player.transform.position.y + offset
//			, transform.position.y
			, transform.position.z);

		transform.position = Vector3.Lerp (transform.position, _targetPos, Time.deltaTime * _lerpSpeed);
	}
}
