using UnityEngine;
using System.Collections;

public class BlockSpawner : Spawner
{
	public GameObject gameEndBlockPrefab;
	bool gameEnded = false;
	bool gameEndSpawned = false;

	public void GameEnded()
	{
		gameEnded = true;
	}

	protected override GameObject GetPrefab ()
	{
//		return base.GetPrefab ();
		if(gameEnded && !gameEndSpawned)
		{
			gameEndSpawned = true;
			return gameEndBlockPrefab;
		}
		else
		{
			return base.GetPrefab ();
		}
	}

	protected override bool ForceInstantiate ()
	{
		return (gameEnded && !gameEndSpawned);
	}
}

