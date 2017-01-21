using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class Player2 : MonoBehaviour
{
//	[SerializeField]private float k_speed = 1f;
//	[SerializeField]private float k_GroundRayLength = 10f; // The length of the ray to check if the ball is grounded.
//	[SerializeField]private float k_groundedDistance = .1f; // The length of the ray to check if the ball is grounded.
	[SerializeField]private bool m_isGrounded = false;
//	private Rigidbody2D m_Rigidbody;

//	[SerializeField]ReactingBlock block;
	[SerializeField]private Rigidbody2D m_rigidbody;
	[SerializeField]private CircleCollider2D m_collider;
//	[SerializeField]private Vector3 m_lastPosition;
//	[SerializeField]private float m_groundDetDelay = .1f;
//	[SerializeField]private float m_curGroundDetDelay = 0;
//	[SerializeField]private bool m_groundDetDelayed = false;

	[Header("Movement")]
	[SerializeField] private float m_MovePower = 5; // The force added to the ball to move it.
	[SerializeField] private float m_JumpPower = 2;

	[Header ("Constraints")]
	[SerializeField]RigidbodyConstraints2D normalConstraints = RigidbodyConstraints2D.FreezeAll;
	[SerializeField]RigidbodyConstraints2D airConstraints = RigidbodyConstraints2D.FreezeAll;
	void Start ()
	{
		m_rigidbody = GetComponent<Rigidbody2D> ();
		m_collider = GetComponent<CircleCollider2D> ();
	}
	
	void Update ()
	{
		float posY = GameControlManager2.Instance.GetPositionFor (gameObject);

		if (m_isGrounded)
		{
			transform.position = new Vector3(transform.position.x, posY + m_collider.radius);
		}


		float h = CrossPlatformInputManager.GetAxis("Horizontal");
		float v = CrossPlatformInputManager.GetAxis("Vertical");
		jump = CrossPlatformInputManager.GetButtonDown("Jump");

		// calculate move direction
		//		if (cam != null)
		//		{
		//			// calculate camera relative direction to move:
		//			camForward = Vector3.Scale(cam.forward, new Vector3(1, 0, 1)).normalized;
		//			move = (v*camForward + h*cam.right).normalized;
		//		}
		//		else
		//		{
		// we use world-relative directions in the case of no main camera
		move = (v*Vector3.forward + h*Vector3.right).normalized;
	}

	private Vector3 move;
	private bool jump; 
	private void FixedUpdate()
	{

		// Call the Move function of the ball controller
		Move(move, jump);
		jump = false;
	}


	public void Move(Vector3 moveDirection, bool jump)
	{
		// If using torque to rotate the ball...
		//		if (m_UseTorque)
		//		{
		//			// ... add torque around the axis defined by the move direction.
		//			m_Rigidbody.AddTorque(new Vector3(moveDirection.z, 0, -moveDirection.x)*m_MovePower);
		//		}
		//		else
		//		{
		// Otherwise add force in the move direction.
		m_rigidbody.AddForce(moveDirection*m_MovePower);
		//		}

		// If on the ground and jump is pressed...
		if (m_isGrounded && jump)
		{
			// ... add force in upwards.
			m_isGrounded = false;
			StartCoroutine (WaitForGrounded ());
			m_rigidbody.AddForce(Vector3.up* m_JumpPower, ForceMode2D.Impulse);
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

//		m_rigidbody.bodyType = RigidbodyType2D.Dynamic;
		m_isGrounded = false;
		m_rigidbody.AddForce (force, ForceMode2D.Impulse);

		StartCoroutine (WaitForGrounded ());
	}

	IEnumerator WaitForGrounded()
	{
//		while(!m_isGrounded)
		print ("Waiting for ground");
		m_rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
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
		m_rigidbody.constraints = RigidbodyConstraints2D.FreezePositionY;
//		m_rigidbody.bodyType = RigidbodyType2D.Kinematic;
	}


}

