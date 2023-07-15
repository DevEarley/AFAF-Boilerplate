using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SampleTitleController : MonoBehaviour, IReactToAllScenarios
{
	//public GameObject SettingsUI;
	public GameObject MainMenuUI;
	private InputController InputController;
	private DataRepository DataRepository;
	// Awake is called when the script instance is being loaded.
	protected void Awake()
	{
		DataRepository =GameObject.FindAnyObjectByType<DataRepository>();
		//SettingsController =GameObject.FindAnyObjectByType<SettingsController>();
		//SettingsController.BehaviourThatGoesBack = this;
		InputController = gameObject.GetComponent<InputController>();
		
	}
	
	
	
	public void OnStartScenario(){}
	public void OnCompletedScenario(){}
	public void OnScenarioCall(string param1, string param2)
	{
		//Debug.Log("Title Controller");
		switch (param1)
		{
	
		case "animate":
			GameUtility.AnimateObject(param2);
			break;
		case "new-game":
			GameUtility.LoadScene("map");
			break;
		case "quit":
			Application.Quit();
			break;
		case "credits":
			ShowCredits();
			break;
		case "load-slot":
			LoadData(param2);
			break;
		default:
			break;
		}
	}
	
	private void LoadData(string loadSlot)
	{
		int loadSlotNumber = int.Parse(loadSlot);
		DataRepository.SetLoadSlot(int.Parse(loadSlot));
		DataRepository.LoadGameFromCurrentLoadSlot();
	}
	private void NewGame()
	{
		
		//DataRepository.SetLoadSlot(1);
		//DataRepository.LoadGameFromCurrentLoadSlot();
	}
	private void ShowMainMenu()
	{
		MainMenuUI.SetActive(true);
		gameObject.GetComponent<ScenarioBehaviour>().StartScenario();
	}
	
	public void OnProcessedLine( string output)
	{
		
	}
	
	private void ShowCredits()
	{
		SceneManager.LoadScene("credits");
	}
	
	private void ChangeSettings()
	{
		//save to data-repo
	}
}
