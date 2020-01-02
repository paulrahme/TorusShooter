using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShot : Shot
{
	protected override int CollisionLayersToHit { get { return Layers.Enemy; } }

	protected override void OnHit(Collider collider)
	{
		PlayerMain.Instance.ShotHit(this, collider.gameObject);
	}

	protected override void OnMissed()
	{
		PlayerMain.Instance.ShotMissed(this);
	}
}
