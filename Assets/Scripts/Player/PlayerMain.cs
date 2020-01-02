using UnityEngine;

public class PlayerMain : MonoBehaviour
{
	enum States { Alive, Respawning };
	enum VerticalMovementTypes { None, Cylindrical, PivotLevel };
	
	#region Inspector variables

	[Header("Horizontal movement")]
	[SerializeField] Transform parentPivot = null;
	[SerializeField] float moveHorizSpeed = 30.0f;
	[SerializeField] float moveHorizBoostMult = 2.0f;
	[Space(10)]
	[SerializeField] VerticalMovementTypes verticalMovementType = VerticalMovementTypes.None;


	[Header("Vertical movement (Cylindrical)")]
	[SerializeField] float moveSpeedVert = 10.0f;
	[SerializeField] float verticalLimit = 15.0f;

	[Header("Level spinning on pivot")]
	[SerializeField] float rotateSpeedPivotUpDown = 20.0f;
	[SerializeField] Transform levelPivot = null;

	[Header("Mouse Movement")]
	[SerializeField] float sensitivityVert = 1.0f;
	[SerializeField] float sensitivityHoriz = 2.0f;

	[Header("Shooting")]
	[SerializeField] Transform[] turrets = null;
	[SerializeField] GameObject shotPrefab = null;
	[SerializeField] GameObject shotHitPFXPrefab = null;
	[SerializeField] float timeBetweenShots = 0.2f;
	[SerializeField] float shotStartDistance = 0.5f;
	[SerializeField] float shotMoveSpeed = 30.0f;
	[SerializeField] float shotLifetime = 2.0f;

	[Header("Death")]
	[SerializeField] float respawnDuration = 3.0f;

	#endregion	// Editor variables

	States state;
	Vector3 pivotEulerAngles;
	Vector3 eulerAngles;
	Vector3 localPosition;
	RecycleStack shotsPool = new RecycleStack();
	RecycleStack shotPFXPool = new RecycleStack();
	float respawnTime;
	float nextShotTime;

	/// <summary> Singleton </summary>
	public static PlayerMain instance;

	/// <summary> Called when object/script first activates </summary>
	void Awake()
	{
		if (instance != null)
			throw new UnityException("Singleton instance already exists");
		instance = this;

		pivotEulerAngles = parentPivot.localEulerAngles;
		eulerAngles = transform.localEulerAngles;
		localPosition = transform.localPosition;

		Cursor.lockState = CursorLockMode.Locked;
	}

	/// <summary> Called before first Update() </summary>
	void Start()
	{
		SetState(States.Alive);
	}

	/// <summary> Changes to a new state </summary>
	/// <param name="_state"> States... state </param>
	void SetState(States _state)
	{
		state = _state;

		switch (state)
		{
			case States.Alive:
				HUDController.instance.reviveHierarchy.SetActive(false);
				break;

			case States.Respawning:
				respawnTime = Time.fixedTime + respawnDuration;
				HUDController.instance.reviveHierarchy.SetActive(true);
				break;

			default:
				break;
		}
	}

	/// <summary> Called once per frame </summary>
	void Update()
	{
		switch (state)
		{
			case States.Alive:
				UpdateKeyboardMovement();
				UpdateMouseLook();
				UpdateShooting();
				break;

			case States.Respawning:
				if (Time.fixedTime > respawnTime)
					SetState(States.Alive);
				else
					HUDController.instance.reviveTimeText.text = ((int)(respawnTime - Time.fixedTime + 1.0f)).ToString();
				break;

			default:
				throw new UnityException("Unhandled Player state " + state);
		}
	}

	/// <summary> Updates the keyboard movement around the torus </summary>
	void UpdateKeyboardMovement()
	{
		// Horizontal movement
		float horizVelocity = 0.0f;
		if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
			horizVelocity = moveHorizSpeed * Time.deltaTime;
		else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
			horizVelocity = -moveHorizSpeed * Time.deltaTime;

		if (horizVelocity != 0.0f)
		{
			if (Input.GetKey(KeyCode.Space))
				horizVelocity *= moveHorizBoostMult;

			pivotEulerAngles.y += horizVelocity;
			parentPivot.localEulerAngles = pivotEulerAngles;
		}

		// Vertical movement
		switch (verticalMovementType)
		{
			case VerticalMovementTypes.None:
				break;

			case VerticalMovementTypes.Cylindrical:
				if ((Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) && (localPosition.y < verticalLimit))
					localPosition.y += moveSpeedVert * Time.deltaTime;
				else if ((Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) && (localPosition.y > -verticalLimit))
					localPosition.y -= moveSpeedVert * Time.deltaTime;
				transform.localPosition = localPosition;
				break;

			case VerticalMovementTypes.PivotLevel:
				if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
					levelPivot.Rotate(transform.right, rotateSpeedPivotUpDown * Time.deltaTime);
				else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
					levelPivot.Rotate(transform.right, -rotateSpeedPivotUpDown * Time.deltaTime);
				break;

			default:
				throw new UnityException("Unhandled Vertical Movement Type " + verticalMovementType);
		}
	}

	/// <summary> Updates looking around using the mouse </summary>
	void UpdateMouseLook()
	{
		eulerAngles.x -= Input.GetAxis("Mouse Y") * sensitivityVert;
		eulerAngles.y += Input.GetAxis("Mouse X") * sensitivityHoriz;
		transform.localEulerAngles = eulerAngles;
	}

	/// <summary> Updates shooting into the screen </summary>
	void UpdateShooting()
	{
		if (Input.GetMouseButtonDown(0))
			nextShotTime = 0.0f;

		if (Input.GetMouseButton(0) && (Time.fixedTime > nextShotTime))
		{
			nextShotTime = Time.fixedTime + timeBetweenShots;

			for (int i = 0; i < turrets.Length; ++i)
			{
				Transform turret = turrets[i];
				GameObject shot = shotsPool.RetrieveOrCreate(shotPrefab);
				Vector3 forwardOffset = turret.forward * shotStartDistance;
				Quaternion forwardRot = turret.rotation;
				shot.GetComponent<PlayerShot>().Shoot(turret.position + forwardOffset, forwardRot, shotMoveSpeed, shotLifetime);
			}
		}
	}

	/// <summary> Called when a shot timed out </summary>
	/// <param name="_shot"> Shot's script </param>
	public void ShotMissed(PlayerShot _shot)
	{
		shotsPool.Recycle(_shot.gameObject);
	}

	/// <summary> Called when a shot hit something </summary>
	/// <param name="_shot"> Shot's script </param>
	public void ShotHit(PlayerShot _shot, GameObject _objHit)
	{
		shotPFXPool.RetrieveOrCreate(shotHitPFXPrefab).transform.position = _shot.transform.position;
		shotsPool.Recycle(_shot.gameObject);

		Enemy enemy = _objHit.GetComponentInParent<Enemy>();
		if (enemy != null)
			enemy.OnShot();
	}

	/// <summary> Called when shot PFX has finished playing </summary>
	/// <param name="_gameObj"> PFX's GameObject </param>
	public void RecycleShotPFX(GameObject _gameObj)
	{
		shotPFXPool.Recycle(_gameObj);
	}

	/// <summary> Called when hit </summary>
	/// <param name="_whatHit"> What hit the player </param>
	public void OnHit(Collidable _whatHit)
	{
		if (state == States.Alive)
			SetState(States.Respawning);
	}
}
