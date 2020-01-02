using UnityEngine;

class ScrollingUVs : MonoBehaviour
{
	#region Inspector variables

	[Header("Required")]
	[SerializeField] Vector2 speed = Vector2.zero;

	[Header("Optional")]
	[SerializeField] string uvPropertyName = null; // Property name from shader, eg. "_MainTex".

	#endregion

	Material thisMaterial;
	Vector2 offset;
	bool uvSpecified;

	/// <summary> Called when object/script first activates </summary>
	void Awake()
	{
		if (GetComponent<Renderer>() == null)
		{
			Debug.LogError("Could not find renderer for GameObject '" + name + "' (parent '" + transform.parent.name + "')");
			enabled = false;
		}
		else
		{
			thisMaterial = GetComponent<Renderer>().material;
			offset = thisMaterial.mainTextureOffset;
		}

		uvSpecified = string.IsNullOrEmpty(uvPropertyName);
	}

	/// <summary> Called once per frame </summary>
	void Update()
	{
		offset += speed * Time.deltaTime;

		// Wrap into [0..1) range
		if (speed.x != 0f)
			offset.x -= (int)(offset.x);
		if (speed.y != 0f)
			offset.y -= (int)(offset.y);

		if (uvSpecified)
			thisMaterial.mainTextureOffset = offset;
		else
			thisMaterial.SetTextureOffset(uvPropertyName, offset);
	}
}
