using UnityEngine;
using System.Collections;

public class Zombie : GroundReactorDynamic
{
	[SerializeField]float speed = .2f;
	[SerializeField]float randomRange = .4f;
	[SerializeField]float height = 1f;
	[SerializeField]GameObject blowPrefab;
	[SerializeField]SpriteRenderer[] spriteRenderers;

	void Awake()
	{
		float rng = Random.Range (speed - randomRange, speed + randomRange);
		speed = rng;
		spriteRenderers = GetComponentsInChildren<SpriteRenderer> ();
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
		transform.position += Vector3.left * (speed + GameManager.Instance.levelSpeed);
//		_rigidbody.AddForce (Vector2.left * speed);
	}

	void OnCollisionEnter2D(Collision2D coll)
	{
		if(coll.transform.GetComponent<Player2>())
		{
			if (coll.transform.position.y > transform.position.y + height || GameManager.Instance.gameEnded) 
			{
				GameControlManager2.Instance.dynamicReactors.Remove (this);

				coll.transform.GetComponent<Player2>().KilledMob ();

				Die ();
//				Destroy (gameObject);
			} 
			else
			{
				coll.transform.GetComponent<Player2> ().LoseLife ();
			}

		}
	}

	void Die()
	{
		if(spawner != true)
		{
			spawner.activeItems.Remove (gameObject);
			spawner.pool.Remove (gameObject);
		}
		_collider.isTrigger = true;

		GameObject blow = Instantiate (blowPrefab, transform.position + (Vector3.up * height / 2), Quaternion.identity) as GameObject;

		blow.transform.parent = transform;

		Animator blowAnim = blow.GetComponent<Animator> ();
		Utility.Instance.WaitTillAnimationTime (blowAnim, .14f, 0, () => {

//			Destroy 
			foreach (SpriteRenderer renderer in spriteRenderers) {
				renderer.color = new Color(0,0,0,0);
			}

		});

		Utility.Instance.WaitTillAnimationTime (blowAnim, 1f, 0, () => {

//			Destroy (blow);
			Destroy(gameObject);

		});
	}
}

