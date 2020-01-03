using UnityEngine;

public class EnemyManager : MonoBehaviour
{
	public enum EnemyTypes
	{
		Red,
		Blue,
	};

	#region Inspector variables

	[Header("Prefabs")]
	[SerializeField] GameObject redEnemyPrefab = null;
	[SerializeField] GameObject blueEnemyPrefab = null;
	[SerializeField] GameObject shotPrefab = null;

	#endregion	// Editor variables

	RecycleStack shotsPool = new RecycleStack();

	/// <summary> Singleton </summary>
	public static EnemyManager instance;

	/// <summary> Called when object/script first activates </summary>
	void Awake()
	{
		if (instance != null)
			throw new UnityException("Singleton instance already exists");
		instance = this;

		EnemyGenerator.ReadCSV();
	}

	/// <summary> Shoot now </summary>
	/// <param name="_enemy"> Enemy to shoot from </param>
	public void Shoot(Enemy _enemy)
	{
		GameObject shot = shotsPool.RetrieveOrCreate(shotPrefab);
		shot.GetComponent<EnemyShot>().Shoot(_enemy.transform.position, Quaternion.LookRotation((GameMaster.Player.transform.position - _enemy.transform.position).normalized), _enemy.shotMoveSpeed, _enemy.shotLifetime);
	}

	/// <summary> Recycle a shot ready for re-use </summary>
	/// <param name="_shot"> Shot's component </param>
	public void RecycleShot(EnemyShot _shot)
	{
		shotsPool.Recycle(_shot.gameObject);
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
