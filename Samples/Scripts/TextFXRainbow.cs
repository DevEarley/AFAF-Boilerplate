using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextFXRainbow: MonoBehaviour
{
	private static float ColorCycleTime = 2.0f;
	private float CurrentColorCycleTime = 0;
	//private static float ColorCycleSpeed =100f;
	private SpriteRenderer SpriteRenderer;
	private int CurrentRainbowIndex = 1;
	//lerp between colors 
	//ROYGBIV
	//R 1,0 1,0,0
	//O 2 1,0.5,0
	//Y 3 1,1,0
	//G 4 0,1,0
	//C 5 0,1,1
	//B 6 0,0,1
	//I 7 0.5,0,1
	//V 8 1,0,1
	private static Color[] Colors = 
	{
		Color.red,
		new Color(1.0f,0.0f,0.5f),
		Color.yellow,
		Color.green,
		Color.cyan,
		Color.blue,
		new Color(0.5f,0.0f,1.0f),
		new Color(1.0f,0.0f,1.0f)
	};
	void Awake()
	{
		SpriteRenderer = gameObject.GetComponent<SpriteRenderer>(); 
		
		
	}
	public void UpdateIndex(int newIndex)
	{
		CurrentRainbowIndex = newIndex % 8; 
	}
	void Update()
	{
		CurrentColorCycleTime+=Time.deltaTime;
		if(CurrentColorCycleTime>ColorCycleTime)
		{
			CurrentColorCycleTime = 0;
			CurrentRainbowIndex = (CurrentRainbowIndex +1) % 8 ;
		}
		
		var nextIndex = (CurrentRainbowIndex +1) % 8;
		Color newColor = Color.Lerp(Colors[CurrentRainbowIndex],Colors[nextIndex],CurrentColorCycleTime/ColorCycleTime);
		newColor.a = SpriteRenderer.color.a;
		SpriteRenderer.color = newColor;
	}
	
}
