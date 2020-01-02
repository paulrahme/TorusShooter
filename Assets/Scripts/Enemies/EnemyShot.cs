using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShot : Shot
{
	protected override int CollisionLayersToHit { get { return Layers.Player; } }

	protected override void OnHit(Collider collider)
	{
		EnemyManager.instance.ShotHit(this);
	}

	protected override void OnMissed()
	{
		EnemyManager.instance.ShotMissed(this);
	}
}
