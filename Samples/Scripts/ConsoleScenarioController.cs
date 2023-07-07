using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ConsoleScenarioController : MonoBehaviour, IReactToAllScenarios, IReactToConsole
{
	private ConsoleService ConsoleService;
	private ServiceLocator ServiceLocator;
	private static string HelpText = 
		@"--Help Text--
help 
	- shows this prompt.
load-scene,<scene name> 
	- loads a scene
list-scenes
	- lists the scenes		";

	protected void Awake()
	{
		ServiceLocator = GameObject.FindAnyObjectByType<ServiceLocator>();
		ConsoleService = GameObject.FindAnyObjectByType<ConsoleService>();
		ConsoleService.ConsoleBehaviour = this;
	}
	
	public void OnShowConsole()
	{
		Time.timeScale = 0.05f;

		//ServiceLocator.PlayerController.LockInput();
	}
	
	public void OnHideConsole()
	{
		Time.timeScale = 1.0f;
		
		//ServiceLocator.PlayerController.UnlockInput();
	}
	
	public void OnStartScenario()
	{
		
	}	
	
	public void StartScenario(string line)
	{
		ServiceLocator.ScenarioService.StartScenario(line);
	}
	
	public void OnProcessedLine(string output)
	{
		ConsoleService.WriteToOutput( output);
	}
	
	public void OnCompletedScenario()
	{
		//HideConsole();
		
	}
	
	public void OnScenarioCall(string param1, string param2)
	{
		//Debug.Log("ConsoleController | OnScenarioCall");
		switch(param1)
		{

		case "clear-console":
			ClearConsole();
			break;
			
		case "hide-console":
			HideConsole();
			break;
		case "load-scene":
			HideConsole();
			break;
		case "help":
			ServiceLocator.ScenarioService.StartScenario(HelpText);
			break;
		default:
			break;
		}
	}
	
	
	private void ClearConsole()
	{
		ConsoleService.ClearConsole();
	}
	
	private void HideConsole()
	{
		ConsoleService.HideConsole();
	}
	
	private void ShowConsole()
	{
		ConsoleService.ShowConsole();
	}
}
