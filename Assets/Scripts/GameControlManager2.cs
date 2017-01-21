using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControlManager2 : MonoBehaviour {

	#region Singleton
	public static GameControlManager2 Instance { private set; get; }

	void Awake()
	{
		Instance = this;
	}

	#endregion

	public KeyCode[] keys;
	[SerializeField]private KeyPower activeKeyPower = null;
	[SerializeField]private List<KeyPower> livingKeyPowers = new List<KeyPower>();

	[Header("Reaction")]
	public float influenceRadius = 2f;
	public float maxInfluenceElevation = 4f;

	public float threshold = 0.03f;

//	[Header("New Reaction")]
	public float moveUpTime = .2f;
	public LeanTweenType moveUpEasing = LeanTweenType.easeInSine;
	public float moveDownTime = .1f;
	public LeanTweenType moveDownEasing = LeanTweenType.easeInSine;


	void Start()
	{
		StartCoroutine (WatchKeys ());
	}

	IEnumerator WatchKeys()
	{
		KeyCode[] keys = GameControlManager2.Instance.keys;
		KeyCode currentKey = KeyCode.None;

		while(true)
		{
			// Dont check for new key down if active key power exists
			if(activeKeyPower != null)
			{
				yield return true;
				continue;
			}
			// A key was down
//			if(currentKey != KeyCode.None)
//			{
//				// Key released
//				if(Input.GetKeyUp(currentKey))
//				{
//					currentKey = KeyCode.None;
//
////					foreach (var block in GameManager.Instance.blocks)
////					{
////						block.MoveDown ();
////					}
//				}
//				else
//				{
//					yield return true;
//					continue;
//				}
//			}

			// Listen for key down
			for (int i = 0; i < keys.Length; i++) 
			{
				if(Input.GetKeyDown(keys[i]))
				{
					currentKey = keys [i];
					print ("Current key is " + currentKey.ToString ());

//					KeyPower kp = new KeyPower (i);
					KeyPower kp = gameObject.AddComponent<KeyPower> ();
					kp.Init (i);

//					foreach (var lkp in livingKeyPowers) 
//					{
//						if(kp.Index == lkp.Index)
//						{
//							LeanTween.cancel (lkp.gameObject);
//						}
//					}

					livingKeyPowers.Add (kp);

//					foreach (var block in GameManager.Instance.blocks)
//					{
//						if (!block.gameObject.activeSelf)
//							continue;
//
//						float elevation = GetElevationForPositionAndKeyIndex (block.transform.position.x, i);
//						block.MoveUp (elevation);
//					}
				}
			}

			yield return true;
		}
	}

	public void KeyReleased(KeyPower keyPower)
	{
		if(activeKeyPower == keyPower)
		{
			activeKeyPower = null;
		}
	}

	public void KeyPowerDied(KeyPower keyPower)
	{
		livingKeyPowers.Remove (keyPower);
		Destroy (keyPower);
	}

	public float GetPositionFor(GameObject go)
	{
		float totalEl = 0;
//		for (int i = livingKeyPowers.Count - 1; i >= 0; i--) 
//		{
//			KeyPower kp = livingKeyPowers [i];
//
//			float el = kp.GetElevationForPositionX (go.transform.position.x);
//			totalEl += el;
////			el = Mathf.Clamp (el, 0, maxInfluenceElevation);
//
////			return el;
//		}

//		return 0;
		foreach (var kp in livingKeyPowers) 
		{
			float el = kp.GetElevationForPositionX (go.transform.position.x);
			totalEl += el;
		}

		totalEl = Mathf.Clamp (totalEl, 0, maxInfluenceElevation);

		return totalEl;
	}
}
