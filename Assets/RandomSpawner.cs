using UnityEngine;
using System;
using System.Collections;

public class RandomSpawner : Spawner
{
	[SerializeField]float[] seconds = new float[]{3, 10};


	protected override void Start ()
	{
		WaitAndSpawn ();
	}
	protected override void FixedUpdate ()
	{
		WatchItems ();
	}

	public void WaitAndSpawn()
	{
		StartCoroutine (WaitRandomSeconds ());
	}

	IEnumerator WaitRandomSeconds()
	{
		float randomSeconds = UnityEngine.Random.Range (seconds[0], seconds[1]);
		print ("Random secons = " + randomSeconds);

		yield return new WaitForSeconds (randomSeconds);

		CreateItem (transform.position);
		StartCoroutine (WaitRandomSeconds());
	}



}

