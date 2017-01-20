using UnityEngine;
using System.Collections;

public class GameControlManager : MonoBehaviour
{
	#region Singleton
	public static GameControlManager Instance { private set; get; }

	void Awake()
	{
		Instance = this;
	}

	#endregion

	public KeyCode[] keys;

//	public int CurrentDownKeyIndex{
//		get{
//			return _currentDownKeyIndex;
//		}
//	}


	public float CurrentDownKeyPos{
		get{
			float curH = Camera.main.orthographicSize * 2;
			float curW = Camera.main.aspect * curH;

			float pos = -(curW / 2) + (_currentDownKeyIndex * GameManager.Instance.blockCountForKey) - 0.5f;
//			pos *= GameManager.Instance.blockCountForKey;
//			print ("CurrentDownKeyPos = " + pos);
			return pos;
		}
	}

	public bool IsKeyDown
	{
		get {
			return _isKeyDown;
		}
	}

	[SerializeField]
	private bool _isKeyDown;
	[SerializeField]
	private int _currentDownKeyIndex;
	[SerializeField]
	private KeyCode _lastKeyDown;
	[SerializeField]
	private int _lastKeyDownIndex;

	[Header("Reaction")]
	public float influenceRadius = 2f;
	public float maxInfluenceElevation = 4f;

	public float threshold = 0.03f;
	public float upSpeed = 1f;
	public float downSpeed = 1f;

	[Header("New Reaction")]
	public float moveUpTime = .2f;
	public LeanTweenType moveUpEasing = LeanTweenType.easeInSine;
	public float moveDownTime = .1f;
	public LeanTweenType moveDownEasing = LeanTweenType.easeInSine;

	void Start()
	{
		StartCoroutine (WatchKeys ());
	}

	void Update()
	{
//		_currentDownKeyIndex = GetDownKeyIndex ();
//		_lastKeyDownIndex = _currentDownKeyIndex;
//		_lastKeyDown = keys [_currentDownKeyIndex];
//		print ("Current key index = " + _currentDownKeyIndex);
	}

	public float GetElevationForPositionAndKeyIndex(float posX, int keyIndex)
	{

//		bool 	isAnyKeyDown = GameControlManager.Instance.IsKeyDown;
		//		float 	keyDownPos = GameControlManager.Instance.CurrentDownKeyPos;
		float 	curH = Camera.main.orthographicSize * 2;
		float 	curW = Camera.main.aspect * curH;

		float 	keyDownPos = -(curW / 2) + (keyIndex * GameManager.Instance.blockCountForKey) - 0.5f;
		float 	dif = Mathf.Abs (keyDownPos - posX);

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
			_targetElevation = impactPercent * maxInfluenceElevation;
		}
		return _targetElevation; 
	}

	int GetDownKeyIndex()
	{
		// If last down key is still down return that key index
		if(Input.GetKey(_lastKeyDown))
		{
//			_isKeyDown = true;
			return _lastKeyDownIndex;
		}

		// Find key index
		for (int i = 0; i < keys.Length; i++) 
		{
			if(Input.GetKey(keys[i]))
			{
				_isKeyDown = true;
				return i;
			}
		}

//		print ("No key is down!");
		_isKeyDown = false;
		return 0;
	}

	int GetDownKey()
	{
		// If last down key is still down return that key index
		if(Input.GetKey(_lastKeyDown))
		{
			//			_isKeyDown = true;
			return _lastKeyDownIndex;
		}

		// Find key index
		for (int i = 0; i < keys.Length; i++) 
		{
			if(Input.GetKeyDown(keys[i]))
			{
				_isKeyDown = true;
				return i;
			}
		}

		//		print ("No key is down!");
		_isKeyDown = false;
		return 0;
	}

	IEnumerator WatchKeys()
	{
		KeyCode currentKey = KeyCode.None;

		while(true)
		{
			// A key was down
			if(currentKey != KeyCode.None)
			{
				// Key released
				if(Input.GetKeyUp(currentKey))
				{
					currentKey = KeyCode.None;

					foreach (var block in GameManager.Instance.blocks)
					{
						block.MoveDown ();
					}

				}
				else
				{
					yield return true;
					continue;
				}
			}

			// Listen for key down
			for (int i = 0; i < keys.Length; i++) 
			{
				if(Input.GetKeyDown(keys[i]))
				{
					currentKey = keys [i];
//					print ("Current key is " + currentKey.ToString ());


					foreach (var block in GameManager.Instance.blocks)
					{
						float elevation = GetElevationForPositionAndKeyIndex (block.transform.position.x, i);
						block.MoveUp (elevation);
					}
				}
			}

			yield return true;
		}
	}

}

