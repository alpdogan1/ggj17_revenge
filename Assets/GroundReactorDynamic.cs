using UnityEngine;
using System.Collections;

public class GroundReactorDynamic : MonoBehaviour
{
	[SerializeField]protected CircleCollider2D _collider;
	[SerializeField]protected Rigidbody2D _rigidbody;
	[SerializeField]protected Animator _animator;
	[Space]
	[SerializeField]protected bool m_isGrounded = false;

	void Start ()
	{
		GameControlManager2.Instance.dynamicReactors.Add (this);
		
		_rigidbody = GetComponent<Rigidbody2D> ();
		_collider = GetComponent<CircleCollider2D> ();
	}

	protected virtual void Update ()
	{
		float posY = GameControlManager2.Instance.GetPositionFor (gameObject);

		if (m_isGrounded) {
			transform.position = new Vector3 (transform.position.x, posY);
		}

//		CheckMinY ();
	}

	protected virtual void FixedUpdate ()
	{
//		CheckMinY ();
	}

	public void CheckMinY()
	{
		float posY = GameControlManager2.Instance.GetPositionFor (gameObject);
		if(transform.position.y < posY)
		{
			transform.position = new Vector3 (transform.position.x, posY);
		}
	}

	public void Bounced(Vector3 force)
	{
		if (force.sqrMagnitude < .1f * .1f)
		{
			print ("Not bouncing! Bounce too small!");
			return;
		}

		if(!m_isGrounded)
		{
			print ("Not bouncing! Already bouncing!!");
			return;
		}

		print ("Bouncing!");
		print ("Bounced Force = " + 		force.ToString());
		print ("Bounced Force Magnt = " + 	force.magnitude);


		// Min force
		float minMagn = 4;
		if(force.sqrMagnitude < minMagn * minMagn)
		{
			print ("Increasing Force");
			force = force.normalized * minMagn;
		}
		if(_animator)
		{
			_animator.SetTrigger ("Bounce");
		}
		//		m_rigidbody.bodyType = RigidbodyType2D.Dynamic;
		m_isGrounded = false;

		_rigidbody.velocity = Vector3.zero;

		_rigidbody.AddForce (force, ForceMode2D.Impulse);

		StartCoroutine (WaitForGrounded ());
	}

	public IEnumerator WaitForGrounded()
	{
		yield return new WaitForSeconds (.2f);
		//		while(!m_isGrounded)
		print ("Waiting for ground");
		_rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;

		while (transform.position.y - 0.1f > GameControlManager2.Instance.GetPositionFor (gameObject))
		{
			if(m_isGrounded)
			{
				yield break;
			}

			yield return true;
		}

		Landed ();
	}

	public void Landed()
	{
		print ("Landed!");
		transform.rotation = Quaternion.identity;
		_animator.SetTrigger ("Run");
		m_isGrounded = true;
		_rigidbody.velocity = Vector3.zero;
		_rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
		//		m_rigidbody.bodyType = RigidbodyType2D.Kinematic;
	}
}

