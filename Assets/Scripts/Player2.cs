using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;
using Com.LuisPedroFonseca.ProCamera2D;

public class Player2 : GroundReactorDynamic
{

	[SerializeField]private bool isDead = false;
	[SerializeField]private float dieDuration = .4f;
	[SerializeField]private AnimationCurve dieEasingCurve;

	public int lives = 3;
	[Header("Movement")]
	[SerializeField] private float m_MovePower = 5; // The force added to the ball to move it.
	[SerializeField] private float m_JumpPower = 2;

	[Header("HorizontalMovement")]
	[SerializeField]float _targetPosX;
	
	protected override void FixedUpdate ()
	{
		base.FixedUpdate ();
		if(!isDead)
			CheckMinY ();

		// Movement
		float h = CrossPlatformInputManager.GetAxis("Horizontal");
//		float v = CrossPlatformInputManager.GetAxis("Vertical");
		jump = CrossPlatformInputManager.GetButtonDown("Jump");

		_targetPosX = transform.position.x + (h * 4);
		_targetPosX = Mathf.Clamp (_targetPosX, GameManager.Instance.CameraBounds.min.x, GameManager.Instance.CameraBounds.max.x);
	}

	private Vector3 move;
	private bool jump; 
	protected override void Update()
	{
		if(!isDead)
			CheckMinY ();
		
		base.Update();

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

		float minX = GameManager.Instance.CameraBounds.min.x;
		float maxX = GameManager.Instance.CameraBounds.max.x;

		if(transform.position.x < minX)
		{
			transform.position = new Vector3( minX, transform.position.y);
		}
		else if(transform.position.x > maxX)
		{
			transform.position = new Vector3( maxX, transform.position.y);
		}
	}

	public void LoseLife()
	{
		lives -= 1;
		StartCoroutine (TouchDelay ());
		ProCamera2DShake.Instance.Shake ();

		if(lives <= 0)
		{
			Die ();

		}

	}

	IEnumerator TouchDelay()
	{
		_animator.SetTrigger ("DelayTouch");
		_animator.SetTrigger ("Bounce");
		_collider.isTrigger = true;

		yield return new WaitForSeconds (2);

		_collider.isTrigger = false;
		_animator.SetTrigger ("DelayTouch");
	}

	public void Die()
	{
		isDead = true;
		_animator.SetTrigger ("Bounce");
		m_isGrounded = false;
		this.enabled = false;

		Bounds camBounds = GameManager.Instance.CameraBounds;
		Vector3 dieTarget = new Vector3 (transform.position.x - .3f,camBounds.min.y - 2f);

		LeanTween.move (gameObject, dieTarget, dieDuration).setEase(dieEasingCurve)
			.setOnComplete(()=>{
//				LeanTween.move (gameObject, GameManager.Instance.CameraBounds.min + (Vector3.down * 2), 1f);
		});
	}


}

