using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServiceLocator : MonoBehaviour
{
	[HideInInspector]
	public ConsoleService ConsoleService;
	[HideInInspector]
	public ConsoleScenarioController ConsoleScenarioController;
	[HideInInspector]
	public InputController InputController;
    [HideInInspector]
	public DataRepository DataRepository;
	[HideInInspector]
	public UIController UIController;
	[HideInInspector]
	public SoundController SoundController;
	[HideInInspector]
	public MusicController MusicController;
	[HideInInspector]
	public SettingsController SettingsController;
	[HideInInspector]
	public CameraController CameraController;
	[HideInInspector]
	public RNGController RNGController ;
    [HideInInspector]
	public ScenarioService ScenarioService;
	[HideInInspector]
	public OptionPickerService OptionPickerService;
	[HideInInspector]
	public ScenarioRepository ScenarioRepository;
	[HideInInspector]
	public LootLockerService LootLockerService;

    private void Awake()
    {
	    ConsoleScenarioController = FindObjectOfType<ConsoleScenarioController>();
	    ConsoleService = FindObjectOfType<ConsoleService>();
	    OptionPickerService = FindObjectOfType<OptionPickerService>();
	    ScenarioRepository = FindObjectOfType<ScenarioRepository>();
	    RNGController = FindObjectOfType<RNGController>();
	    CameraController = FindObjectOfType<CameraController>();
	    InputController = FindObjectOfType<InputController>();
	    MusicController = FindObjectOfType<MusicController>();
	    SoundController = FindObjectOfType<SoundController>();
        DataRepository = FindObjectOfType<DataRepository>();
	    UIController = FindObjectOfType<UIController>();
	    ScenarioService = FindObjectOfType<ScenarioService>();
	    LootLockerService = FindObjectOfType<LootLockerService>();
    }
}
