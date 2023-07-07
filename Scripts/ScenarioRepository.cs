using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenarioRepository: MonoBehaviour
{
	public  TextAsset GetScenarioTextByName(string scenarioName)
	{
		return (TextAsset)this.GetType().GetField(scenarioName).GetValue(this);
	}

	public TextAsset SomeScenarioI;

}
