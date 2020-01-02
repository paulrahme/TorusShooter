using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Helpers
{
	public static readonly Vector3 Vec3Zero = Vector3.zero;	// Because Vector3.zero creates a new one under-the-hood every time
	public static readonly Vector3 Vec3One = Vector3.one;	// Because Vector3.one creates a new one under-the-hood every time

	public static void ReadInt(string[] _data, int _column, out int _outInt)
	{
		string current = _data[_column];
		if (string.IsNullOrEmpty(current))
			_outInt = 0;
		else if (!int.TryParse(current, out _outInt))
		{
			string debugStr = "Could not parse '" + current + "' into Column '" + _column + "' (int value), CSV row was: '";
			for (int i = 0; i < _data.Length; ++i)
				debugStr += _data[i] + ((i < _data.Length - 1) ? "," : "'");

			throw new UnityException(debugStr);
		}
	}

	public static void ReadFloat(string[] _data, int _column, out float _outFloat)
	{
		string current = _data[_column];
		if (string.IsNullOrEmpty(current))
			_outFloat = 0.0f;
		else if (!float.TryParse(current, out _outFloat))
		{
			string debugStr = "Could not parse '" + current + "' into Column '" + _column + "' (float value), CSV row was: '";
			for (int i = 0; i < _data.Length; ++i)
				debugStr += _data[i] + ((i < _data.Length - 1) ? "," : "'");

			throw new UnityException(debugStr);
		}
	}

	public static T ParseEnum<T>(string _value)
	{
		T convertedEnum;

		try { convertedEnum = (T)Enum.Parse(typeof(T), _value, true); }
		catch (Exception) { throw new UnityException("Could not parse '" + _value + " into " + typeof(T).ToString()); }

		return convertedEnum;
	}

	public static void ParentAndResetTransform(Transform _trans, Transform _parent)
	{
		_trans.parent = _parent;
		_trans.localPosition = _trans.localEulerAngles = Vec3Zero;
		_trans.localScale = Vec3One;
	}
}
