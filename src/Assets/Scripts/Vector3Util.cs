using System;
using UnityEngine;
public class Vector3Util
{
	public static Vector3 Vector3(double x, double y, double z)
	{
		return new Vector3(System.Convert.ToSingle(x), System.Convert.ToSingle (y), System.Convert.ToSingle (z));
	}
}