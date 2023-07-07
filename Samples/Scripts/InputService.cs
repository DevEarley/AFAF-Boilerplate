using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InputService
{
	public static float ClampAngle(float angle)
	{
		if (angle >= 360.0f) return angle-360.0f;
		if (angle < 0.0f) return 360.0f+angle;
		else return angle;
	}

	public static Vector3 Vector3FromAngle(float angle)
	{
		var radians = Mathf.Deg2Rad * angle;
		return Vector3.left * Mathf.Cos(radians) + Vector3.forward * Mathf.Sin(radians);
	}

	public static Vector2 Vector2FromBinaryInput(bool up, bool left, bool right, bool down)
	{
		float upFloat = up ? 1 : 0;
		float leftFloat = left ? 1 : 0;
		float rightFloat = right ? 1 : 0;
		float downFloat = down ? 1 : 0;
		return new Vector2(rightFloat - leftFloat, upFloat - downFloat);
	}
    
	public static Vector2 GetBinaryVector2(Vector2 vector)
	{
		var x = vector.x<0? -1.0f: vector.x>0? 1.0f : 0.0f;
		var y = vector.y<0? -1.0f: vector.y>0? 1.0f : 0.0f;
		return new Vector2(x,y);
	}
	
	public static Vector3 GetBinaryVector3(Vector3 vector)
	{
		var x = vector.x<0? -1.0f: vector.x>0? 1.0f : 0.0f;
		var y = vector.y<0? -1.0f: vector.y>0? 1.0f : 0.0f;
		var z = vector.z<0? -1.0f: vector.z>0? 1.0f : 0.0f;
		return new Vector3(x,y,z);
	}

	public static Vector3 ConvertV2toV3(Vector2 vector,float up)
	{
		return new Vector3(vector.x,up,vector.y);	
	}
}
