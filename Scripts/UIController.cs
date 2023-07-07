using System.Collections;
using System.Collections.Generic;
using System.Linq;
 
using UnityEngine;

public enum PauseMenuActions
{
	Continue = 0,
	Map = 1,
	Inventory = 2,
	Settings = 3,
	MainMenu = 4,
	QuitToDesktop = 5
}

public enum MainMenuActions
{
	NewGame = 0,
	ContinueGame = 1,
	Settings = 2,
	Achievements = 3,
	Credits = 4,
	QuitToDesktop = 5
}

public class UIController : MonoBehaviour, IDrawText
{
	public  List<Layout> CurrentLayout;
	
	private ServiceLocator ServiceLocator;
	public SpriteFontPanel SpriteFontPanel;
	private void Awake()
	{
		ServiceLocator = FindObjectOfType<ServiceLocator>();
	}
	// Update is called every frame, if the MonoBehaviour is enabled.
	protected void Update()
	{	
		
	}
	public void HidePanel()
	{
		SpriteFontPanel.Reset();
		//SpriteFontPanel.gameObject.SetActive(false);
	}
		
	public void ShowPanel()
	{
		//SpriteFontPanel.Reset();
		//SpriteFontPanel.gameObject.SetActive(true);
	}
	public void ClearText()
	{
		//ShowPanel();
		SpriteFontPanel.Reset();
	}
	public void SetText(string text)
	{
		//	ShowPanel();
		SpriteFontPanel.Render(text);
	}

}

