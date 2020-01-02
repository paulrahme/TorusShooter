using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShotHitPFX : MonoBehaviour
{
	#region Editor variables

	[SerializeField] ParticleSystem pfx;

	#endregion	// Editor variables

	/// <summary> Called once per frame </summary>
	void Update()
	{
		if (!pfx.IsAlive())
			PlayerMain.Instance.RecycleShotPFX(gameObject);
	}
}
