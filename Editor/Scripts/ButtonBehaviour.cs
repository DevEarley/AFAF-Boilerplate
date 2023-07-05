using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonBehaviour : MonoBehaviour
{
	private static float ButtonTextFadeInDelay = 0.1f;
	private static float ButtonLockDelay = 0.15f;
	private static float ButtonDelayTime = 0.4f;
	public string ButtonText;
	private SpriteFontPanel ButtonSpriteFontPanel;
	private InputController InputController;
	//private ServiceLocator ServiceLocator;
	public int OptionIndex;
	public bool ButtonHovering = false;
	public bool ButtonLock = false;
	
	public delegate void CallbackDelegate(int optionIndex);
	public CallbackDelegate ButtonCallback;
	
	public delegate void HoverDelegate(int optionIndex);
	public HoverDelegate ButtonHover;
	
	public Animator ButtonAnimator;
	
	void LateUpdate()
	{
		if(InputController.InputDetected)
		{
			OnInputEvent();
		}
	}
	
	void Awake()
	{
		InputController = FindObjectOfType<InputController>();
		ButtonSpriteFontPanel = gameObject.GetComponentInChildren<SpriteFontPanel>();
		ButtonAnimator = gameObject.GetComponentInChildren<Animator>();
		//InputController.Subscribers.Add(gameObject);
	}
	
	protected void Start()
	{
		CheckButton();
	}
	
	public void OnRegistered(string buttonText,int optionIndex, CallbackDelegate _Callback, HoverDelegate _Hover)
	{
		ButtonLock = true;
		
		SetButtonText(buttonText);
		OptionIndex = optionIndex;
		ButtonCallback = _Callback;
		ButtonHover = _Hover;
	}

	public void SetButtonText(string _text)
	{
		ButtonText = _text;
		if(ButtonText!=null)
		{
			StartCoroutine(AnimateButtonText());
		}
	}
	
	private IEnumerator AnimateButtonText()
	{
		yield return new WaitForSeconds(ButtonTextFadeInDelay);
		StartCoroutine(ButtonSpriteFontPanel.Render(ButtonText, SpriteFontPanelTransitions.FadeEverything,SpriteFontPanelTransitions.FadeOneLetterAtATime));
		yield return new WaitForSeconds(ButtonLockDelay);
		ButtonLock = false;
	}
	
	private void CheckButton()
	{
		if(ButtonLock) return;
		var buttonReleased = InputController.WasAnyButtonReleased();
		if(ButtonHovering && buttonReleased == false)
		{
			ButtonAnimator.Play("ButtonHover");
		}
		else if(ButtonHovering && buttonReleased == true)
		{
			StartCoroutine(ButtonPressed());
		}
		else
		{
			ButtonAnimator.Play("ButtonIdle");
		}
	}
	
	public void OnInputEvent()
	{
		CheckButton();
	}
	
	//protected void OnDestroy()
	//{
	//	InputController.Subscribers.Remove(gameObject);
	//}
	public void PressButtonAnimation()
	{
		ButtonAnimator.Play("ButtonDown");
		
	}
	private IEnumerator ButtonPressed()
	{
		ButtonLock = true;
		ButtonAnimator.Play("ButtonDown");
		yield return new WaitForSeconds(ButtonDelayTime);
		ButtonLock = false;
		ButtonCallback(OptionIndex);
	}
	
	void OnMouseOver()
	{
		if(ButtonHovering == false)
		{
			ButtonHovering = true;
			ButtonAnimator.Play("ButtonHover");
			ButtonHover(OptionIndex);
			
		}
	}

	void OnMouseExit()
	{
		StopHovering();
	}
	public void StopHovering()
	{
		ButtonHovering = false;
		ButtonAnimator.Play("ButtonIdle");
	}
}
