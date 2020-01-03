using UnityEngine;

public class PlayerShot : Shot
{
	protected override int CollisionLayersToHit { get { return Layers.Enemy; } }

	protected override void OnHit(Collider collider)
	{
		GameMaster.Player.ShotHit(this, collider.gameObject);
	}

	protected override void OnMissed()
	{
		GameMaster.Player.ShotMissed(this);
	}
}
