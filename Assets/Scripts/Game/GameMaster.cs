﻿using UnityEngine;

public class GameMaster : MonoBehaviour
{
	#region Inspector variables

	[Header("Prefabs")]
	[SerializeField] GameObject torusPrefab = null;
	[SerializeField] PlayerMain playerPrefab = null;
	[SerializeField] GameObject UIPrefab = null;
	[SerializeField] GameObject starPrefab = null;

	[Header("Hierarchy")]
	[SerializeField] Transform torusParentPivot = null;
	[SerializeField] Transform playerParentPivot = null;
	[SerializeField] Transform sceneryParent = null;

	[Header("Managers")]
	[SerializeField] EnemyManager enemyManager = null;

	[Header("Tuning")]
	[SerializeField] int startsToSpawn = 1000;
	[SerializeField] float startMinDistance = 200;
	[SerializeField] float startMaxDistance = 700;

	#endregion

	public static PlayerMain Player { get; private set; }
	public static HUDController HUDController { get; private set; }
	public static Transform LevelPivot { get => instance.torusParentPivot; }	

	static GameMaster instance;

	/// <summary> Called when object/script first activates </summary>
	void Awake()
	{
		if (instance != null)
			throw new UnityException("Singleton instance is not null");
		instance = this;

		Instantiate(torusPrefab, torusParentPivot);
		Player = Instantiate(playerPrefab, playerParentPivot);
		Player.Init(playerParentPivot);

		GameObject ui = Instantiate(UIPrefab, Player.MainCameraTransform);
		HUDController = ui.GetComponentInChildren<HUDController>(true);
		if (HUDController == null)
			throw new UnityException("Could not find HUDController component anywhere under '" + UIPrefab.name + "'");

		Cursor.lockState = CursorLockMode.Locked;

		// Spawn randomn stars
		Vector3 starPos = Vector3.zero;
		for (int i = 0; i < startsToSpawn; ++i)
		{
			starPos = Random.insideUnitSphere.normalized * Random.Range(startMinDistance, startMaxDistance);
			Instantiate(starPrefab, starPos, Quaternion.Euler(starPos.normalized), sceneryParent);
		}
	}

	/// <summary> Called when a shot timed out </summary>
	/// <param name="_shot"> Shot's script </param>
	public static void EnemyShotMissed(EnemyShot _shot)
	{
		instance.enemyManager.RecycleShot(_shot);
	}

	/// <summary> Called when a shot hit something </summary>
	/// <param name="_shot"> Shot's script </param>
	public static void EnemyShotHit(EnemyShot _shot)
	{
		instance.enemyManager.RecycleShot(_shot);

		if (Player.IsAlive && !Player.IsInvincible)
			Player.OnHit(_shot);
	}
}
