using UnityEngine;
using System.Collections;

public class ReactingBlock : MonoBehaviour
{
	private Vector3 _cachedLocalPos;
	private bool _isMovingUp = false;
	private Rigidbody2D _rigibody;
	public float NormalElevation{
		get{
			return _cachedLocalPos.y + transform.parent.position.y;
		}
	}

	public Vector3 targetPosition;

	private Vector3 m_lastPosition;

	void Start()
	{
		_cachedLocalPos = transform.localPosition;
		_rigibody = GetComponent<Rigidbody2D> ();
		targetPosition = transform.position;
	}

	void Update()
	{
		RefreshElevation ();
	}

	void FixedUpdate()
	{
//		_rigibody.MovePosition (targetPosition + transform.parent.position);
		transform.position = targetPosition + transform.parent.position;
//	
//		if (transform.position.y < m_lastPosition)
//		{
//			Bounce ();
//		}
	}

	private void Bounce()
	{
		
	}

	private void RefreshElevation()
	{
		float elevation = GameControlManager2.Instance.GetPositionFor (gameObject);

//		Vector3 currentOrTargetPos = targetPosition == null ? transform.position : (Vector3)targetPosition;

		Vector3 newPos = new Vector3 ( targetPosition.x, _cachedLocalPos.y) + (Vector3.up * elevation);
//		transform.position = newPos;
//		_rigibody.MovePosition (newPos);
		targetPosition = newPos;
	}

}

