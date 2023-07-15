using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//todo make this a don't destroy on load behaviour. just instantiate it once and load data once.
public class DataRepository : MonoBehaviour
{
	[HideInInspector]
	public GameDataModel gameData = new GameDataModel();
    
	[HideInInspector]
	public List<GameObject> Subscribers;
    
	private int CurrentLoadSlot = 1;
	private static float LoadDealy = 0.1f;
	string saveFile;
	private SampleServiceLocator ServiceLocator;
	//public static DataRepository Instance;
	public bool HasLoaded = false;

	private void Awake()
	{
		//Instance = this;
		ServiceLocator = FindObjectOfType<SampleServiceLocator>();
    }
    
    void Start()
	{
		//Debug.Log("Data Service | Start");
        saveFile = Application.persistentDataPath + "/gamedata.json";
        ReadFile();
	}
	
	public void UpdateSettingsAndSave(SampleSettingsController settings)
	{
		gameData.Slots[0].mouseSensitivity = settings.mx.ToString();
		gameData.Slots[0].mouseSensitivity = settings.volume.ToString();
		gameData.Slots[0].mouseSensitivity = settings.playerName;
		WriteFile();
	}
	public SaveState GetCurrentSaveState()
	{
		return gameData.Slots[CurrentLoadSlot];
	}
	public bool WasThisCollectedInPastGame(string ID)
	{
		return false;
		//return gameData.Slots[CurrentLoadSlot].Collectables.Exists(x=>x.ID == ID);
	}
	
	public void CollectItem(Collectable collectable)
	{
		//gameData.Slots[CurrentLoadSlot].Collectables.Add(collectable);
	}
	
	public void LoadGameFromCurrentLoadSlot()
	{
		
		//SceneManager.LoadScene(gameData.Slots[CurrentLoadSlot].LastScene);
	}
	
	public void SetLoadSlot(int number)
	{
		if(number == 0)throw new System.Exception("must be positive non zero");
		CurrentLoadSlot = number;
	}
	
	public void SaveGame()
	{
		//var player = ServiceLocator.PlayerController;
		//gameData.Slots[CurrentLoadSlot].PlayerAbilityUnlocks.IsDoubleJumpUnlocked = player.PlayerAbilityUnlocks.IsDoubleJumpUnlocked;
		//gameData.Slots[CurrentLoadSlot].PlayerAbilityUnlocks.IsSuperJumpUnlocked = player.PlayerAbilityUnlocks.IsSuperJumpUnlocked;
		//gameData.Slots[CurrentLoadSlot].PlayerAbilityUnlocks.IsSuperRunUnlocked = player.PlayerAbilityUnlocks.IsSuperRunUnlocked;
		WriteFile();
	}
	
    public void ReadFile()
	{
		//Debug.Log("Data Service | ReadFile");
    	
        if (File.Exists(saveFile))
        {
	        //Debug.Log("Data Service | Loading");
        	
	        string fileContents = File.ReadAllText(saveFile);
	        //Debug.Log("Data Service | Loading | done");
            
            gameData = JsonUtility.FromJson<GameDataModel>(fileContents);
	        StartCoroutine(DelayedLoad());
        }
        else
        {
        	gameData = new GameDataModel();
	        //Debug.Log("Data Service | new game ");
        	
            WriteFile();
        }
	}
    
	IEnumerator DelayedLoad()
	{
		yield return new WaitForSeconds(LoadDealy);	
		HasLoaded = true;
		Subscribers.ForEach(subscriber => subscriber.SendMessage("OnDataLoaded",SendMessageOptions.DontRequireReceiver));
		//Debug.Log("Data Service | notify subs ");
		
	}
	
	private void WriteFile()
	{
		
		//Debug.Log("Data Service | WriteFile");
        string jsonString = JsonUtility.ToJson(gameData);
        File.WriteAllText(saveFile, jsonString);
	}
	
	//TODO: something needs to call this !!
	public void PickLoadState(int loadState)
	{
		CurrentLoadSlot = loadState;
	}
	
	public void UpdateScenarioScriptDataRepoAndSave()
	{
		//if(CurrentLoadSlot == 0) throw new System.Exception("can't save new game - user needs to pick a save state."); 
			
		//convert current slot's data to a flat list
	}
	
	public void UpdateScenarioScriptDataRepo()
	{
		//convert current slot's data to a flat list
		
		//collectables:
		//puzzles & bosses
		//locations
		//secrets
		
		
		var DataType = "";
		var ID = "";
		var FlatName = "";
		
		
	}
	
	public void SaveDataRepoFromScenarioScript(string dataIdentifier, string value)
	{
		//convert flat list to the a game data, set to current slot and save
	}
}
