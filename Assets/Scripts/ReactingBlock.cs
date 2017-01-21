using UnityEngine;
using System.Collections;

public class ReactingBlock : MonoBehaviour
{
	private Vector3 _cachedLocalPos;
	private bool _isMovingUp = false;
	private Rigidbody2D _rigibody;

	public Vector3 targetPosition;

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
//		if(targetPosition != null)
//		{
			_rigibody.MovePosition (targetPosition);
//		}
//		targetPosition = null;
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

