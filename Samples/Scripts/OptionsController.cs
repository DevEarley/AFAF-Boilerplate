using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class OptionPickerService : MonoBehaviour
{
	public GameObject ButtonPrefab;
	public GameObject UIPanel;
	[HideInInspector]
	public int OptionsCursorIndex;
	private static float ButtonDelayTime = 0.3f;
	private bool ButtonLock = false;
	private UIController UIController;
	private InputController InputController;
	private ScenarioService ScenarioService;
	private bool ShowingOptions = false;    
	private List<ButtonBehaviour> OptionButtons = new List<ButtonBehaviour>();
	private List<string> CurrentOptions = new List<string>();
	
	private void Awake()
	{
		
		
	}
	private void Start()
	{
		InputController = GameObject.FindAnyObjectByType<InputController>();
		ScenarioService = GameObject.FindAnyObjectByType<ScenarioService>();
		UIController = GameObject.FindAnyObjectByType<UIController>();
	
	}
	void LateUpdate()
	{
		if(InputController.InputDetected)
		{
			OnInputEvent();
		}
	}
	
	private IEnumerator ButtonPressed()
	{
		if(InputController.LookInputVector.magnitude!=0){}
		else if(OptionButtons.Count == 0 || OptionsCursorIndex <0 || OptionsCursorIndex>OptionButtons.Count-1){
			OptionsCursorIndex =0;
		}
		else
		{
			if(InputController.MovementInputBinaryVector.y>0 || InputController.DPadVector.y>0 )
			{
				OptionsCursorIndex --;
			}
			else if(InputController.MovementInputBinaryVector.y<0|| InputController.DPadVector.y<0 )
			{
				OptionsCursorIndex ++;
			}
			else if(InputController.MovementInputBinaryVector.x>0.5f|| InputController.DPadVector.x>0.5f)
			{
				OptionsCursorIndex ++;
			}
			else if(InputController.MovementInputBinaryVector.x<-0.5f|| InputController.DPadVector.x<-0.5f)
			{
				OptionsCursorIndex --;
			}
			if(OptionsCursorIndex>OptionButtons.Count-1)
			{
				OptionsCursorIndex = 0;
			}
			if(OptionsCursorIndex<0)
			{
				OptionsCursorIndex=OptionButtons.Count-1;
			}
		
			OptionButtons.ForEach(b=>{
				b.ButtonHovering=false;
			});
		
			OptionButtons[OptionsCursorIndex].ButtonHovering = true;
			OptionButtons[OptionsCursorIndex].ButtonAnimator.Play("ButtonHover");
		
			ButtonLock = true;
			yield return new WaitForSeconds(ButtonDelayTime);
			ButtonLock = false;
			
		}
	}
	public void OnInputEvent()
	{
		if(ButtonLock)return;
		StartCoroutine(ButtonPressed());
	}
	
	public void ShowOptions_ForScenario(string[] options)
	{
		if(ShowingOptions)return;
		ClearOptionButtons();
		Init_Options(options);
		CreateOptionButtons_ForScenario(options);
		if(OptionButtons.Count == 0 || OptionsCursorIndex <0 || OptionsCursorIndex>OptionButtons.Count-1){
			OptionsCursorIndex =0; 
			return;	
		}
		OptionButtons[0].ButtonHovering = true;
		OptionButtons[0].ButtonAnimator.Play("ButtonHover");
	}
	private void OnOptionButtonHover_ForScenaio(int optionIndex)
	{
		foreach(var button in OptionButtons.Where(b=>b.OptionIndex!=optionIndex))
		{
			button.StopHovering();
		}
	}
	private void OnOptionButtonDown_ForScenario(int optionIndex)
	{
		if(CurrentOptions.Count<optionIndex){
			//Debug.Log("button is out of range. no options for index "+optionIndex);
		}
		var pickedOption =CurrentOptions[optionIndex];
		ClearOptionButtons();
		ScenarioService.UserPickedOption(pickedOption);
	}

	private void CreateOptionButtons_ForScenario(string[] options)
	{
		ClearOptionButtons();
		ShowingOptions = true;
		var optionIndex = 0;
		foreach(string optionText in options)
		{
			var newButtonBehaviour = CreateNewButton();
			newButtonBehaviour.RegisterButton(optionText,optionIndex,OnOptionButtonDown_ForScenario, OnOptionButtonHover_ForScenaio);
			optionIndex++;
		}
		
		Resize();
	}

	private void Init_Options(string[] options)
	{
		CurrentOptions = options.Select(x=>x.TrimStart().TrimEnd()).ToList();
		OptionsCursorIndex = 0;
		ShowingOptions = true;
	}
	
	private ButtonBehaviour CreateNewButton()
	{
		var newButton = GameObject.Instantiate(ButtonPrefab);
		newButton.transform.parent = UIPanel.gameObject.transform;
		OptionButtons.Add(newButton.GetComponent<ButtonBehaviour>());
		var newButtonBehaviour = newButton.GetComponent<ButtonBehaviour>();
		return newButtonBehaviour;
	}
		
	private void ScaleOptions()
	{
		Resize();
	}
	
	public void ClearOptionButtons()
	{
		ShowingOptions = false;
		OptionButtons.ForEach(g=>GameObject.Destroy(g.gameObject));
		OptionButtons.Clear();
	}
	
	private static Vector2 TallSize= new Vector2(3.5f, 3.0f);
	
	private static Vector3 TallOffset = new Vector3(0.0f,  -0.09f, -0.01f);
	
	private static Vector3 TallTextOffset = new Vector3(-0.08f,   0.04f, 0.0f);

	private static Vector3 TallButtonNorthOffset = new Vector3(0.0f,0.0f,10.0f);
	
	private static Vector3 TallButtonEastOffset = new Vector3(0.0f,-3.0f,10.0f);

	private static Vector3 TallButtonWestOffset = new Vector3(0.0f,-1.5f,10.0f);
	
	private static Vector3 TallButtonSouthOffset = new Vector3(0.0f,-4.5f,10.0f);
	

	
	
	private void SetButton(int ButtonOrientation,Vector3 offset)
	{
		if(OptionButtons.Count<= (int)ButtonOrientation)return;
		OptionButtons[(int)ButtonOrientation].transform.localPosition = offset;
	}

	public void Resize()
	{

		var newVector =  new Vector3(TallSize.x,TallSize.y,1.0f);
		SetButton(0,TallButtonNorthOffset);
		SetButton(1,TallButtonWestOffset);
		SetButton(2,TallButtonEastOffset);
		SetButton(3,TallButtonSouthOffset);

		//}
	}
}
