using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
	public enum EnemyTypes
	{
		Red,
		Blue,
	};

	#region Editor variables

	[Header("Prefabs")]
	[SerializeField] GameObject redEnemyPrefab;
	[SerializeField] GameObject blueEnemyPrefab;
	[SerializeField] GameObject shotPrefab;

	#endregion	// Editor variables

	RecycleStack shotsPool = new RecycleStack();

	/// <summary> Singleton </summary>
	public static EnemyManager Instance;

	/// <summary> Called when object/script activates </summary>
	void Awake()
	{
		if (Instance != null)
			throw new UnityException("Singleton instance already exists");
		Instance = this;

		EnemyGenerator.ReadCSV();
	}

	/// <summary> Shoot now </summary>
	/// <param name="_enemy"> Enemy to shoot from </param>
	public void Shoot(Enemy _enemy)
	{
		GameObject shot = shotsPool.RetrieveOrCreate(shotPrefab);
		shot.GetComponent<EnemyShot>().Shoot(_enemy.transform.position, Quaternion.LookRotation((PlayerMain.Instance.transform.position - _enemy.transform.position).normalized), _enemy.shotMoveSpeed, _enemy.shotLifetime);
	}

	/// <summary> Called when a shot timed out </summary>
	/// <param name="_shot"> Shot's script </param>
	public void ShotMissed(EnemyShot _shot)
	{
		shotsPool.Recycle(_shot.gameObject);
	}

	/// <summary> Called when a shot hit something </summary>
	/// <param name="_shot"> Shot's script </param>
	public void ShotHit(EnemyShot _shot)
	{
		shotsPool.Recycle(_shot.gameObject);

		PlayerMain.Instance.OnHit(_shot);
	}

	/// <summary> Called once per frame </summary>
	void Update()
	{
		EnemyInfo nextEnemyInfo = EnemyGenerator.CheckForNextEnemy(Time.fixedTime);
		if (nextEnemyInfo != null)
		{
			GenerateEnemy(nextEnemyInfo);
		}
	}

	/// <summary> Creates and initialises an enemy from an info struct </summary>
	/// <param name="_info"> Info struct </param>
	void GenerateEnemy(EnemyInfo _info)
	{
		GameObject gameObj;
		switch (_info.enemyType)
		{
			case EnemyTypes.Red:	gameObj = GameObject.Instantiate(redEnemyPrefab);	break;
			case EnemyTypes.Blue:	gameObj = GameObject.Instantiate(blueEnemyPrefab);	break;

			default: throw new UnityException("Unhandled Enemy Type " + _info.enemyType);
		}

		Helpers.ParentAndResetTransform(gameObj.transform, transform);
		EnemyMovement movementScript = gameObj.GetComponentInChildren<EnemyMovement>();
		if (movementScript == null)
			throw new UnityException("Could not find ConstantBob component on '" + gameObj.name + "'");
		movementScript.Init(_info);
	}
}
