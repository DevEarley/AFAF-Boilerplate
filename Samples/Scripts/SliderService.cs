using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderService : MonoBehaviour
{
	//public Sprite RailSprite;
	//public Sprite CursorSprite;
	//[HideInInspector]
	//public GameObject Cursor;
	//public static float CursorScaler = 2.5f;
	//public GameObject ButtonPrefabLeft;
	//public GameObject ButtonPrefabRight;
	//public GameObject ButtonPrefabDone;
	//public GameObject UIPanel;
	//private UIController UIController;
	//private InputController InputController;
	//private ScenarioService ScenarioService;
	//public delegate void CallbackDelegate(int optionIndex);
	//public CallbackDelegate ButtonCallback;
	
	//public delegate void HoverDelegate(int optionIndex);
	//public HoverDelegate ButtonHover;
	

	//private void Start()
	//{
	//	InputController = GameObject.FindAnyObjectByType<InputController>();
	//	ScenarioService = GameObject.FindAnyObjectByType<ScenarioService>();
	//	UIController = GameObject.FindAnyObjectByType<UIController>();
	//}
	
	//public void UpdateValue(float value)
	//{
	//	Cursor.transform.localPosition = new Vector3(value*CursorScaler,0,0);
	//}
	
	//public SliderBehaviour CreateSliderButtons(float minValue, float maxValue, float currentValue, string SliderName)
	//{
	//	var SliderParent = new GameObject();
	//	SliderParent.name = "SliderParent_"+SliderName;
	//	var SliderParentBehaviour = SliderParent.AddComponent<SliderBehaviour>();
	//	var railRenderer = SliderParent.AddComponent<SpriteRenderer>();
	//	railRenderer.sprite = RailSprite;
	//	Cursor = new GameObject();
	//	Cursor.transform.parent = SliderParent.transform;
	//	var cursorRenderer = Cursor.AddComponent<SpriteRenderer>();
	//	cursorRenderer.sprite = CursorSprite;
	//	var leftButton = GameObject.Instantiate(ButtonPrefabLeft);
	//	leftButton.transform.parent = UIPanel.gameObject.transform;
	//	leftButton.transform.parent = SliderParent.transform;
	//	var leftButtonBehaviour = leftButton.GetComponent<ButtonBehaviour>();
	//	leftButtonBehaviour.RegisterButton("-",0,SliderParentBehaviour.OnLeftSliderButtonDown, SliderParentBehaviour.OnLeftSliderButtonHover);
	//	leftButton.gameObject.SetActive(true);
	//	var rightButton = GameObject.Instantiate(ButtonPrefabRight);
	//	rightButton.transform.parent = UIPanel.gameObject.transform;
	//	rightButton.transform.parent = SliderParent.transform;
	//	var rightButtonBehaviour = rightButton.GetComponent<ButtonBehaviour>();
	//	rightButtonBehaviour.RegisterButton("+",1,SliderParentBehaviour.OnRightSliderButtonDown, SliderParentBehaviour.OnRightSliderButtonHover);
	//	rightButton.gameObject.SetActive(true);
	//	var doneButton = GameObject.Instantiate(ButtonPrefabDone);
	//	doneButton.transform.parent = UIPanel.gameObject.transform;
	//	doneButton.transform.parent = SliderParent.transform;
	//	var doneButtonBehaviour = doneButton.GetComponent<ButtonBehaviour>();
	//	doneButtonBehaviour.RegisterButton("DONE",2,SliderParentBehaviour.OnDoneButtonDown, SliderParentBehaviour.OnDoneButtonHover);
	//	doneButton.gameObject.SetActive(true);
	//	return SliderParentBehaviour;
	//}

}
