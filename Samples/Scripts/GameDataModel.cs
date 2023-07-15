using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameDataModel
{
	public GameDataModel()
	{
		Slots = new List<SaveState>();
		Slots.Add( new SaveState());
		Slots.Add( new SaveState());
		Slots.Add( new SaveState());
		Slots.Add(new SaveState());
		Slots.Add(new SaveState());
	}
	public List<SaveState> Slots;
}

[System.Serializable]
public class SaveState
{

	public string mouseSensitivity;
	public string volume;
	public string name;
	
}


