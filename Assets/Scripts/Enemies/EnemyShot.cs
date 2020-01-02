using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShot : Shot
{
	protected override int CollisionLayersToHit { get { return Layers.Player; } }

	protected override void OnHit(Collider collider)
	{
		EnemyManager.Instance.ShotHit(this);
	}

	protected override void OnMissed()
	{
		EnemyManager.Instance.ShotMissed(this);
	}
}
