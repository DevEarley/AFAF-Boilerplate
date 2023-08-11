using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour, IDrawText
{
	public SpriteFontPanel SpriteFontPanel;
	public SpriteFontPanel LeaderboardSpriteFontPanel;
	public void HidePanel()
	{
		SpriteFontPanel.Reset();
	}
		
	public void ShowPanel()
	{

	}
	public void ClearText()
	{
		SpriteFontPanel.Reset();
	}
	public void SetText(string text)
	{
		SpriteFontPanel.Render(text);
	}
}