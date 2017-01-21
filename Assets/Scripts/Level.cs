using UnityEngine;
using System.Collections;

public class Level : MonoBehaviour
{
	private ReactingBlock[] _blocks;
	public float speed = .04f;

	void Start()
	{
		_blocks = GetComponentsInChildren<ReactingBlock> ();
	}

	void Update()
	{
		foreach (ReactingBlock block in _blocks) 
		{
//			child.GetComponent<Rigidbody2D> ().MovePosition (transform.position + Vector3.left * .04f);
//			Vector2 targetPos =
			block.targetPosition += Vector3.left * speed;
		}

//		transform.position += Vector3.left * .04f;
	}
}

