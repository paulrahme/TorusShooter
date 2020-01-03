using UnityEngine;

public class GameMaster : MonoBehaviour
{
	#region Inspector variables

	[Header("Prefabs")]
	[SerializeField] GameObject torusPrefab = null;
	[SerializeField] PlayerMain playerPrefab = null;
	[SerializeField] GameObject UIPrefab = null;

	[Header("Hierarchy")]
	[SerializeField] Transform torusParentPivot = null;
	[SerializeField] Transform playerParentPivot = null;

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
	}
}
