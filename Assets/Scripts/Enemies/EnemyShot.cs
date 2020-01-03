using UnityEngine;

public class EnemyShot : Shot
{
	protected override int CollisionLayersToHit { get { return Layers.Player; } }

	protected override void OnHit(Collider collider)
	{
		GameMaster.EnemyShotHit(this);
	}

	protected override void OnMissed()
	{
		GameMaster.EnemyShotMissed(this);
	}
}
