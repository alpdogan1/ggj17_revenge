using UnityEngine;
using System.Collections;

public class GroundReactorStatic : MonoBehaviour
{
	private Vector3 _cachedLocalPos;

	protected virtual void Start()
	{
		_cachedLocalPos = transform.localPosition;
	}

	public void ReCache()
	{
		_cachedLocalPos =transform.localPosition;
	}

	protected void RefreshElevation()
	{
		float elevation = GameControlManager2.Instance.GetPositionFor (gameObject);

		Vector3 newPos = new Vector3 ( transform.position.x, _cachedLocalPos.y) + (Vector3.up * elevation);

		transform.position = newPos;
	}
}

