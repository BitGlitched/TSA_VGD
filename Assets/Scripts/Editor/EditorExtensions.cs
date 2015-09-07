//|============================/
//|Copyright (c) 2015 Firebit /
//|==========================/
using UnityEngine;
using System.Collections;

public static class ExtensionMethods
{
	public static float AngleToVector(this Vector3 vec3, Vector3 angleFacingVector, Vector3 Direction)
	{
		Vector3 targetDir = angleFacingVector - vec3;
		return Vector3.Angle(targetDir, Direction);
	}
	
	public static float AngleToVector(this Vector2 vec2, Vector2 angleFacingVector, Vector2 Direction)
	{
		Vector2 targetDir = angleFacingVector - vec2;
		return Vector2.Angle(targetDir, Direction);
	}
}
