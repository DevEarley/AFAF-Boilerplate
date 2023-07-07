using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Layout{
	
	[Tooltip("Absolute Screen Position from the top-center")]
	public Vector2 AbsoluteScreenPosition;
	public List<LayoutGroup> LayoutGroups;
	public string Name;
	public int ID;
}
