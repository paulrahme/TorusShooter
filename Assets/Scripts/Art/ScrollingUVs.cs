using UnityEngine;

class ScrollingUVs : MonoBehaviour
{
	[SerializeField] string uvName;
	[SerializeField] Vector2 speed = Vector2.zero;

	Material thisMaterial;
	Vector2 offset;
	bool uvSpecified;

	void Awake()
	{
		if (GetComponent<Renderer>() == null)
		{
			Debug.LogError("Could not find renderer for GameObject '"+name+"' (parent '"+transform.parent.name+"')");
			enabled = false;
		}
		else
		{
			thisMaterial = GetComponent<Renderer>().material;
			offset = thisMaterial.mainTextureOffset;
		}

		uvSpecified = string.IsNullOrEmpty(uvName);
	}
	
	void Update()
	{
		offset += speed * Time.deltaTime;

		// Wrap into [0..1) range
		if (speed.x != 0.0f) { offset.x -= (int)(offset.x); }
		if (speed.y != 0.0f) { offset.y -= (int)(offset.y); }

		if(uvSpecified)
			thisMaterial.mainTextureOffset = offset;
		else
			thisMaterial.SetTextureOffset(uvName, offset);
	}
}