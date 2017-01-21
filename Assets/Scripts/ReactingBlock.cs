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
//		targetPosition = transform.position;
	}

	public void ReCache()
	{
		_cachedLocalPos =transform.localPosition;
	}

	void Update()
	{
		RefreshElevation ();

		// Move
		transform.position += Vector3.left * GameManager.Instance.levelSpeed;
	}

	private void RefreshElevation()
	{
		float elevation = GameControlManager2.Instance.GetPositionFor (gameObject);

		Vector3 newPos = new Vector3 ( transform.position.x, _cachedLocalPos.y) + (Vector3.up * elevation);

		transform.position = newPos;
	}

}

