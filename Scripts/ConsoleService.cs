using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ConsoleService : MonoBehaviour
{
	private InputController InputController;
	private SpriteFontPanel ConsoleSpriteFontPanel_Input;
	private SpriteFontPanel ConsoleSpriteFontPanel_Output;
	private GameObject ConsoleAnimator;
	public IReactToConsole ConsoleBehaviour;
	private string CurrentInput = "";
	private string CurrentOutput = "";
	private bool KeyboardRegistered = false;
	private bool ConsoleIsVisible = false;
	private int MaxLines = 18;
	//private float LineHeight = 0.1f;
	private float offset = -1.1f;
	private string LastLine = "";
	private static float ConsoleDelayTime = 0.001f;
	public float ConsoleDelayTimer = 0.0f;
	private bool Locked = false;
	public bool IsConsoleVisible(){
		return ConsoleIsVisible;
	}
	void Update()
	{
		if(Locked)
		{
			Locked = false;
		}
		if(InputController.keyboard.backquoteKey.wasPressedThisFrame )
		{	
			////Debug.Log("ConsoleDelayTimer < Time.time" + ConsoleDelayTimer +" | "+Time.time);
			if(ConsoleDelayTimer < Time.time)
			{
				//Debug.Log("Console Toggle");
				ConsoleDelayTimer = Time.time + ConsoleDelayTime;
				
				if(ConsoleIsVisible)
				{
					HideConsole();
				}
				else
				{
					ShowConsole();
				}	
			}
		}
	}
    void Start()
	{
		ConsoleAnimator = GameObject.Find("ConsoleUI");
		InputController = GameObject.FindAnyObjectByType<InputController>();
		ConsoleSpriteFontPanel_Input = GameObject.Find("ConsoleSpriteFontPanel_Input").GetComponent<SpriteFontPanel>();
		ConsoleSpriteFontPanel_Output = GameObject.Find("ConsoleSpriteFontPanel_Output").GetComponent<SpriteFontPanel>();
		//	InputController.Subscribers.Add(this.gameObject);
		ConsoleAnimator.SetActive(false);
		//ConsoleAnimator.Play("hide-console");
	}
	void LateUpdate()
	{
		if(InputController.InputDetected)
		{
			OnInputEvent();
		}
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
		if(ConsoleIsVisible==false)return;
		if(Locked == false)
		{
			Locked = true;
			if(InputController.keyboard.backspaceKey.wasPressedThisFrame )
			{
				//Debug.Log("OnInputEvent | BACKSPACE");
				if(CurrentInput.Length>0)
				{
					
					CurrentInput = CurrentInput.Substring(0,CurrentInput.Length-1);
					
					ConsoleSpriteFontPanel_Input.Render(CurrentInput);
					return;
				}
			}
			else if(InputController.keyboard.enterKey.wasPressedThisFrame)
			{
					//Debug.Log("OnInputEvent | ENTER");
					ProcessLine();
			}
		else
			{
				ConsoleSpriteFontPanel_Input.Render(CurrentInput);
			}
		}
	
		////Debug.Log("OnInputEvent");
#endif
		
	}

	private void OnTextInput(char nextCharacter)
	{
		//Debug.Log("Console - OnTextInput | "+nextCharacter);
		if(ConsoleIsVisible == false) return;
		
		if(nextCharacter == '`')
		{
			return;
		}
#if UNITY_WEBGL && !UNITY_EDITOR
		//Debug.Log("WEBGL");
		CurrentInput += nextCharacter;
		
#elif UNITY_EDITOR || UNITY_STANDALONE
		if(InputController.keyboard.upArrowKey.value > 0)
		{
			CurrentInput = LastLine;
		}
		if( InputController.keyboard.backspaceKey.value > 0)
		{
			//Debug.Log("CONSOLE _ BACKSPACE");
			if(CurrentInput.Length>0)
			{
				CurrentInput = CurrentInput.Substring(0,CurrentInput.Length-1);
				//Debug.Log("OnTextInput | BACKSPACE | Render");
				ConsoleSpriteFontPanel_Input.Render(CurrentInput);
				return;
			}
		}
	
		if( nextCharacter == '\n' || nextCharacter == '\r')
		{
			//Debug.Log("CONSOLE _ ENTER");
				
			if(InputController.keyboard.shiftKey.value == 0)
			{
				ProcessLine();
			}
			else
			{
				//	Debug.Log("CurrentInput += nextCharacter I");
				CurrentInput += nextCharacter;
			}
		}
		else
		{
			//	Debug.Log("CurrentInput += nextCharacter II");
			
			CurrentInput += nextCharacter;
		}
	
	
		//Debug.Log("Console - Render | "+CurrentInput);
		ConsoleSpriteFontPanel_Input.Render(CurrentInput);
#endif
		
	}
	
	private void ProcessLine()
	{
		//Debug.Log("Console - ProcessLine | "+CurrentInput);
		if(ConsoleIsVisible==false)return;
		var line = CurrentInput;
		WriteToOutput(line);
		if(line.StartsWith(".call")==false&&line.StartsWith(".")==false)
		{
			var paramsForLine = line.Split(",");
			if(line.Contains(",")==false)
			{
				line = ".call["+line+"]";
			}
			else if(paramsForLine.Length ==2)
			{
				line = ".call["+paramsForLine[0]+"]"+paramsForLine[1];	
			}
			else
			{
				var lastParams = string.Concat(paramsForLine.TakeLast(paramsForLine.Length-1).ToArray());
				line = ".call["+paramsForLine[0]+"]"+lastParams;	
			}
		}
		
		ConsoleBehaviour.StartScenario(line);
		//CurrentOutput += '\n' + line;
		LastLine= line;
		CurrentInput = "";
		
		ConsoleSpriteFontPanel_Input.Render("");
	}
	
	private void SetOutputPosition()
	{
		var lines = ConsoleSpriteFontPanel_Output.text.Split('\n').Length ;
		ConsoleSpriteFontPanel_Output.transform.localPosition= Vector3.up* offset + Vector3.up * ConsoleSpriteFontPanel_Output.lineHeight * lines;
	}
	
	public void OnStartScenario()
	{
		//ConsoleSpriteFontPanel_Output.Render(ConsoleSpriteFontPanel_Output.text+"\nOnStartScenario\n");
	}
	
	public void WriteToOutput(string output)
	{
		//Debug.Log("WriteToOutput");
		if(ConsoleIsVisible==false)return;
		
		ConsoleSpriteFontPanel_Output.text = (ConsoleSpriteFontPanel_Output.text+"\n"+output);
		if(ConsoleSpriteFontPanel_Output.text.Split('\n').Length > MaxLines)
		{
			ConsoleSpriteFontPanel_Output.text =  string.Join('\n',ConsoleSpriteFontPanel_Output.text.Split("\n").TakeLast(MaxLines).ToArray());
		}
		ConsoleSpriteFontPanel_Output.text += "\n";
		SetOutputPosition();
		
		ConsoleSpriteFontPanel_Output.RenderText();
		CurrentInput = "";
		ConsoleSpriteFontPanel_Input.Render("");
	}
	
	
	public void ClearConsole()
	{
		//Debug.Log("ClearConsole");
		ConsoleSpriteFontPanel_Input.Reset();
		ConsoleSpriteFontPanel_Output.Reset();
	}
	
	public void HideConsole()
	{
		Debug.Log("Hide Console");
		
		ConsoleIsVisible = false;
		//ConsoleAnimator.Play("hide-console");
		ConsoleAnimator.SetActive(false);
		
		ConsoleBehaviour.OnHideConsole();
	}
	
	public void ShowConsole()
	{
		Debug.Log("Show Console");
		ConsoleIsVisible = true;
		//ConsoleAnimator.Play("show-console");
		ConsoleAnimator.SetActive(true);
		
		ConsoleBehaviour.OnShowConsole();
	}
}
