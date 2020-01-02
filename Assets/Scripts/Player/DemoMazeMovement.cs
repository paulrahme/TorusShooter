using UnityEngine;

public class DemoMazeMovement : MonoBehaviour
{
	[SerializeField] float moveSpeed = 1.0f;

	/// <summary> Called once per frame </summary>
	void Update()
	{
		RaycastHit hitInfo;
		if (!Physics.Raycast(transform.position, transform.forward, out hitInfo, 0.5f))
			transform.position += transform.forward * (moveSpeed * Time.deltaTime);
	}
}
