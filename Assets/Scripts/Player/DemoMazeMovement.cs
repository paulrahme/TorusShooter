using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoMazeMovement : MonoBehaviour
{
	[SerializeField] float moveSpeed = 1.0f;

	void Update()
	{
		RaycastHit hitInfo;
		if (!Physics.Raycast(transform.position, transform.forward, out hitInfo, 0.5f))
			transform.position += transform.forward * (moveSpeed * Time.deltaTime);
	}
}
