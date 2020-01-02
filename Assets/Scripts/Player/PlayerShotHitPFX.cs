using UnityEngine;

public class PlayerShotHitPFX : MonoBehaviour
{
	#region Inspector variables

	[SerializeField] ParticleSystem pfx = null;

	#endregion	// Editor variables

	/// <summary> Called once per frame </summary>
	void Update()
	{
		if (!pfx.IsAlive())
			PlayerMain.instance.RecycleShotPFX(gameObject);
	}
}
