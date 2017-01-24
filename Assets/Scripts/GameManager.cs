using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
	#region Singleton
	public static GameManager Instance { private set; get; }

	void Awake()
	{
		Instance = this;
	}

	#endregion
	public float levelSpeed = .1f;
	public float cachedLevelSpeed = .1f;
	public float blockCountForKey = 1;
//	public Transform blockContainer;
//	public List<ReactingBlock> blocks;
	public LayerMask groundLayer;
	public Canvas mainCanvas;
	public BlockSpawner blockSpawner;
	public Spawner[] mobSpawner;

	public Player2 player;

	public bool gameEnded = false;

	[Header("THE GAME")]
	public int currentPlayerIndex = 0;
	public float[] playerDistances = new float[]{0,0};
	public float winDistance = 50f;
	public RectTransform[] playerIndicators;

	public Bounds CameraBounds{
		get{
			return GetCameraBounds (Camera.main);
		}
	}

	public GameObject credits;

	void Start()
	{
//		Screen.SetResolution (1920, 1080, true);

		SetCameraSize ();
		cachedLevelSpeed = levelSpeed;
		levelSpeed = 0;

		// Gather blocks
//		ReactingBlock[] blocksArr = blockContainer.GetComponentsInChildren<ReactingBlock> ();
//		blocks = new List<ReactingBlock> (blocksArr);

	}

	void Update()
	{
		RecordDistances ();

		if(Input.GetKeyDown(KeyCode.Escape))
		{
			Application.LoadLevel (0);
		}

		if(Input.GetKeyDown(KeyCode.F1))
		{
			if(credits.activeSelf)
			{
				credits.gameObject.SetActive (false);
			} 
			else
			{
				credits.gameObject.SetActive (true);
				
			}
		}
	}

	public void SetCameraSize()
	{
		float curH = Camera.main.orthographicSize * 2;
		float curW = Camera.main.aspect * curH;

		float expW = GameControlManager2.Instance.keys.Length * blockCountForKey;

		// Calculate expected height
		float expH = expW * curH / curW;

		Camera.main.orthographicSize = expH / 2;
	}

	Bounds GetCameraBounds (Camera cam)
	{

		float screenAspect = (float)Screen.width / (float)Screen.height;
		float cameraHeight = cam.orthographicSize * 2;
		Bounds newBounds = new Bounds(
			cam.transform.position,
			new Vector3(cameraHeight * screenAspect, cameraHeight, 0));
		return newBounds;
	}

	void RecordDistances()
	{
		playerDistances [currentPlayerIndex] += levelSpeed;

		float currentDistance = playerDistances [currentPlayerIndex];
		RectTransform indicator = playerIndicators [currentPlayerIndex];

		float posPercent = currentDistance / winDistance;

		float totalUIDistance = mainCanvas.pixelRect.size.x - 72;
		float currentUIDistance = 36 + (totalUIDistance * posPercent);

		if(posPercent >=1)
		{
//			EndGame ();
			blockSpawner.GameEnded ();
//			player.Win ();
			return;
		}

		indicator.anchoredPosition = new Vector2 (currentUIDistance, indicator.anchoredPosition.y);
	}

	public void StartGame()
	{
		player.gameObject.SetActive (true);
		LeanTween.value (0, cachedLevelSpeed, .2f).setOnUpdate ((val) => {
			levelSpeed = val;
		});

		player.GameStarted ();
		blockSpawner.isActive = true;

		foreach (var item in mobSpawner) 
		{
			item.isActive = true;
		}
	}

	public void EndGame()
	{
		gameEnded = true;
		LeanTween.value (levelSpeed, 0, 2).setEase(LeanTweenType.easeInOutSine).setOnComplete(()=>{
			player.Win();
		})
			.setOnUpdate ((val) => {
			levelSpeed = val;
		});

		blockSpawner.isActive = false;

		foreach (var item in mobSpawner) 
		{
			item.isActive = false;
		}

	}
//	void SpawnInitialBlocks()
//	{
//		while
//	}

}

