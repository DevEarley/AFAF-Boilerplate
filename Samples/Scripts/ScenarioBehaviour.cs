using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class ScenarioBehaviour : MonoBehaviour
{
	[HideInInspector]
	public List<GameObject> Subscribers;
	public TextAsset TextFile;
	public bool RunOnStart;
	public bool RunOnTriggerEnter;
	public InputController InputController;
    private int player_layer_index = 3;
	public bool RequireActionPress = false;
	private bool ReadyForInteraction = false;
	public bool OnceAndDone = false;
	public bool Locked = false;
	
    private void Awake()
	{
		InputController = FindObjectOfType<InputController>();
	    if(Subscribers == null)
	    Subscribers = new List<GameObject>();
    }
    
	void Start()
	{
		if(TextFile == null)return;
		if(RunOnStart ==false)return;
		StartScenario();
	}
	
	void Update()
	{
		if(TextFile == null)return;
		
		if(RequireActionPress == false)return;
		var buttonWasPressed = InputController.WasAction2Released || InputController.WasAction1Released;
		if(ReadyForInteraction && buttonWasPressed)
		{
			ReadyForInteraction = false;
			StartScenario();
		}
	}
	
	private void OnTriggerEnter(Collider other)
	{ 
		if(Locked )return;
		if(TextFile == null)return;
		if(RunOnTriggerEnter ==false)return;
		if (other.gameObject.layer != player_layer_index) return;
		if(RequireActionPress)
		{
			ReadyForInteraction = true;
		}
		else{
			StartScenario();
		}
		if(OnceAndDone)
		{
			Locked = true;
		}
	}
	
	private void OnTriggerExit(Collider other)
	{ 
		if(Locked)return;
		
		if(TextFile == null)return;
		
		if (other.gameObject.layer != player_layer_index) return;
		ReadyForInteraction = false;
	}

    public void StartScenario()
	{
		if(TextFile == null)return;
		Subscribers.ForEach(x=>x.SendMessage("OnStartScenario",SendMessageOptions.DontRequireReceiver));
		var ScenarioService = FindObjectOfType<ScenarioService>();
		ScenarioService.StartScenario(TextFile.text);
    }

    private void OnCompletedScenario()
	{	
		Subscribers.ForEach(x=>x.SendMessage("OnCompletedScenario",SendMessageOptions.DontRequireReceiver));
	
    }
}
