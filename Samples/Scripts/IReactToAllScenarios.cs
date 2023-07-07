using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IReactToAllScenarios 
{
	public void OnStartScenario();
	public void OnCompletedScenario();
	public void OnProcessedLine(string output);
	public void OnScenarioCall(string param1, string param2);
}
