using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TextFXFadeStates
{
	FadingIn,
	FadedIn,
	FadingOut,
	FadedOut
}

public class TextFXFade : MonoBehaviour
{
	private static float fadeTime = 0.25f;
	private float lerpProgress = 0;
	
	public bool FadeInOnStart = false;
	private TextFXFadeStates State = TextFXFadeStates.FadedIn;
	private SpriteRenderer SpriteRenderer;
	
	void Awake()
	{
		SpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
	
	}
	
	protected void Start()
	{
		if(FadeInOnStart)
		{
			StartCoroutine(FadeIn());
		}
	}
	void Update()
	{
		if(State == TextFXFadeStates.FadingIn)
		{
			lerpProgress +=Time.deltaTime;
			SetAlpha(Mathf.Lerp(0.0f,1.0f,Mathf.Clamp01(lerpProgress/fadeTime)));
		}
		else if(State == TextFXFadeStates.FadingOut)
		{
			lerpProgress +=Time.deltaTime;
			SetAlpha(Mathf.Lerp(1.0f,0.0f,Mathf.Clamp01(lerpProgress/fadeTime)));
		}
	}
	
	public IEnumerator FadeIn()
	{
		lerpProgress = 0.0f;
		SetAlpha(0.0f);
		State = TextFXFadeStates.FadingIn;
		yield return new WaitForSeconds(fadeTime);
		State = TextFXFadeStates.FadedIn;
		SetAlpha(1.0f);
	}
	
	public IEnumerator FadeOut()
	{
		lerpProgress = 0.0f;
		SetAlpha(1.0f);
		State = TextFXFadeStates.FadingOut;
		yield return new WaitForSeconds(fadeTime);
		State = TextFXFadeStates.FadedOut;
		SetAlpha(0.0f);
	}
	
	private void SetAlpha(float newAlpha)
	{
		if(SpriteRenderer==null)return;
		SpriteRenderer.color =  new Color(SpriteRenderer.color.r,SpriteRenderer.color.g,SpriteRenderer.color.b,newAlpha);;
	}
}
