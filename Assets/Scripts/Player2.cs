using UnityEngine;
using System.Collections;

public class Player2 : MonoBehaviour
{
	[SerializeField]private float k_speed = 1f;
	[SerializeField]private float k_GroundRayLength = 10f; // The length of the ray to check if the ball is grounded.
	[SerializeField]private float k_groundedDistance = .1f; // The length of the ray to check if the ball is grounded.
	[SerializeField]private bool m_isGrounded = false;
//	private Rigidbody2D m_Rigidbody;

	[SerializeField]ReactingBlock block;
	[SerializeField]private Rigidbody2D m_rigidbody;
	[SerializeField]private CircleCollider2D m_collider;
	[SerializeField]private Vector3 m_lastPosition;
	[SerializeField]private float m_groundDetDelay = .1f;
	[SerializeField]private float m_curGroundDetDelay = 0;
	[SerializeField]private bool m_groundDetDelayed = false;

	void Start ()
	{
		m_rigidbody = GetComponent<Rigidbody2D> ();
		m_collider = GetComponent<CircleCollider2D> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		BounceDelayLoop ();
//		Move ();

//		if(!m_groundDetDelayed)
//			CheckGrounded ();


//		if(m_isGrounded)
//		{
//			SetGroundedVerticalPos ();
//		}

//		float posY = GameControlManager2.Instance.GetPositionFor (gameObject);
//
//		if (transform.position.y < posY && m_isGrounded)
//		{
//			print ("Setting player Y " + posY);
//			transform.position = new Vector3(transform.position.x, posY + m_collider.radius);
		//		}

		float posY = GameControlManager2.Instance.GetPositionFor (gameObject);

//		if (transform.position.y - m_collider.radius < posY && m_isGrounded)
		if (m_isGrounded)
		{
//			print ("Setting player Y " + posY + m_collider.radius);
//			m_rigidbody.MovePosition(new Vector3(transform.position.x, posY + m_collider.radius));
			transform.position = new Vector3(transform.position.x, posY + m_collider.radius);
		}
	}

	void FixedUpdate()
	{

	}

	public void Move()
	{
		transform.position += (Vector3.right * k_speed);

//		if (transform.position.y < m_lastPosition.y - 0.01)
//		{
//			Bounced (Vector3.up * 2);
//		}

		m_lastPosition = transform.position;
	}

	public void SetGroundedVerticalPos()
	{
		float posY = GameControlManager2.Instance.GetPositionFor (gameObject);
		transform.position = new Vector3 (transform.position.x, block.NormalElevation + posY + m_collider.radius);
	}

	private void BounceDelayLoop()
	{
		if(m_groundDetDelayed)
		{
			m_curGroundDetDelay -= Time.deltaTime;

			if(m_curGroundDetDelay <= 0)
			{
				m_groundDetDelayed = false;
				print ("Delay ended!");
			}
		}
	}

	private void CheckGrounded()
	{
		RaycastHit2D hit = Physics2D.Raycast (transform.position, -Vector3.up, k_GroundRayLength, GameManager.Instance.groundLayer);
		RaycastHit2D boxHit = Physics2D.BoxCast (transform.position, Vector2.one * m_collider.radius * 2, 0, -Vector3.up, k_GroundRayLength, GameManager.Instance.groundLayer);
//		RaycastHit2D boxHit = Physics2D.BoxCast(transform.position, m_collider.ra)


		if(hit)
		{
			Debug.DrawRay (transform.position, -Vector3.up * k_GroundRayLength);

			block = hit.transform.GetComponent<ReactingBlock> ();

			if(boxHit.distance <= k_groundedDistance + m_collider.radius)
			{
				if(!m_isGrounded)
				{
					print ("Landed!");
					m_rigidbody.bodyType = RigidbodyType2D.Static;
				}

				m_isGrounded = true;
			}
//			return true;
		}
		else
		{

			block = null;
			m_isGrounded = false;
//			return false;
		}
	}

	public void Bounced(Vector3 force)
	{
		print ("Bounced Force = " + 		force.ToString());
		print ("Bounced Force Magnt = " + 	force.magnitude);

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
		m_groundDetDelayed = true;
		m_curGroundDetDelay = m_groundDetDelay;

		m_rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
//		m_rigidbody.bodyType = RigidbodyType2D.Dynamic;
		m_isGrounded = false;
		m_rigidbody.AddForce (force, ForceMode2D.Impulse);

		StartCoroutine (WaitForGrounded ());
	}

	IEnumerator WaitForGrounded()
	{
//		while(!m_isGrounded)
		print ("Waiting for ground");
		while (transform.position.y - m_collider.radius + 0.2f > GameControlManager2.Instance.GetPositionFor (gameObject))
		{
			yield return true;
		}

		Landed ();
	}

	public void Landed()
	{
		print ("Landed!");
		m_isGrounded = true;
		m_rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
//		m_rigidbody.bodyType = RigidbodyType2D.Kinematic;
	}


}

