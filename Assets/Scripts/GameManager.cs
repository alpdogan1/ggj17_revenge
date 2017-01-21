using UnityEngine;
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
	public float blockCountForKey = 1;
//	public Transform blockContainer;
//	public List<ReactingBlock> blocks;
	public LayerMask groundLayer;

	public Player2 player;

	public Bounds CameraBounds{
		get{
			return GetCameraBounds (Camera.main);
		}
	}


	void Start()
	{
		SetCameraSize ();

		// Gather blocks
//		ReactingBlock[] blocksArr = blockContainer.GetComponentsInChildren<ReactingBlock> ();
//		blocks = new List<ReactingBlock> (blocksArr);

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

//
//	void SpawnInitialBlocks()
//	{
//		while
//	}

}

