using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BGController : MonoBehaviour
{
	public Camera gameCamera;
	public float parallax_value = 1.5f;
	private Vector3 startingPos;

	public GameObject bgSprite;
	private Vector3 length;
	
	private void Start()
	{
		startingPos = transform.position;
		length = GetComponentInChildren<SpriteRenderer>().bounds.size;

	}

	// Update is called once per frame
	void Update()
    {
		Vector3 relative_pos = gameCamera.transform.position * parallax_value;
		Vector3 dist = gameCamera.transform.position - relative_pos;

		if (dist.x > startingPos.x + length.x)
		{
			startingPos.x += length.x;
		}
		if (dist.x < startingPos.x - length.x)
		{
			startingPos.x -= length.x;
		}

		relative_pos.z = startingPos.z;
		transform.position = startingPos + relative_pos;
	}
}
