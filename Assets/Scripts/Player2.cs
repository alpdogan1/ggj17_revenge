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

	public void KilledMob()
	{
		_rigidbody.velocity = Vector3.zero;
		_rigidbody.AddForce (Vector3.up * 10, ForceMode2D.Impulse);
	}

	public void LoseLife()
	{
		lives -= 1;

		ProCamera2DShake.Instance.Shake ();

		if (lives <= 0) {

			Die ();
			
		} else
		{
			StartCoroutine (TouchDelay ());
		}

		_collider.isTrigger = true;


	}

	IEnumerator TouchDelay()
	{
		_animator.SetTrigger ("DelayTouch");
		_animator.SetTrigger ("Bounce");
//		_collider.isTrigger = true;

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
		_rigidbody.velocity = Vector3.zero;
		_rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;

		Bounds camBounds = GameManager.Instance.CameraBounds;
		Vector3 dieTarget = new Vector3 (transform.position.x - .3f,camBounds.min.y - 2f);

		LeanTween.move (gameObject, dieTarget, dieDuration).setEase(dieEasingCurve)
			.setOnComplete(()=>{
				StartCoroutine(Swap());
//				LeanTween.move (gameObject, GameManager.Instance.CameraBounds.min + (Vector3.down * 2), 1f);
		});
	}

	IEnumerator Swap()
	{
		
		if(GameManager.Instance.currentPlayerIndex == 0)
		{
			GameManager.Instance.currentPlayerIndex = 1;
		}
		else
		{
			GameManager.Instance.currentPlayerIndex = 0;
		}

		lives = 5;
		m_isGrounded = false;

		yield return new WaitForSeconds (2f);

		// Throw Char
		Vector3 firstRespawnPos = new Vector3(GameManager.Instance.CameraBounds.min.x, GameManager.Instance.CameraBounds.min.y + 4, 0);
		transform.position = firstRespawnPos;

		_collider.isTrigger = true;
		_rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
		_rigidbody.velocity = Vector3.zero;
		_rigidbody.AddForce (Vector3.right * 10, ForceMode2D.Impulse);

		yield return new WaitForSeconds (.1f);

		// Stop Charactedr phase
		float cachedTS = Time.timeScale;

		// Stop Level
		float cachedLS = GameManager.Instance.levelSpeed;
		GameManager.Instance.levelSpeed = 0;

		// Stop Char
		_rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;

		// Wait for key
		while(!CrossPlatformInputManager.GetButtonDown("Jump"))
		{
			yield return true;
		}

		_rigidbody.velocity = Vector3.zero;
		_rigidbody.AddForce (Vector3.right * 7, ForceMode2D.Impulse);
		_rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;

		GameManager.Instance.levelSpeed = cachedLS;
//		Time.timeScale = cachedTS;

		this.enabled = true;
		isDead = false;
		StartCoroutine (WaitForGrounded ());

		_animator.SetTrigger ("DelayTouch");
		yield return new WaitForSeconds (5f);
		_animator.SetTrigger ("DelayTouch");
		_collider.isTrigger = false;
	}

}

