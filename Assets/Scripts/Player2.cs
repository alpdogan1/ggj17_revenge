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

	public CharBit[] bits;
	public HealtIndicator healthIndicator;
	public StunIndicator stunIndicator;

	void Start()
	{
		bits = GetComponentsInChildren<CharBit> ();
	}

	protected override void FixedUpdate ()
	{

		base.FixedUpdate ();
		if(!isDead)
			CheckMinY ();

		if (GameManager.Instance.gameEnded)
			return;
		
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

	public void GameStarted()
	{
		_animator.SetTrigger ("Run");
	}

	public void KilledMob()
	{
		_rigidbody.velocity = Vector3.zero;
		_rigidbody.AddForce (Vector3.up * 12, ForceMode2D.Impulse);
	}

	public void LoseLife()
	{
		if (GameManager.Instance.gameEnded)
			return;

		lives -= 1;

		healthIndicator.Show (lives);

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

		GameManager.Instance.levelSpeed = 0;

		LeanTween.move (gameObject, dieTarget, dieDuration).setEase(dieEasingCurve)
			.setOnComplete(()=>{
				StartCoroutine(Swap());
//				LeanTween.move (gameObject, GameManager.Instance.CameraBounds.min + (Vector3.down * 2), 1f);
		});
	}

	IEnumerator Swap()
	{
		GameManager.Instance.playerIndicators [GameManager.Instance.currentPlayerIndex].transform.GetChild (0).gameObject.SetActive (false);

		if(GameManager.Instance.currentPlayerIndex == 0)
		{
			GameManager.Instance.currentPlayerIndex = 1;
		}
		else
		{
			GameManager.Instance.currentPlayerIndex = 0;
		}

		foreach (var item in bits) {
			item.ChooseBit (GameManager.Instance.currentPlayerIndex);
		}

		GameManager.Instance.playerIndicators [GameManager.Instance.currentPlayerIndex].transform.GetChild (0).gameObject.SetActive (true);

		lives = 2;
		m_isGrounded = false;

		// Delay before respawn
		yield return new WaitForSeconds (.5f);

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
//		float cachedLS = GameManager.Instance.levelSpeed;
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

//		GameManager.Instance.levelSpeed = cachedLS;
		GameManager.Instance.levelSpeed = GameManager.Instance.cachedLevelSpeed;
//		Time.timeScale = cachedTS;

		this.enabled = true;
		isDead = false;
		StartCoroutine (WaitForGrounded ());

		_animator.SetTrigger ("DelayTouch");
		yield return new WaitForSeconds (1.8f);
		_animator.SetTrigger ("DelayTouch");
		_collider.isTrigger = false;
	}

	public void Win()
	{
		print ("triggered win");
		_animator.SetTrigger ("Win");
	}

	protected override void AfterBounced ()
	{
		stunIndicator.Show ();
	}
}

