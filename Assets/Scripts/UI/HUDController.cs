using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
	#region Inspector variables

	[Header("Revive")]
	public GameObject reviveHierarchy;
	public Text reviveTimeText;

	#endregion	// Editor variables

	/// <summary> Singleton </summary>
	public static HUDController instance;

	/// <summary> Called when object/script first activates </summary>
	void Awake()
	{
		if (instance != null)
			throw new UnityException("Singleton instance already exists");
		instance = this;
	}
}
