using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour
{
	[SerializeField] private float m_MovePower = 5; // The force added to the ball to move it.
//	[SerializeField] private bool m_UseTorque = true; // Whether or not to use torque to move the ball.
//	[SerializeField] private float m_MaxAngularVelocity = 25; // The maximum velocity the ball can rotate at.
	[SerializeField] private float m_JumpPower = 2; // The force added to the ball when it jumps.

	private const float k_GroundRayLength = 1f; // The length of the ray to check if the ball is grounded.
	private Rigidbody2D m_Rigidbody;
	[SerializeField] private bool m_isGrounded = false;


	private void Start()
	{
		m_Rigidbody = GetComponent<Rigidbody2D>();
		// Set the maximum angular velocity.
//		GetComponent<Rigidbody>().maxAngularVelocity = m_MaxAngularVelocity;
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
		m_Rigidbody.AddForce(moveDirection*m_MovePower);
//		}

		// If on the ground and jump is pressed...
		if (m_isGrounded && jump)
		{
			// ... add force in upwards.
			m_Rigidbody.AddForce(Vector3.up* m_JumpPower, ForceMode2D.Impulse);
		}
	}

//	private Ball ball; // Reference to the ball controller.

	private Vector3 move;
//	// the world-relative desired move direction, calculated from the camForward and user input.
//
//	private Transform cam; // A reference to the main camera in the scenes transform
//	private Vector3 camForward; // The current forward direction of the camera
	private bool jump; // whether the jump button is currently pressed


	private void Awake()
	{
		// Set up the reference.
//		ball = GetComponent<Ball>();


		// get the transform of the main camera
//		if (Camera.main != null)
//		{
//			cam = Camera.main.transform;
//		}
//		else
//		{
//			Debug.LogWarning(
//				"Warning: no main camera found. Ball needs a Camera tagged \"MainCamera\", for camera-relative controls.");
//			// we use world-relative controls in this case, which may not be what the user wants, but hey, we warned them!
//		}
	}


	private void Update()
	{
		// Get the axis and jump input.

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
//		}
	}


	private void FixedUpdate()
	{
		RaycastHit2D hit = Physics2D.Raycast (transform.position, -Vector3.up, k_GroundRayLength, GameManager.Instance.groundLayer);
		if(hit)
		{
			
			Debug.DrawRay (transform.position, -Vector3.up * k_GroundRayLength);
			m_isGrounded = true;
		}
		else
		{
			m_isGrounded = false;
		}

		// Call the Move function of the ball controller
		Move(move, jump);
		jump = false;
	}

//}
}
