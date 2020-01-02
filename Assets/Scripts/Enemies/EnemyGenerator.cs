using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator
{
	/// <summary> Stats for all levels </summary>
	static EnemyInfo[] enemyInfo;
	static int nextEnemyIdx;

	/// <summary> Parses the contents of the CSV file </summary>
	public static void ReadCSV()
	{
		List<EnemyInfo> enemyInfoList = new List<EnemyInfo>();

		// Load all text strings
		TextAsset textAsset = Resources.Load<TextAsset>("CSV/EnemyInfo");
		string csv = textAsset.text.Replace("\r\n", "\n");
		string[] lines = csv.Split('\n');
		for (int rowNo = 1; rowNo < lines.Length; ++rowNo)
		{
			string line = lines[rowNo];
			if (!string.IsNullOrEmpty(line.Trim()))
			{
				string[] data = line.Split('\t');
				enemyInfoList.Add(new EnemyInfo(data));
			}
		}

		enemyInfo = enemyInfoList.ToArray();
		nextEnemyIdx = 0;
	}

	/// <summary> Checks if the next enemy is ready to spawn </summary>
	/// <param name="_time"> Current gameplay time </param>
	/// <returns> The enemy if it's ready to spawn, else null </returns>
	public static EnemyInfo CheckForNextEnemy(float _time)
	{
		if ((nextEnemyIdx < enemyInfo.Length) && (_time > enemyInfo[nextEnemyIdx].spawnTime))
			return enemyInfo[nextEnemyIdx++];
		else
			return null;
	}
}

public class EnemyInfo
{
	enum CSVColumns
	{
		SpawnTime,
		EnemyType,
		RotateSpeed,
		BobFrequency,
		BobAmplitudeX,
		BobAmplitudeY,
		BobAmplitudeZ,
		SpiralSpeed,
		SpiralMaxRadius,
	}

	public float spawnTime;
	public EnemyManager.EnemyTypes enemyType;
	public Vector3 rotateVelocity;
	public float bobFrequency;
	public Vector3 bobAmplitude;
	public Vector3 spiralVelocity;
	public float spiralMaxRadius;

	/// <summary> Constructor </summary>
	public EnemyInfo(string[] _data)
	{
		Helpers.ReadFloat(_data, (int)CSVColumns.SpawnTime, out spawnTime);

		enemyType = Helpers.ParseEnum<EnemyManager.EnemyTypes>(_data[(int)CSVColumns.EnemyType]);

		float rotateSpeed;
		Helpers.ReadFloat(_data, (int)CSVColumns.RotateSpeed, out rotateSpeed);
		rotateVelocity = new Vector3(0.0f, rotateSpeed, 0.0f);

		Helpers.ReadFloat(_data, (int)CSVColumns.BobFrequency, out bobFrequency);

		Helpers.ReadFloat(_data, (int)CSVColumns.BobAmplitudeX, out bobAmplitude.x);
		Helpers.ReadFloat(_data, (int)CSVColumns.BobAmplitudeY, out bobAmplitude.y);
		Helpers.ReadFloat(_data, (int)CSVColumns.BobAmplitudeZ, out bobAmplitude.z);

		spiralVelocity = Helpers.Vec3Zero;
		Helpers.ReadFloat(_data, (int)CSVColumns.SpiralSpeed, out spiralVelocity.x);

		Helpers.ReadFloat(_data, (int)CSVColumns.SpiralMaxRadius, out spiralMaxRadius);
	}
}
