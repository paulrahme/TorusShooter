using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Collidable : MonoBehaviour
{
	RaycastHit hitInfo;
	Vector3 lastPosition;

	#region Override these

	protected abstract int CollisionLayersToHit { get; }
	protected abstract void OnHit(Collider _collider);

	#endregion	// Override these

	/// <summary> Reset position, state, etc </summary>
	protected virtual void Reset()
	{
		lastPosition = transform.position;
	}

	/// <summary> Called once per frame </summary>
	protected virtual void Update()
	{
		if (Physics.Raycast(transform.position, transform.position - lastPosition, out hitInfo, 0.5f, CollisionLayersToHit))
			OnHit(hitInfo.collider);

		lastPosition = transform.position;
	}
}
