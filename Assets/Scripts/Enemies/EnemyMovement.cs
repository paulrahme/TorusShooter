using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
	#region Editor variables

	[Header("Rotation")]
	[SerializeField] Transform baseTransform;
	[SerializeField] Vector3 rotateVelocity = new Vector3(0.0f, 50.0f, 0.0f);

	[Header("Spiralling Outward")]
	[SerializeField] Transform spiralTransform;
	[SerializeField] Vector3 spiralVelocity = new Vector3(3.0f, 0.0f, 0.0f);
	[SerializeField] float spiralMaxRadius = 40.0f;

	[Header("Bobbing")]
	[SerializeField] Transform childTransform;
	[SerializeField] float bobFrequency = 3.14152f;
	[SerializeField] Vector3 bobAmplitude = new Vector3(0.0f, 2.0f, 0.0f);

	#endregion	// Editor variables

	bool isRotating, isSpiralling, isBobbing;
	Vector3 rotateAngles;
	Vector3 bobBasePos;
	float bobAngle;

	/// <summary> Initialises variables from info structure </summary>
	/// <param name="_info"> Info structure </param>
	public void Init(EnemyInfo _info)
	{
		rotateVelocity = _info.rotateVelocity;
		isRotating = (rotateVelocity != Vector3.zero);

		spiralVelocity = _info.spiralVelocity;
		spiralMaxRadius = _info.spiralMaxRadius;
		isSpiralling = (spiralVelocity != Vector3.zero);

		bobFrequency = _info.bobFrequency;
		bobAmplitude = _info.bobAmplitude;
		isBobbing = (bobFrequency != 0.0f);
	}

	/// <summary> Called when object/script activates </summary>
	void Awake()
	{
		bobBasePos = childTransform.localPosition;
	}

	/// <summary> Called before first Update() </summary>
	void Start()
	{
		if (baseTransform != null)
			rotateAngles = baseTransform.eulerAngles;
	}

	/// <summary> Called once per frame </summary>
	protected virtual void Update()
	{
		if (isRotating)
		{
			rotateAngles += rotateVelocity * Time.deltaTime;
			baseTransform.localEulerAngles = rotateAngles;
		}

		if (isSpiralling)
		{
			if (spiralTransform.localPosition.x < spiralMaxRadius)
				spiralTransform.localPosition += spiralVelocity * Time.deltaTime;
		}

		if (isBobbing)
		{
			bobAngle += bobFrequency * Time.deltaTime;
			childTransform.localPosition = bobBasePos + (bobAmplitude * Mathf.Sin(bobAngle));
		}
	}
}
