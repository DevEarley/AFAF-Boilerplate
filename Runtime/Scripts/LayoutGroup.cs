using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]

public class LayoutGroup 
{
	[Tooltip("Relative Screen Position from the top-left.")]
	
	public Vector2 RelativeScreenPosition;
	public List<LayoutObject> LayoutObjects;
	public string Name;
	public int ID;
	[Tooltip("Height before it scolls")]
	
	public float Height;
}
