using UnityEngine;
using System.Collections;

public class ReactingBlock : MonoBehaviour
{

	private Vector3 _cachedLocalPos;
	private float _currentElevation = 0;
	private float _targetElevation = 0;

	private bool _isMoving = false;

	private bool _isMovingUp = false;

	void Start()
	{
		_cachedLocalPos = transform.localPosition;
	}

	void Update()
	{
//		PosLoop ();
	}


	void PosLoop()
	{
//		_targetElevation = GameControlManager.Instance.GetElevationForPositionAndKeyIndex (transform.position.x);

//		print ("_targetElevation = " + _targetElevation);

		// Move towards target elevation
		if(Mathf.Abs( _currentElevation - _targetElevation) >= GameControlManager.Instance.threshold)
		{
			_isMoving = true;
			float speed = _targetElevation > _currentElevation ? GameControlManager.Instance.upSpeed : GameControlManager.Instance.downSpeed;
			_currentElevation = Mathf.Lerp (_currentElevation, _targetElevation, Time.deltaTime * speed);
			transform.localPosition = _cachedLocalPos + (Vector3.up * _currentElevation);
		}
		// Stop Moving if movinf
		else if(_isMoving)
		{
			// Is at peak
			if(_targetElevation > _currentElevation)
			{
//				print ("Peak!");
			}

			_isMoving = false;
			_currentElevation = _targetElevation;
			transform.localPosition = _cachedLocalPos + (Vector3.up * _currentElevation);
		}

	}

	public void MoveUp(float elevation)
	{
		//		print ("Moving Up, Elevation: " + elevation);
		_isMovingUp = true;
		LeanTween.cancel (gameObject);
		LeanTween.moveLocal (gameObject, _cachedLocalPos + (Vector3.up * elevation), GameControlManager.Instance.moveUpTime)
			.setEase (GameControlManager.Instance.moveUpEasing)
			.setOnComplete (() => {

				_isMovingUp = false;
				Bounce ();

		});
	}

	public void MoveDown()
	{
		if(_isMovingUp)
		{
			Bounce ();
		}
		//		print ("Moving Down: ");
		_isMovingUp = false;
		LeanTween.cancel (gameObject);
		LeanTween.moveLocal (gameObject, _cachedLocalPos, GameControlManager.Instance.moveUpTime)
			.setEase (GameControlManager.Instance.moveDownEasing)
			.setOnComplete (() => {



			});
	}

	private void Bounce()
	{
		
	}

	private void Peaked()
	{
		print ("Peaked! Block: " + name);
	}
}

