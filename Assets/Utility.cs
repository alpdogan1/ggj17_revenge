using UnityEngine;
using System.Collections;
using System;

public class Utility : MonoBehaviour
{
	#region Singleton
	public static Utility Instance { private set; get; }

	void Awake()
	{
		Instance = this;
	}

	#endregion

	public void WaitTillAnimationTime(Animator animation, float time, int layer, Action callback)
	{
		StartCoroutine (WaitTillAnimationTimeRoutine (animation, time, layer, callback));
	}

	private IEnumerator WaitTillAnimationTimeRoutine(Animator anim, float time, int layer, Action callback)
	{
		while(anim != null && anim.GetCurrentAnimatorStateInfo(layer).normalizedTime < time)
		{
			//			print (anim.GetCurrentAnimatorStateInfo (0).normalizedTime);
			yield return false;
		}
		callback ();
	}
}

public static class Vector2Extension {

	public static Vector2 Rotate(this Vector2 v, float degrees) {
		float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
		float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

		float tx = v.x;
		float ty = v.y;
		v.x = (cos * tx) - (sin * ty);
		v.y = (sin * tx) + (cos * ty);
		return v;
	}
}
