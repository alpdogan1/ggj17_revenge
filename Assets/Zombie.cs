using UnityEngine;
using System.Collections;

public class Zombie : GroundReactorDynamic
{
	[SerializeField]float speed = .2f;
	[SerializeField]float randomRange = .4f;

	void Awake()
	{
		float rng = Random.Range (speed - randomRange, speed + randomRange);
		speed = rng;
	}

	protected override void FixedUpdate ()
	{
		CheckMinY ();
		base.FixedUpdate ();
		float posY = GameControlManager2.Instance.GetPositionFor (gameObject);

		if(transform.position.y < posY)
		{
			transform.position = new Vector3 (transform.position.x, posY);
		}
	}

	protected override void Update ()
	{
		CheckMinY ();
		base.Update ();
		transform.position += Vector3.left * speed;
//		_rigidbody.AddForce (Vector2.left * speed);
	}

	void OnCollisionEnter2D(Collision2D coll)
	{
		if(coll.transform.GetComponent<Player2>())
		{
			coll.transform.GetComponent<Player2> ().LoseLife ();
		}
	}

}

