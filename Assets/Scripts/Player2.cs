using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class Player2 : GroundReactorDynamic
{
//	[SerializeField]private float k_speed = 1f;
//	[SerializeField]private float k_GroundRayLength = 10f; // The length of the ray to check if the ball is grounded.
//	[SerializeField]private float k_groundedDistance = .1f; // The length of the ray to check if the ball is grounded.
//	[SerializeField]private bool m_isGrounded = false;
//	private Rigidbody2D m_Rigidbody;

//	[SerializeField]ReactingBlock block;
//	[SerializeField]private Rigidbody2D m_rigidbody;
//	[SerializeField]private CircleCollider2D m_collider;
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

	[Header("HorizontalMovement")]
	[SerializeField]float _targetPosX;
//	[Space]
//	[SerializeField]Animator _animator;

//	void Start ()
//	{
//		m_rigidbody = GetComponent<Rigidbody2D> ();
//		m_collider = GetComponent<CircleCollider2D> ();
//	}
	
	void Update ()
	{
		base.Update ();
//		float posY = GameControlManager2.Instance.GetPositionFor (gameObject);
//
//		if (m_isGrounded)
//		{
//			transform.position = new Vector3(transform.position.x, posY + m_collider.radius);
//		}

		// Movement
		float h = CrossPlatformInputManager.GetAxis("Horizontal");
		float v = CrossPlatformInputManager.GetAxis("Vertical");
		jump = CrossPlatformInputManager.GetButtonDown("Jump");

		_targetPosX = transform.position.x + (h * 4);
		_targetPosX = Mathf.Clamp (_targetPosX, GameManager.Instance.CameraBounds.min.x, GameManager.Instance.CameraBounds.max.x);
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

		Vector3 targetPos = new Vector3 (_targetPosX, transform.position.y);

		float newMovePower = m_isGrounded ? m_MovePower : m_MovePower * .6f;

		transform.position = Vector3.Lerp (transform.position, targetPos, Time.deltaTime * newMovePower);

		// If on the ground and jump is pressed...
		if (m_isGrounded && jump)
		{
			// ... add force in upwards.
			_animator.SetTrigger ("Jump");
			m_isGrounded = false;
			_rigidbody.velocity = Vector3.zero;
			_rigidbody.AddForce(Vector3.up* m_JumpPower, ForceMode2D.Impulse);
			print ("Bounced Force = " + 		(Vector3.up* m_JumpPower).ToString());
			print ("Bounced Force Magnt = " + 	(Vector3.up* m_JumpPower).magnitude);

			StartCoroutine (WaitForGrounded ());
		}
	}

//	public void Bounced(Vector3 force)
//	{
//		print ("Bounced Force = " + 		force.ToString());
//		print ("Bounced Force Magnt = " + 	force.magnitude);
//
//		if (force.sqrMagnitude < .1f * .1f)
//		{
//			print ("Not bouncing! Bounce too small!");
//			return;
//		}
//
//		if(!m_isGrounded)
//		{
//			print ("Not bouncing! Already bouncing!!");
//			return;
//		}
//		
//		print ("Bouncing!");
//
//		// Min force
//		float minMagn = 4;
//		if(force.sqrMagnitude < minMagn * minMagn)
//		{
//			print ("Increasing Force");
//			force = force.normalized * minMagn;
//		}
//
//		_animator.SetTrigger ("Bounce");
//		m_isGrounded = false;
//		m_rigidbody.velocity = Vector3.zero;
//		m_rigidbody.AddForce (force, ForceMode2D.Impulse);
//
//		StartCoroutine (WaitForGrounded ());
//	}
//
//	IEnumerator WaitForGrounded()
//	{
//		yield return new WaitForSeconds (.2f);
//		print ("Waiting for ground");
//		m_rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
//		while (transform.position.y - m_collider.radius + 0.2f > GameControlManager2.Instance.GetPositionFor (gameObject))
//		{
//			yield return true;
//		}
//
//		Landed ();
//	}
//
//	public void Landed()
//	{
//		print ("Landed!");
//		transform.rotation = Quaternion.identity;
//		_animator.SetTrigger ("Run");
//		m_isGrounded = true;
//		m_rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
//	}

}

