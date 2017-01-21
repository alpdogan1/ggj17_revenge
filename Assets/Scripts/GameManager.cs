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

	public float blockCountForKey = 1;
	public Transform blockContainer;
	public List<ReactingBlock> blocks;
	public LayerMask groundLayer;

	public Player2 player;

	void Start()
	{
		SetCameraSize ();

		// Gather blocks
		ReactingBlock[] blocksArr = blockContainer.GetComponentsInChildren<ReactingBlock> ();
		blocks = new List<ReactingBlock> (blocksArr);

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
//
//	void SpawnInitialBlocks()
//	{
//		while
//	}

}

