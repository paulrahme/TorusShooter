using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
	#region Editor variables

	[Header("Revive")]
	public GameObject reviveHierarchy;
	public Text reviveTimeText;

	#endregion	// Editor variables

	/// <summary> Singleton </summary>
	public static HUDController Instance;

	/// <summary> Called when object/script activates </summary>
	void Awake()
	{
		if (Instance != null)
			throw new UnityException("Singleton instance already exists");
		Instance = this;
	}
}
