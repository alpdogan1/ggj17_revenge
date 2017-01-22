using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour {

	[SerializeField]float speed = .2f;
	[SerializeField]float randomRange = .1f;
	[SerializeField]float height = 1f;

	void Awake()
	{
		float rng = Random.Range (speed - randomRange, speed + randomRange);
		speed = rng;
	}

	void Update()
	{
		transform.position += Vector3.left * speed;
	}

}
