using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

	[SerializeField]private float itemWidth = 1;
	[SerializeField]private GameObject[] prefabs;
	[SerializeField]public List<GameObject> activeItems = new List<GameObject>();
	[SerializeField]public List<GameObject> pool = new List<GameObject>();

	[SerializeField]private GameObject lastItem;
	[Header("Random Y")]
	[SerializeField]private bool randomY;
	[SerializeField]private float randomYRange;
	[Header("Random Scale")]
	[SerializeField]private bool randomScale;
	[SerializeField]private float randomScaleRange;


	public bool isActive = false;

	protected virtual void Start()
	{
//		WatchBlocks ();
//		CreateBlock (transform.position);
		CreateInitialBlocks ();
	}

	protected virtual void FixedUpdate()
	{
		if(isActive)
		{
			CheckToSpawn ();
		}

		WatchItems ();
	}

	void CheckToSpawn()
	{
		if(lastItem == null)
		{
			return;
		}

		float dist = transform.position.x - lastItem.transform.position.x;

		if(dist > itemWidth * 2)
		{
			float dif = dist - (itemWidth * 2);
			CreateItem (transform.position - ( Vector3.right * ( itemWidth + dif ) ) );
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
			CreateItem (current);
			current += Vector3.right * itemWidth;
		}
	}

	public void CreateItem(Vector3 pos)
	{
		GameObject newItem;

		// Get pooled if available
		if (pool.Count > 0 && !ForceInstantiate()) {
			
			newItem = pool [0];
			pool.RemoveAt (0);

			newItem.SetActive (true);
			newItem.transform.position = pos;
		} 
		else
		{
			newItem = Instantiate (GetPrefab(), pos, Quaternion.identity);
			newItem.transform.parent = transform;
		}

		if(newItem.GetComponent<GroundReactorStatic>())
		{
			newItem.GetComponent<GroundReactorStatic> ().ReCache ();
		}

		if(newItem.GetComponent<GroundReactorDynamic>())
		{
			newItem.GetComponent<GroundReactorDynamic> ().spawner = this;
		}

		if(randomY)
		{
			float rngRange = Random.Range (0, randomYRange);
			newItem.transform.position += Vector3.up * rngRange;

		}
		if(randomScale)
		{
			float rngScale = Random.Range (-randomScaleRange, randomScaleRange);
			newItem.transform.localScale *= 1+ rngScale;
		}

		activeItems.Add (newItem);
		lastItem = newItem;
	}

	protected virtual GameObject GetPrefab()
	{
		return prefabs [Random.Range (0, prefabs.Length)];
//		return prefab;
	}

	protected virtual bool ForceInstantiate()
	{
		return true;
	}

	void PoolItem(GameObject block)
	{
//		print("Pooling block!");

		if(block.GetComponent<GroundReactorDynamic>())
		{
			block.GetComponent<GroundReactorDynamic> ().Landed ();
		}

		activeItems.Remove (block);
		pool.Add (block);
		block.SetActive (false);
	}

	protected void WatchItems()
	{
		GameObject[] activeItemsDup = activeItems.ToArray ();

		foreach (var block in activeItemsDup) 
		{
			if(block == null)
			{
				activeItems.Remove (block);
			}
			else if(block.transform.position.x < GameManager.Instance.CameraBounds.min.x - itemWidth - 0.1f)
			{
				PoolItem (block);
			}
		}
	}

}
