using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseController : MonoBehaviour
{
	public TextAsset PauseScenario;
	private InputController InputController;
	private ScenarioService ScenarioService;
	private bool ShowingPause = false;
    void Start()
    {
	    InputController = FindObjectOfType<InputController>();
	    ScenarioService = FindObjectOfType<ScenarioService>();
    }

    // Update is called once per frame
    void Update()
    {
	    if(InputController.WasPausePressed )
	    {
	    	ShowingPause = !ShowingPause;
	    	if(ShowingPause)
	    	{
	    		ScenarioService.StartScenario(PauseScenario.text);
	    	}
	    }
	    
    }
}
