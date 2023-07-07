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
	//public List<Collectable> Collectables = new List<Collectable>();
	//public List<PuzzleModel> PuzzleModels = new List<PuzzleModel>();

	public string mouseSensitivity;
	public string volume;
	public string name;
	
	//public string LastSpawn = "S1";
	//public string LastScene = "intro";
	//public int TotalHealth = 2;
	//public int CurrentHealth = 2;
}

public enum CollectableType
{
   
}

public enum PuzzleType
{
	TimedDoorPuzzle,
	ButtonDoorPuzzle
}


[System.Serializable]
public class PuzzleModel
{
	public PuzzleType PuzzleType;
	public int PuzzleId;
	public bool Completed;
}



