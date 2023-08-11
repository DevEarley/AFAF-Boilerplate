using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
	
	void Update()
	{
		CheckButton();
		
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
	
	public void RegisterButton(string buttonText,int optionIndex, CallbackDelegate _Callback, HoverDelegate _Hover)
	{
		SetButtonText(buttonText);
		OptionIndex = optionIndex;
		ButtonCallback = _Callback;
		ButtonHover = _Hover;
	}
	public void RegisterButton(int optionIndex, CallbackDelegate _Callback, HoverDelegate _Hover)
	{
		OptionIndex = optionIndex;
		ButtonCallback = _Callback;
		ButtonHover = _Hover;
	}

	public void SetButtonText(string _text)
	{
		ButtonText = _text;
		if(ButtonText!=null && ButtonText != ""  && ButtonSpriteFontPanel!=null)
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
		var buttonReleased =  Mouse.current.leftButton.wasReleasedThisFrame;
		if(ButtonHovering && buttonReleased == false)
		{
			//Debug.Log("ButtonHover");
			
			ButtonAnimator.Play("ButtonHover");
		}
		else if(ButtonHovering && buttonReleased == true)
		{
			//Debug.Log("ButtonPressed");
			
			StartCoroutine(ButtonPressed());
		}
		else
		{
			//Debug.Log("ButtonIdle");
			
			ButtonAnimator.Play("ButtonIdle");
		}
	}
	
	public void OnInputEvent()
	{
		CheckButton();
	}
	
	public void PressButtonAnimation()
	{
		ButtonAnimator.Play("ButtonDown");
		
	}
	private IEnumerator ButtonPressed()
	{
		Debug.Log("ButtonPressed");
		ButtonLock = true;
		ButtonAnimator.Play("ButtonDown");
		yield return new WaitForSeconds(ButtonDelayTime);
		ButtonLock = false;
		ButtonCallback(OptionIndex);
	}
	
	void OnMouseOver()
	{
		//Debug.Log("OnMouseOver");
		
		if(ButtonHovering == false)
		{
			ButtonHovering = true;
			ButtonAnimator.Play("ButtonHover");
			ButtonHover(OptionIndex);
			CheckButton();
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
