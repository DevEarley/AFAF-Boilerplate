using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum TextFXScaleStates
{
	ScalingIn,
	ScaledIn,
	ScalingOut,
	ScaledOut
}

public class TextFXScale : MonoBehaviour
{
	public float ScaleTo = 1.0f;
	
	private static float scaleTime = 0.15f;
	
	private float lerpProgress = 0;
	

	public bool ScaleInOnStart = false;
	private TextFXScaleStates State = TextFXScaleStates.ScaledIn;
	private SpriteRenderer SpriteRenderer;

	void Awake()
	{
		SpriteRenderer = gameObject.GetComponent<SpriteRenderer>();

	}

	protected void Start()
	{
		if(ScaleInOnStart)
		{
			StartCoroutine(ScaleIn());
		}
	}
	void Update()
	{
		if(State == TextFXScaleStates.ScalingIn)
		{
			lerpProgress +=Time.deltaTime;
			SetScale(Mathf.Lerp(0.0f,ScaleTo,lerpProgress/scaleTime));
		}
		else if(State == TextFXScaleStates.ScalingOut)
		{
			lerpProgress +=Time.deltaTime;
			SetScale(Mathf.Lerp(ScaleTo,0.0f,lerpProgress/scaleTime));
		}
	}

	public IEnumerator ScaleIn()
	{
		lerpProgress = 0.0f;
		SetScale(0.0f);
		State = TextFXScaleStates.ScalingIn;
		yield return new WaitForSeconds(scaleTime);
		State = TextFXScaleStates.ScaledIn;
		SetScale(ScaleTo);
	}

	public IEnumerator ScaleOut()
	{
		lerpProgress = 0.0f;
		SetScale(ScaleTo);
		State = TextFXScaleStates.ScalingOut;
		yield return new WaitForSeconds(scaleTime);
		State = TextFXScaleStates.ScaledOut;
		SetScale(0.0f);
	}

	private void SetScale(float newScale)
	{
		if(SpriteRenderer==null)return;
		SpriteRenderer.transform.localScale =Vector3.one * newScale;
	}
}