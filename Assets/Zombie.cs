using UnityEngine;
using System.Collections;

public class Zombie : GroundReactorDynamic
{
	[SerializeField]float speed = .2f;

	protected override void Update ()
	{
		base.Update ();

		transform.position += Vector3.left * (GameManager.Instance.levelSpeed + speed);
	}
//	void Update()
//	{
//		RefreshElevation ();
//
//		transform.position += Vector3.left * (GameManager.Instance.levelSpeed + speed);
//	}

}

