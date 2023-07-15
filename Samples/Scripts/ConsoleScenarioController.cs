using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ConsoleScenarioController : MonoBehaviour, IReactToAllScenarios, IReactToConsole
{
	private static bool SlowTimeOnOpen = true;
	private ConsoleService ConsoleService;
	private ScenarioService ScenarioService;
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
		ConsoleService = GameObject.FindAnyObjectByType<ConsoleService>();
		ScenarioService = GameObject.FindAnyObjectByType<ScenarioService>();
		ConsoleService.ConsoleBehaviour = this;
	}
	
	public void OnShowConsole()
	{
		if(SlowTimeOnOpen) Time.timeScale = 0.05f;
	}
	
	public void OnHideConsole()
	{
		if(SlowTimeOnOpen) Time.timeScale = 1.0f;
	}
	
	public void OnStartScenario()
	{
		
	}	
	
	public void StartScenario(string line)
	{
		ScenarioService.StartScenario(line);
	}
	
	public void OnProcessedLine(string output)
	{
		ConsoleService.WriteToOutput( output);
	}
	
	public void OnCompletedScenario()
	{
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
			ScenarioService.StartScenario(HelpText);
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
