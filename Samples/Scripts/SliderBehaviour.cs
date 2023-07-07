using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderBehaviour : MonoBehaviour
{
	[HideInInspector]
	public GameObject SliderParent;
	private ButtonBehaviour ButtonLeft;
	private ButtonBehaviour ButtonRight;
	private ButtonBehaviour ButtonDone;
	[HideInInspector]
	public int SliderCursorIndex;
	private static float ButtonDelayTime = 0.3f;
	private bool ButtonLock = false;
	private UIController UIController;
	private InputController InputController;
	private ScenarioService ScenarioService;
	private bool ShowingSlider = false;    
	private float CurrentValue = 0.5f;
	private float MaxValue = 1.0f;
	private float MinValue = 0.0f;
	private float RailWidth = 5.0f;
	public bool HasDoneButton = false;

	void LateUpdate()
	{
		if(InputController.InputDetected)
		{
			OnInputEvent();
		}
	}
	
	private void Start()
	{
		InputController = GameObject.FindAnyObjectByType<InputController>();
		ScenarioService = GameObject.FindAnyObjectByType<ScenarioService>();
		UIController = GameObject.FindAnyObjectByType<UIController>();
	}

	private IEnumerator ButtonPressed()
	{
		if(InputController.MovementInputBinaryVector.x>0 || InputController.DPadVector.x>0 || InputController.MovementInputBinaryVector.y>0|| InputController.DPadVector.y>0 )
		{
			ButtonDone.ButtonHovering=false;
			ButtonRight.ButtonHovering=true;
			ButtonRight.ButtonAnimator.Play("ButtonHover");
			ButtonLeft.ButtonHovering=false;
		}
		else if(InputController.MovementInputBinaryVector.x<0|| InputController.DPadVector.x<0 )
		{
			ButtonDone.ButtonHovering=false;
			ButtonRight.ButtonHovering=false;
			ButtonLeft.ButtonHovering=true;
			ButtonLeft.ButtonAnimator.Play("ButtonHover");
		}
		else if(InputController.MovementInputBinaryVector.y<0|| InputController.DPadVector.y<0 )
		{
			ButtonDone.ButtonHovering=true;
			ButtonDone.ButtonAnimator.Play("ButtonHover");
			ButtonRight.ButtonHovering=false;
			ButtonLeft.ButtonHovering=false;
		}
		ButtonLock = true;
		yield return new WaitForSeconds(ButtonDelayTime);
		ButtonLock = false;
	}
	
	public void OnInputEvent()
	{
		if(ButtonLock || ShowingSlider == false)return;
		StartCoroutine(ButtonPressed());
	}
	
	public void OnLeftSliderButtonHover(int optionId)
	{
		
			ButtonDone.ButtonHovering=false;
			ButtonRight.ButtonHovering=false;
			ButtonLeft.ButtonHovering=true;
			ButtonLeft.ButtonAnimator.Play("ButtonHover");
	
	}
	
	public void OnRightSliderButtonHover(int optionId)
	{
	
			ButtonDone.ButtonHovering=false;
			ButtonLeft.ButtonHovering=false;
			ButtonRight.ButtonHovering=true;
			ButtonRight.ButtonAnimator.Play("ButtonHover");
	
	}
	
	public void OnDoneButtonHover(int optionId)
	{
		
			ButtonDone.ButtonHovering=true;
			ButtonDone.ButtonAnimator.Play("ButtonHover");
			ButtonLeft.ButtonHovering=false;
			ButtonRight.ButtonHovering=false;
		
	}
	public void OnDoneButtonDown(int optionId)
	{
		ScenarioService.UserAdjustedTheValue(CurrentValue);
	}	
	
	public void OnLeftSliderButtonDown(int optionId)
	{
	
			ButtonDone.ButtonHovering=false;
			ButtonRight.ButtonHovering=false;
			ButtonLeft.ButtonHovering=false;
			ButtonLeft.ButtonAnimator.Play("ButtonDown");
	
	}
	public void OnRightSliderButtonDown(int optionId)
	{
		
			ButtonDone.ButtonHovering=false;
			ButtonLeft.ButtonHovering=false;
			ButtonRight.ButtonHovering=false;
			ButtonRight.ButtonAnimator.Play("ButtonDown");
		
	}

	public void ClearSliderButtons()
	{
		ShowingSlider = false;
		ButtonLeft.gameObject.SetActive(false);
		ButtonRight.gameObject.SetActive(false);
		ButtonDone.gameObject.SetActive(false);
	}
}
