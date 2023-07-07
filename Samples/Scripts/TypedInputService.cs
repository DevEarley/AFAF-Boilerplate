using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TypedInputService : MonoBehaviour
{
    
	private InputController InputController;
	private UIController UIController;
	//private ScenarioService ScenarioService;
	private string CurrentInput = "";
	private bool ListeningForUserInput;
	private bool Locked;
	private bool KeyboardRegistered;
	public delegate void OnEnterDelegate(string input);
	public OnEnterDelegate OnEnter;
	void LateUpdate()
	{
		if(InputController.InputDetected)
		{
			OnInputEvent();
		}
	}
	
	void Update()
	{
		if(Locked)
		{
			Locked = false;
		}
		
	}	
	void Start()
	{
		InputController = GameObject.FindAnyObjectByType<InputController>();
		UIController = GameObject.FindAnyObjectByType<UIController>();
		//ScenarioService = GameObject.FindAnyObjectByType<ScenarioService>();
		//InputController.Subscribers.Add(this.gameObject);
		
	}
	public void CaptureUserInput(OnEnterDelegate onEnter)
	{
		Debug.Log("CaptureUserInput");
		OnEnter = onEnter;
		ListeningForUserInput = true;
	}
	private void ProcessLine()
	{
		ListeningForUserInput = false;
		OnEnter(CurrentInput);
		//KeyboardRegistered = false;
		//	InputController.keyboard.onTextInput -= OnTextInput;
		//this.enabled = false;
	}
	private void OnInputEvent()
	{
		if(InputController.keyboard == null) return;
		if(KeyboardRegistered == false)
		{
			//Debug.Log("KeyboardRegistered");
			KeyboardRegistered = true;
			InputController.keyboard.onTextInput +=OnTextInput;
		}
#if UNITY_WEBGL && !UNITY_EDITOR
		if(ListeningForUserInput==false)return;
		if(Locked == false)
		{
			Locked = true;
			if(InputController.keyboard.backspaceKey.wasPressedThisFrame )
			{
				//Debug.Log("OnInputEvent | BACKSPACE");
				if(CurrentInput.Length>0)
				{
					CurrentInput = CurrentInput.Substring(0,CurrentInput.Length-1);	
					UIController.SpriteFontPanel.Render(CurrentInput);
					return;
				}
			}
		else if(InputController.keyboard.enterKey.wasPressedThisFrame && CurrentInput!="" )
			{
				//Debug.Log("OnInputEvent | ENTER");
				ProcessLine();
			}
			else
			{
					UIController.SpriteFontPanel.Render(CurrentInput);
			}
		}
	
#endif
		
	}

	private void OnTextInput(char nextCharacter)
	{
		//Debug.Log("Console - OnTextInput | "+nextCharacter);
		if(ListeningForUserInput == false) return;
		
		if(nextCharacter == '`')
		{
			return;
		}
#if UNITY_WEBGL && !UNITY_EDITOR
		//Debug.Log("WEBGL");
		CurrentInput += nextCharacter;
		
#elif UNITY_EDITOR || UNITY_STANDALONE
		if( InputController.keyboard.backspaceKey.value > 0)
		{
			//Debug.Log("CONSOLE _ BACKSPACE");
			if(CurrentInput.Length>0)
			{
				CurrentInput = CurrentInput.Substring(0,CurrentInput.Length-1);
				//Debug.Log("OnTextInput | BACKSPACE | Render");
				UIController.SpriteFontPanel.Render(CurrentInput);
				return;
			}
		}
		
		if(CurrentInput!="" && ( nextCharacter == '\n' || nextCharacter == '\r' || InputController.keyboard.enterKey.wasPressedThisFrame))
		{
			
				ProcessLine();
		}
		else
		{
			Debug.Log("CurrentInput += nextCharacter II");
			
			CurrentInput += nextCharacter;
		}
	
		UIController.SpriteFontPanel.Render(CurrentInput);
#endif
		
	}
}
