using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SampleGameController : MonoBehaviour, IReactToAllScenarios 
{
	[HideInInspector]
	public ServiceLocator ServiceLocator;
	public SettingsController SettingsController;
	private SamplePlayerController PlayerController; 
	private LootLockerService LootLockerService;
	//public GameObject SceneTransitionerPrefab;
	private void Awake() 
	{ 
		SettingsController = FindAnyObjectByType<SettingsController>();
		LootLockerService = FindAnyObjectByType<LootLockerService>();
		PlayerController = FindAnyObjectByType<SamplePlayerController>();
		ServiceLocator = FindAnyObjectByType<ServiceLocator>();  
		
		//if(GameObject.Find("SceneTransitioner")==null)
		//{
		//	GameObject.Instantiate(SceneTransitionerPrefab);
		//}	
	}
	
	protected void Start()
	{
		ServiceLocator.DataRepository.Subscribers.Add(this.gameObject);
	}
	
	protected void OnDataLoaded()
	{
		var data = ServiceLocator.DataRepository.gameData;
		ServiceLocator.ScenarioService.LoadData(new ScenarioDataSet(data));
		var gameData = data.Slots[0];
		if(gameData.mouseSensitivity!=""&& gameData.mouseSensitivity!=null)
		{
			ChangeMouseSensitivityX(gameData.mouseSensitivity);
			ChangeMouseSensitivityY(gameData.mouseSensitivity);
		}
		if(gameData.volume!=""&& gameData.volume!=null)
		{
			ServiceLocator.SoundController.SetVolume(float.Parse(gameData.volume));
			ChangeVolume(gameData.volume);
		}
		if(gameData.name!=""&& gameData.name!=null)
		{
			LootLockerService.SetName(gameData.name);
		}
	}
	public void Settings_OnBack()
	{
		ServiceLocator.ScenarioService.Continue();
	}
    
	public void OnScenarioCall(string param1, string param2)
	{
		switch(param1)
		{
			case "animate":
					GameUtility.AnimateObject(param2);
				break;
			case "new-game":
				GameUtility.LoadScene("map");
				break;
			case "main menu":
			case "title":
				GameUtility.LoadScene("title");
				break;
		
			case "load-scene":
				GameUtility.LoadScene(param2);
				break;
		
			case "show-settings":
				SettingsController.ShowSettings(Settings_OnBack);
				break;
			
	
			case "set-name-for-LL":
				ServiceLocator.LootLockerService.SetName(param2);
				break;
				
			case "quit":
			case "exit":
				Application.Quit();
				break;
			
			case "cam":
			case "camera":
			case "set-camera":
			case "set-cam":
				SetCamera(param2);
				break;
				
			case "mx":
			case "mouse-x":
			case "mouse-sensitivity-x":
				ChangeMouseSensitivityX(param2);
				break;
			case "cs":
			case "controller-sensitivity":
				ChangeControllerSensitivity(param2);
				break;
				
			case "my":
			case "mouse-y":
			case "mouse-sensitivity-y":
				ChangeMouseSensitivityY(param2);
				break;
				
			case "v":
			case "volume":
				ChangeVolume(param2);
				break;
				
			case "hs1":
			case "hs":
			case "high-score":
			case "high-score-mountain":
				ServiceLocator.LootLockerService.ShowScoresOnScreen("15696","Console");
				break;
				
			case "hs2":
			case "high-score-tree":
				ServiceLocator.LootLockerService.ShowScoresOnScreen("15697","Console");
				break;
				
			case "t":
			case "time":
			case "set-time":
				Time.timeScale = float.Parse(param2);
				break;
				
			case "freeze":
			case "freeze-time":
				Time.timeScale = 0.0f;
				break;
				
			case "unfreeze":
			case "unfreeze-time":
				Time.timeScale = 1.0f;
				break;
			case "clear":
			case "clear-screen":
				ServiceLocator.UIController.ClearText();
				break;
			
			default:
				break;
				
		}
	}
	private void SetCamera(string param2)
	{
		//Debug.Log("SetCamera "+param2);
		var camera = GameObject.Find(param2).GetComponent<Camera>();
		var otherCameras =GameObject.FindGameObjectsWithTag("camera");
		foreach(var cam in otherCameras)
		{
			cam.GetComponent<Camera>().enabled = false;
		}
		camera.enabled = true;
	}

	private void ChangeVolume(string value)
	{
		ServiceLocator.ScenarioService.SetDataValue("volume",value);
		ServiceLocator.SoundController.SetVolume(float.Parse(value));
	}
	private void ChangeMouseSensitivityX(string value)
	{
		ServiceLocator.InputController.MouseSensitivity_x = float.Parse(value) / 1000.0f;
	}
	private void ChangeControllerSensitivity(string value)
	{
		ServiceLocator.InputController.controller_look_sensitivity_x = float.Parse(value);
	}
	private void ChangeMouseSensitivityY(string value)
	{
		ServiceLocator.InputController.MouseSensitivity_y = float.Parse(value)/ 1000.0f;
	}
	
	public void OnProcessedLine( string output)
	{
		
	}

	public void OnStartScenario()
	{
		if(PlayerController != null)
		{
			PlayerController.LockedPlayer=true;
			PlayerController.LockedCamera=true;
			PlayerController.PlayerBall.isKinematic = true;
			PlayerController.PlayerAnimatorAnimator.speed = 0.0f;
		}
		
		if(ServiceLocator.ConsoleService.IsConsoleVisible())
		{
			return;
		}
		
	}
	
	public void StartScenario(TextAsset TextFile)
	{
		ServiceLocator.ScenarioService.StartScenario(TextFile.text);
	}
	
	public void OnCompletedScenario()
	{
		PlayerController.LockedPlayer=false;
		PlayerController.LockedCamera=false;
		PlayerController.PlayerBall.isKinematic = false;
		PlayerController.PlayerAnimatorAnimator.speed = 1.0f;
		
		if(ServiceLocator.ConsoleService.IsConsoleVisible())
		{
			return;
		}
	}


}
