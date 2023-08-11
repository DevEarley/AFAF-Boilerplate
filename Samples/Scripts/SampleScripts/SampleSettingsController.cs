using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleSettingsController : MonoBehaviour
{
	[HideInInspector]
	public float volume;
	private float volumeMin = 0.0f;
	private float volumeMax = 1.0f;
	private float volumeDelta = 0.1f;
	
	[HideInInspector]
	public float mx; 
	[HideInInspector]
	public float my; 
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
	public SliderBehaviour VolumeSlider;
	public SliderBehaviour MXSlider;
	public SliderBehaviour MYSlider;
	private GameObject NameInput; 
	public ButtonBehaviour BackButton;	
	private UIController UIController;
	private LootLockerService LootLockerService;
	private DataRepository DataRepository;
	private InputController InputController;
	private ScenarioService ScenarioService;
	private TypedInputService TypedInputService;
	
	public delegate void OnBackDelegate();
	public OnBackDelegate OnBack;
	public void RegisterSettings (OnBackDelegate onBack)
	{
		OnBack = onBack;
	}
	private void Start()
	{
		LootLockerService = GameObject.FindAnyObjectByType<LootLockerService>();
		DataRepository = GameObject.FindAnyObjectByType<DataRepository>();
		
		InputController = GameObject.FindAnyObjectByType<InputController>();
		ScenarioService = GameObject.FindAnyObjectByType<ScenarioService>();
		UIController = GameObject.FindAnyObjectByType<UIController>();
		TypedInputService = GameObject.FindAnyObjectByType<TypedInputService>();
		
		VolumeSlider.RegisterSlider(Volume_OnValueChanged);
		MXSlider.RegisterSlider(MX_OnValueChanged);
		MYSlider.RegisterSlider(MY_OnValueChanged);
		BackButton.RegisterButton(0,OnBackButton_Press,OnBackButton_Hover);
		
	}
	public void OnBackButton_Press(int index)
	{
		DataRepository.WriteFile();
		OnBack();
	}	
	public void OnBackButton_Hover(int index)
	{
		
	}
	public void OnDataLoaded()
	{
		if(DataRepository.gameData.Slots[0].volume == null || DataRepository.gameData.Slots[0].volume == "")return;
		volume = float.Parse(DataRepository.gameData.Slots[0].volume);
		mx =  float.Parse(DataRepository.gameData.Slots[0].mouseSensitivityX);
		mx =  float.Parse(DataRepository.gameData.Slots[0].mouseSensitivityY);
		playerName = DataRepository.gameData.Slots[0].name;
		if(playerName!="")
		{
			LootLockerService.SetName(playerName);
		}
		InputController.MouseSensitivity_x = mx;
		InputController.MouseSensitivity_y = my;
		//MusicController.volume = volume;
	}
	

	public void Volume_OnValueChanged(float value)
	{
		volume = value;
		DataRepository.gameData.Slots[0].volume = volume.ToString();
		
	}
	public void MX_OnValueChanged(float value)
	{
		mx = value;
		DataRepository.gameData.Slots[0].mouseSensitivityX = mx.ToString();
		InputController.MouseSensitivity_x = mx;
		
	}
	public void MY_OnValueChanged(float value)
	{
		my = value;
		DataRepository.gameData.Slots[0].mouseSensitivityY = my.ToString();
		InputController.MouseSensitivity_x = my;
	}
	
	

}
