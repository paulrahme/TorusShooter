using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Shot : Collidable
{
	float moveSpeed;
	float lifetime;
	RaycastHit hitInfo;
	float lifetimeEnd;

	#region Override these

	protected abstract void OnMissed();

	#endregion	// Override these

	/// <summary> Starts the shot firing </summary>
	/// <param name="_pos"> Starting position </param>
	/// <param name="_rot"> Facing rotation </param>
	public void Shoot(Vector3 _pos, Quaternion _rot, float _moveSpeed, float _lifetime)
	{
		moveSpeed = _moveSpeed;
		lifetime = _lifetime;

		transform.SetPositionAndRotation(_pos, _rot);
		lifetimeEnd = Time.fixedTime + lifetime;
	}

	/// <summary> Called once per frame </summary>
	protected override void Update()
	{
		base.Update();

		if (Time.fixedTime > lifetimeEnd)
			OnMissed();
		else
			transform.position += transform.forward * (moveSpeed * Time.deltaTime);
	}
}
