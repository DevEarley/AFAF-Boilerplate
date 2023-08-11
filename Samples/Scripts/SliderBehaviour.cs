using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

public class SliderBehaviour : MonoBehaviour
{
	private string RailText ="_______________";
	public SpriteFontPanel SpriteFontPanel;
	public GameObject ButtonLeftPrefab;
	public GameObject ButtonRightPrefab;
	private ButtonBehaviour ButtonLeft;
	private ButtonBehaviour ButtonRight;
	[HideInInspector]
	public int SliderCursorIndex;
	private static float ButtonDelayTime = 0.3f;
	private bool ButtonLock = false;
	private InputController InputController;
	private bool ShowingSlider = false;    
	private float CurrentValue = 0.5f;
	private float MaxValue = 1.0f;
	private float MinValue = 0.0f;
	private float RailWidth = 5.0f;


	public delegate void SliderDelegate(float value);
	public SliderDelegate OnSliderValueChanged;
	

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
		RenderSlider();
		
	}
	public void IncrementUp()
	{
		CurrentValue+=0.1f;
		if(CurrentValue>1.0f)
		{
			CurrentValue = 1.0f;
		}
		OnSliderValueChanged(CurrentValue);
	}
	
	public void IncrementDown()
	{
		
		CurrentValue-=0.1f;
		if(CurrentValue<0.0f)
		{
			CurrentValue = 0.0f;
		}
		OnSliderValueChanged(CurrentValue);
		
	}
	
	private IEnumerator ButtonPressed()
	{
		if(InputController.MovementInputBinaryVector.x>0 || InputController.DPadVector.x>0 || InputController.MovementInputBinaryVector.y>0|| InputController.DPadVector.y>0 )
		{
		
			ButtonRight.ButtonHovering=true;
			ButtonRight.ButtonAnimator.Play("ButtonHover");
			ButtonLeft.ButtonHovering=false;
		}
		else if(InputController.MovementInputBinaryVector.x<0|| InputController.DPadVector.x<0 )
		{
			
			ButtonRight.ButtonHovering=false;
			ButtonLeft.ButtonHovering=true;
			ButtonLeft.ButtonAnimator.Play("ButtonHover");
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
			ButtonRight.ButtonHovering=false;
			ButtonLeft.ButtonHovering=true;
			ButtonLeft.ButtonAnimator.Play("ButtonHover");
	}
	
	public void OnRightSliderButtonHover(int optionId)
	{
			ButtonLeft.ButtonHovering=false;
			ButtonRight.ButtonHovering=true;
			ButtonRight.ButtonAnimator.Play("ButtonHover");
	}
	
	public void OnLeftSliderButtonDown(int optionId)
	{
		IncrementDown();
		ButtonRight.ButtonHovering=false;
		ButtonLeft.ButtonHovering=false;
		RailText = BuildStringForRail(CurrentValue);
		SpriteFontPanel.Render(BuildStringForRail(CurrentValue));
	}
	
	public void OnRightSliderButtonDown(int optionId)
	{
		IncrementUp();
		ButtonLeft.ButtonHovering=false;
		ButtonRight.ButtonHovering=false;
		RailText = BuildStringForRail(CurrentValue);
		SpriteFontPanel.Render(BuildStringForRail(CurrentValue));		
	}
	
	public string BuildStringForRail(float percentage)
	{
		int RoundedPercent = Mathf.RoundToInt(percentage*100);
		int RoundToNearestTen = RoundedPercent-RoundedPercent%10;
		Debug.Log("BuildStringForRail | "+RoundToNearestTen);
		string rail = "";
		for(var i = 0; i<=100; i+=10)
		{
			if(i == RoundToNearestTen)
			{
				rail += "|";
			}
			else
			{
				rail += "_";
			}
		}
		return rail+" "+ RoundToNearestTen;
	} 
	
	public void RegisterSlider(SliderDelegate onSliderValueChanged)
	{
		OnSliderValueChanged = onSliderValueChanged;
	}
	
	public void RenderSlider()
	{
		if(ButtonLeft==null)
			ButtonLeft = Instantiate(ButtonLeftPrefab).GetComponent<ButtonBehaviour>();
		ButtonLeft.RegisterButton(0,OnLeftSliderButtonDown,OnLeftSliderButtonHover);
		ButtonLeft.gameObject.transform.parent = this.transform;
		ButtonLeft.gameObject.transform.localPosition = new Vector3(-3.0f,0,0);
		
		if(ButtonRight==null)
			ButtonRight = Instantiate(ButtonRightPrefab).GetComponent<ButtonBehaviour>();
		ButtonRight.RegisterButton(0,OnRightSliderButtonDown,OnRightSliderButtonHover);
		ButtonRight.gameObject.transform.parent = this.transform;
		ButtonRight.gameObject.transform.localPosition = new Vector3(3.0f,0,0);
		
		SpriteFontPanel.Render(BuildStringForRail(CurrentValue));
	}
}

#if UNITY_EDITOR

[CustomEditor(typeof(SliderBehaviour))]
[CanEditMultipleObjects]
 public class SliderBehaviourButton : Editor {
    
	
	 public override void OnInspectorGUI()
	 {
		 DrawDefaultInspector();

		 SliderBehaviour sliderBehaviour = (SliderBehaviour)target;
		 if (GUILayout.Button("Render Slider"))
		 {
			 
			 sliderBehaviour.RenderSlider();
		 }
		
	 }
 }
#endif