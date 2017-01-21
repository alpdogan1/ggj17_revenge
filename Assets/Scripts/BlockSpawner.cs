using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSpawner : MonoBehaviour {

	[SerializeField]private float blockWidth;
	[SerializeField]private ReactingBlock blockPrefab;
	[SerializeField]List<ReactingBlock> blocks = new List<ReactingBlock>();
	[SerializeField]List<ReactingBlock> blocksPool = new List<ReactingBlock>();


	[SerializeField]private ReactingBlock lastBlock;


	void Start()
	{
//		WatchBlocks ();
//		CreateBlock (transform.position);
		CreateInitialBlocks ();
	}

	void FixedUpdate()
	{
		WatchBlocks ();

		if(lastBlock == null)
		{
			return;
		}

		float dist = transform.position.x - lastBlock.transform.position.x;
//		float maxDist = blockWidth;

		print ("dist = " + dist);
//		print ("maxDist = " + maxDist);

		if(dist > blockWidth * 2)
		{
			float dif = dist - (blockWidth * 2);
			CreateBlock (transform.position - ( Vector3.right * ( blockWidth + dif ) ) );
		}
	}

	void CreateInitialBlocks()
	{
		float cameraWidth = GameManager.Instance.CameraBounds.size.x;

		float extends = (cameraWidth / 2) + 2;
		Vector3 start = Camera.main.transform.position - (extends  * Vector3.right);
		start = new Vector3 (start.x, transform.position.y);
		Vector3 end = Camera.main.transform.position + (extends * Vector3.right );
		end = new Vector3 (end.x, transform.position.y);
		Vector3 current = start;

		while (current.x < end.x)
		{
			CreateBlock (current);
			current += Vector3.right * blockWidth;
		}
	}

	void CreateBlock(Vector3 pos)
	{
		ReactingBlock newBlock;

		// Get pooled if available
		if (blocksPool.Count > 0) {
			
			newBlock = blocksPool [0];
			blocksPool.RemoveAt (0);
		} 
		else
		{
			newBlock = Instantiate (blockPrefab, pos, Quaternion.identity);
		}

		newBlock.transform.position = pos;
		newBlock.ReCache ();
		newBlock.transform.parent = transform;
		blocks.Add (newBlock);
		lastBlock = newBlock;
	}

	void PoolBlock(ReactingBlock block)
	{
		print("Pooling block!");
		blocks.Remove (block);
		blocksPool.Add (block);
	}

	void WatchBlocks()
	{
		ReactingBlock[] blocksDup = blocks.ToArray ();

		foreach (var block in blocksDup) 
		{
			if(block.transform.position.x < GameManager.Instance.CameraBounds.min.x - 5)
			{
				PoolBlock (block);
			}
		}
	}

}
