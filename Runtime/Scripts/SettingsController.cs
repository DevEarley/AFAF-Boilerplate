using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsController : MonoBehaviour
{
	[HideInInspector]
	public float volume;
	private float volumeMin = 0.0f;
	private float volumeMax = 1.0f;
	private float volumeDelta = 0.1f;
	
	[HideInInspector]
	public float mx; 
	private float mxMin = 1.0f; 
	private float mxMax = 500.0f; 
	private float mxDelta = 5.0f; 
	
	[HideInInspector]
	public string playerName = "";
	private bool IsShowing;
	
	private int SettingsCursorIndex;
	private int SettingsButtonCount = 6;
	private bool ButtonLock = false;
	private static float ButtonDelayTime = 0.3f;
	private SliderBehaviour VolumeSlider;
	private SliderBehaviour MXSlider;
	public GameObject NameInputPrefab; 
	public GameObject BackButtonPrefab; 
	private GameObject NameInput; 
	private GameObject BackButton;	
	private UIController UIController;
	private SliderService SliderService;
	private ServiceLocator ServiceLocator;
	private LootLockerService LootLockerService;
	private InputController InputController;
	private ScenarioService ScenarioService;
	private MusicController MusicController;
	private TypedInputService TypedInputService;
	public delegate void OnBackDelegate();
	public OnBackDelegate OnBack;
	
	private void Start()
	{
		MusicController = GameObject.FindAnyObjectByType<MusicController>();
		LootLockerService = GameObject.FindAnyObjectByType<LootLockerService>();
		ServiceLocator = GameObject.FindAnyObjectByType<ServiceLocator>();
		InputController = GameObject.FindAnyObjectByType<InputController>();
		ScenarioService = GameObject.FindAnyObjectByType<ScenarioService>();
		UIController = GameObject.FindAnyObjectByType<UIController>();
		TypedInputService = GameObject.FindAnyObjectByType<TypedInputService>();
	}
	
	public void OnDataLoaded()
	{
		if(ServiceLocator.DataRepository.gameData.Slots[0].volume == null || ServiceLocator.DataRepository.gameData.Slots[0].volume == "")return;
		volume = float.Parse(ServiceLocator.DataRepository.gameData.Slots[0].volume);
		mx =  float.Parse(ServiceLocator.DataRepository.gameData.Slots[0].mouseSensitivity);
		playerName = ServiceLocator.DataRepository.gameData.Slots[0].name;
		if(playerName!="")
		{
			LootLockerService.SetName(playerName);
		}
		InputController.MouseSensitivity_x = mx;
		MusicController.volume = volume;
	}
	
	public void ShowSettings(OnBackDelegate onBack)
	{
		OnBack = onBack;
		if(VolumeSlider == null)
		{
			VolumeSlider = SliderService.CreateSliderButtons(volumeMin,volumeMax,volume,"Volume");
		}	
		if(MXSlider == null)
		{
			MXSlider = SliderService.CreateSliderButtons(mxMin,mxMax,mx,"Mouse Sensitivity");
			
		}	
		if(NameInput == null)
		{
			NameInput = GameObject.Instantiate(NameInputPrefab);
		}	
		if(BackButton == null)
		{
			BackButton = GameObject.Instantiate(BackButtonPrefab);
			
		}	
	}
	
	public void OnBackButton_SaveAndQuit()
	{
		ServiceLocator.DataRepository.UpdateSettingsAndSave(this);
		OnBack();
	}
	
	private IEnumerator CursorMoved()
	{
		if(InputController.LookInputVector.magnitude!=0){}
		
		else
		{
			if(InputController.MovementInputBinaryVector.y>0 || InputController.DPadVector.y>0 )
			{
				SettingsCursorIndex --;
			}
			else if(InputController.MovementInputBinaryVector.y<0|| InputController.DPadVector.y<0 )
			{
				SettingsCursorIndex ++;
			}
			else if(InputController.MovementInputBinaryVector.x>0.5f|| InputController.DPadVector.x>0.0f)
			{
				SettingsCursorIndex ++;
			}
			else if(InputController.MovementInputBinaryVector.x<-0.5f|| InputController.DPadVector.x<0.0f)
			{
				SettingsCursorIndex --;
			}
			if(SettingsCursorIndex>SettingsButtonCount-1)
			{
				SettingsCursorIndex = 0;
			}
			if(SettingsCursorIndex<0)
			{
				SettingsCursorIndex=SettingsButtonCount-1;
			}
		
			ButtonLock = true;
			yield return new WaitForSeconds(ButtonDelayTime);
			ButtonLock = false;
		}
	}
	void LateUpdate()
	{
		if(InputController.InputDetected && IsShowing)
		{
			OnInputEvent();
		}
	}	
	public void OnInputEvent()
	{
		if(ButtonLock)return;
		if(InputController.IsAnyButtonDown())
		{
			switch( (SettingIndexes)SettingsCursorIndex)
			{
			case SettingIndexes.MouseSensitivity_Less:
				mx -= mxDelta;
				mx=Mathf.Clamp(mx,mxMin,mxMax);
				break;
			case SettingIndexes.MouseSensitivity_More:
				mx += mxDelta;
				mx=Mathf.Clamp(mx,mxMin,mxMax);
				break;
			case SettingIndexes.Volume_Less:
				volume -= volumeDelta;
				volume=Mathf.Clamp(volume,volumeMin,volumeMax);
				break;
			case SettingIndexes.Volume_More:
				volume += volumeDelta;
				volume=Mathf.Clamp(volume,volumeMin,volumeMax);
			
				break;
			case SettingIndexes.Name:
				Debug.Log("case SettingIndexes.Name: CaptureUserInput");
					TypedInputService.CaptureUserInput(OnCapturedName);
				
				break;
			case SettingIndexes.Done:
				ServiceLocator.DataRepository.UpdateSettingsAndSave(this);
				IsShowing = false;
				ScenarioService.Continue();
				break;
			}
		}
		else
		{
			StartCoroutine(CursorMoved());
		}
	}
	public void OnCapturedName(string name)
	{
		LootLockerService.SetName(name);
		playerName = name;
	}
}

public enum SettingIndexes
{
	MouseSensitivity_Less  =0,
	MouseSensitivity_More =1,
	Volume_Less=2,
	Volume_More=3,
	Name=4,
	Done=5
}
