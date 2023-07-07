using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDrawText
{
	public void ShowPanel();
	public void HidePanel();
	public void ClearText();
	public void SetText(string capturedText);
}
