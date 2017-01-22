using UnityEngine;
using System.Collections;

public class GroundReactorStatic : MonoBehaviour
{
	[SerializeField]protected Vector3 _cachedLocalPos;

	protected virtual void Start()
	{
		_cachedLocalPos = transform.position;
	}

	public void ReCache()
	{
		_cachedLocalPos = transform.position;
	}

	protected void RefreshElevation()
	{
		float elevation = GameControlManager2.Instance.GetPositionFor (gameObject);

		Vector3 newPos = new Vector3 ( transform.position.x, _cachedLocalPos.y) + (Vector3.up * elevation);

		transform.position = newPos;
	}
}

