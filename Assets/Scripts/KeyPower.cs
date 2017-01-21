using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class KeyPower : MonoBehaviour
{
	[SerializeField]KeyCode keyCode;
	public int Index{
		get{
			return index;
		}
	}
	[SerializeField]int index = 0;
	[SerializeField]float power = 0;
	/// <summary>
	/// Moved all the way up.
	/// </summary>
	bool isSatisfied = false;

	public void Init(int index)
	{
		print ("New key Power!");

		this.index = index;
		this.keyCode = GameControlManager2.Instance.keys[index];

		LeanTween.value (power, 1, GameControlManager2.Instance.moveUpTime)
			.setEase (GameControlManager2.Instance.moveUpEasing)
			.setOnComplete (() => {
				//					GameControlManager2.Instance.KeyPowerDied(this);
				// TODO: Bounce!
			}).setOnUpdate((value)=>{
				power = value;
			});

		StartCoroutine (ListenForRelease () );
	}

	IEnumerator ListenForRelease()
	{
		//			bool cont = true;
		while(!Input.GetKeyUp(keyCode)) 
		{
			yield return true;
		}

		print ("Key Power Released, " + keyCode.ToString());
		Released ();
	}

	public float GetElevationForPositionX(float posX)
	{

		//		bool 	isAnyKeyDown = GameControlManager.Instance.IsKeyDown;
		//		float 	keyDownPos = GameControlManager.Instance.CurrentDownKeyPos;
		float 	curH = Camera.main.orthographicSize * 2;
		float 	curW = Camera.main.aspect * curH;

		float 	keyDownPos = -(curW / 2) + (index * GameManager.Instance.blockCountForKey) - 0.5f;
		float 	dif = Mathf.Abs (keyDownPos - posX);

		float 	influenceRadius = GameControlManager2.Instance.influenceRadius;
		float 	impactPercent = 1 - (dif / influenceRadius);

		//		print ("keyDownPos = " + keyDownPos);
		//		print ("posX = " + posX);
		//		print ("dif = " + dif);
		//		print ("impactPercent = " + impactPercent);

		float _targetElevation = 0;

		// Getting impacted
		//		if(isAnyKeyDown && impactPercent <= 1 && impactPercent > 0)
		if(impactPercent <= 1 && impactPercent > 0)
		{
			float maxInfluenceElevation = GameControlManager2.Instance.maxInfluenceElevation;
			_targetElevation = impactPercent * maxInfluenceElevation;
		}
		return _targetElevation * power; 
	} 

	public void Released()
	{
		if(!isSatisfied)
		{
			// TODO: Bounce
		}

		GameControlManager2.Instance.KeyReleased(this);

		LeanTween.value (power, 0, GameControlManager2.Instance.moveDownTime)
			.setEase (GameControlManager2.Instance.moveDownEasing)
			.setOnComplete (() => {
				GameControlManager2.Instance.KeyPowerDied(this);
				// TODO: Might need clean up
			}).setOnUpdate((value)=>{
				power = value;
			});
	}
}