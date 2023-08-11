using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseController : MonoBehaviour
{
	private SampleSettingsController Settings;
	public GameObject SettingsPrefab;
	public ButtonBehaviour SettingsButton;
	public ButtonBehaviour QuitButton;
	public ButtonBehaviour ResumeButton;
	private InputController InputController;
	private ScenarioService ScenarioService;
	private bool ShowingPause = false;
	
    void Start()
    {
	    InputController = FindObjectOfType<InputController>();
	    ScenarioService = FindObjectOfType<ScenarioService>();
	    SettingsButton.RegisterButton(0,SettingsButton_Press,SettingsButton_Hover);
	    QuitButton.RegisterButton(0,QuitButton_Press,QuitButton_Hover);
	    ResumeButton.RegisterButton(0,ResumeButton_Press,ResumeButton_Hover);
    }
    
	private void ShowSettings()
	{
		if(Settings!=null)
		{
			Settings.gameObject.SetActive(true);
			return;
		}
		var settings = Instantiate(SettingsPrefab);
		Settings = settings.GetComponent<SampleSettingsController>();
		Settings.RegisterSettings(Settings_OnBack);
	}
	
	public void Settings_OnBack()
	{
		Settings.gameObject.SetActive(false);
	}
	
	public void SettingsButton_Press(int index)
	{
		HideButtons();
		ShowSettings();
	}
		
	public void QuitButton_Press(int index)
	{
		HideButtons();
		Application.Quit();
	}
	
	public void ResumeButton_Press(int index)
	{
		HideButtons();
		ShowingPause= false;
	}
	
	public void SettingsButton_Hover(int index)
	{
		
	}
	
	public void QuitButton_Hover(int index)
	{
		
	}
	
	public void ResumeButton_Hover(int index)
	{
		
	}
	
	private void ShowButtons()
	{
		SettingsButton.gameObject.SetActive(true);
		QuitButton.gameObject.SetActive(true);
		ResumeButton.gameObject.SetActive(true);
	}
	
	private void HideButtons()
	{
		Cursor.lockState = CursorLockMode.Locked;
		
		SettingsButton.gameObject.SetActive(false);
		QuitButton.gameObject.SetActive(false);
		ResumeButton.gameObject.SetActive(false);
	}
	
    void Update()
    {
	    if(InputController.WasPausePressed )
	    {
	    	ShowingPause = !ShowingPause;
	    	if(ShowingPause)
	    	{
	    		ShowButtons();
	    	}
	    }
	    
    }
}
