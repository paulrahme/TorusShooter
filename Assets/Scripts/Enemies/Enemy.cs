using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Collidable
{
	#region Editor variables

	[Header("Enemy")]
	[SerializeField] float respawnTime = 3.0f;

	[Header("Shooting")]
	[SerializeField] float shotTimeMin = 3.0f;
	[SerializeField] float shotTimeMax = 6.0f;
	public float shotMoveSpeed = 5.0f;
	public float shotLifetime = 2.0f;

	#endregion	// Editor variables

	float timeToRespawn;
	float timeToShoot;

	/// <summary> Called before first Update() </summary>
	void Start()
	{
		Reset();
	}

	/// <summary> Reset position, state etc </summary>
	protected override void Reset()
	{
		base.Reset();

		timeToRespawn = 0.0f;
		transform.localScale = Vector3.one;
		CalcNextTimeToShoot();
	}

	/// <summary> Triggered when hit by one of the player's shots </summary>
	public void OnShot()
	{
		if (respawnTime == 0.0f)
			gameObject.SetActive(false);
		else
			transform.localScale = Vector3.zero;
		
		if (respawnTime != 0.0f)
			timeToRespawn = Time.fixedTime + respawnTime;
	}

	/// <summary> Called once per frame </summary>
	protected override void Update()
	{
		base.Update();

		if ((timeToRespawn != 0.0f) && (Time.fixedTime > timeToRespawn))
			Reset();
		else if (Time.fixedTime > timeToShoot)
		{
			CalcNextTimeToShoot();
			EnemyManager.Instance.Shoot(this);
		}
	}

	/// <summary> Finds the next time to fire a shot </summary>
	void CalcNextTimeToShoot()
	{
		timeToShoot = Time.fixedTime + Random.Range(shotTimeMin, shotTimeMax);
	}

	#region Collidable overrides

	protected override int CollisionLayersToHit { get { return Layers.Player; } }

	protected override void OnHit(Collider collider)
	{
		PlayerMain.Instance.OnHit(this);
	}

	#endregion	// Collidable overrides
}
