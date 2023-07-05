using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]

public class LayoutObject
{
	public LayoutObjectTypes LayoutObjectType;
	[Tooltip("Index in the group.")]
	public int Index;

	public Transform transform;
	private int GroupID;
	
	private string ID;
}
public enum LayoutObjectTypes
{
	BlockingFullWidth,
	FixedWidth,
	FillWidth
}