using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

public class SpriteFontPanel : MonoBehaviour
{
	private bool WaitingForTransition = false;
	private static float TransitionTimeFor_FadeOneLetterAtATime = 1.5f;
	private static float TransitionTimeFor_FadeEverything = 0.25f;
	private static float TransitionTimeFor_None = 0.1f;
	[TextAreaAttribute]
	public string text;
	public Color Color = Color.black;
	private  List<SpriteRenderer> renderers;
    public SpriteFont SpriteFont;
    public Vector2 offset;
    public float scale = 1.0f;
    public float kerning = 0.0f;
	public int maxCharactersPerLine = 80;
	public int maxCharacterPool = 200;
	public float lineHeight = 0.1f;
	public Material outlineMaterial;
	public Material boldOutlineMaterial;
	public bool ShowOnStart = true;
	public bool IsCentered = false;
	public bool ShowOneCharacterAtATime = false;
	public float CharacterDelay = 0.1f;
	private float NextShowing = 0;
	private bool ReadyToShow = false;
	private int CurrentlyShowingIndex = 0;
	
	protected void Update()
	{
		if(ShowOneCharacterAtATime && ReadyToShow && NextShowing<Time.time )
		{
			if(renderers.Count == CurrentlyShowingIndex)
			{
				ReadyToShow = false;
				return;
			}
			NextShowing = Time.time + CharacterDelay;
			SpriteRenderer sprite =renderers[CurrentlyShowingIndex];
			sprite.color = Color;
			CurrentlyShowingIndex++;
		}
	}
	// Parse text for 'fancy' indicators.
	// fancy indicators are escaped with grave ticks ``
	/*
	
	`small`If you say so!`
	
	`bold`Ok fine!`
	
	`wavvy`H E L L O`
	
	`rainbow+wavvy`C O M B O!`
	
	pseudo code:
	1. Look for first grave tick
	2. Get text till next grave tick, this is the modifier
	3. Get text till next grave tick, this is the copy.
	3.a. Add modifier to Renderer
	
	*/
	public void Start()
	{
		renderers = new List<SpriteRenderer>();
	
		if(ShowOnStart && text !=null && text.Length>0)
		{
			StartCoroutine(Render(text,SpriteFontPanelTransitions.None,SpriteFontPanelTransitions.FadeEverything));
		}
	}
	
	private void FadeOutAllChildRenderers()
	{
		var childRenderers = gameObject.GetComponentsInChildren<SpriteRenderer>();
		foreach(var spriteRenderer in childRenderers)
		{
			TextFXFade fadeComponent = spriteRenderer.gameObject.GetComponent<TextFXFade>();
			if(fadeComponent == null)
			{
				fadeComponent =	spriteRenderer.gameObject.AddComponent<TextFXFade>();
			}
			StartCoroutine(fadeComponent.FadeOut());
		}
	}
	
	private void FadeInAllChildRenderers()
	{
		if(gameObject.activeSelf==false)return;
		var childRenderersII = gameObject.GetComponentsInChildren<SpriteRenderer>();
		
		foreach(var spriteRenderer in childRenderersII)
		{
			TextFXFade fadeComponent = spriteRenderer.gameObject.GetComponent<TextFXFade>();
			if(fadeComponent == null)
			{
				fadeComponent =	spriteRenderer.gameObject.AddComponent<TextFXFade>();
			}
			StartCoroutine(fadeComponent.FadeIn());
		}
	}
	
	
	private void ScaleOutAllChildRenderers()
	{
		var childRenderers = gameObject.GetComponentsInChildren<SpriteRenderer>();
		foreach(var spriteRenderer in childRenderers)
		{
			TextFXScale scaleComponent = spriteRenderer.gameObject.GetComponent<TextFXScale>();
			if(scaleComponent == null)
			{
				scaleComponent = spriteRenderer.gameObject.AddComponent<TextFXScale>();
			}
			StartCoroutine(scaleComponent.ScaleOut());
		}
	}
	
	private void ScaleInAllChildRenderers()
	{
		if(gameObject.activeSelf==false)return;
		var childRenderersII = gameObject.GetComponentsInChildren<SpriteRenderer>();
		
		foreach(var spriteRenderer in childRenderersII)
		{
			TextFXScale scaleComponent = spriteRenderer.gameObject.GetComponent<TextFXScale>();
			if(scaleComponent == null)
			{
				scaleComponent =	spriteRenderer.gameObject.AddComponent<TextFXScale>();
			}
	 		StartCoroutine(scaleComponent.ScaleIn());
		}
	}
	
	public void RenderText()
	{
		CurrentlyShowingIndex = 0;
		Debug.Log("RenderText");
		WaitingForTransition = false;
		Reset();
		
		CreateRenderers(text);
	}
	
	public void Render(string _text)
	{
		CurrentlyShowingIndex = 0;
		Debug.Log("Render(string _text)");
		WaitingForTransition = false;
		text = _text;
		Reset();
		CreateRenderers(_text);
	}
	
	public IEnumerator Render(string _text, SpriteFontPanelTransitions OutTransition, SpriteFontPanelTransitions InTransition)
	{
		CurrentlyShowingIndex = 0;
		Debug.Log("IEnumerator Render(...");
		var needsFade = text!=_text;
		if(gameObject.activeSelf)
		{
			if(needsFade)
			{
				FadeOutAllChildRenderers();
			}
			text = _text;
			WaitingForTransition = true;
			//add fade component if it's not already added - set to fade out
			switch (OutTransition)
			{
				case SpriteFontPanelTransitions.FadeEverything:
					yield return new WaitForSeconds(TransitionTimeFor_FadeEverything);
					break;
				case SpriteFontPanelTransitions.FadeOneLetterAtATime:
					yield return new WaitForSeconds(TransitionTimeFor_FadeOneLetterAtATime);
					break;
				default:
				case SpriteFontPanelTransitions.None:
					yield return new WaitForSeconds(TransitionTimeFor_None);
					break;
			}
		}
		if(gameObject.activeSelf)
		{
			Reset();
			CreateRenderers(_text);
			//add fade component if it's not already added - set to fade in
			if(needsFade)
			{
				FadeInAllChildRenderers();
			}
			switch (InTransition)
			{
			case SpriteFontPanelTransitions.FadeEverything:
				yield return new WaitForSeconds(TransitionTimeFor_FadeEverything);
				break;
			case SpriteFontPanelTransitions.FadeOneLetterAtATime:
				yield return new WaitForSeconds(TransitionTimeFor_FadeOneLetterAtATime);
				break;
			default:
			case SpriteFontPanelTransitions.None:
				yield return new WaitForSeconds(TransitionTimeFor_None);
				break;
			}
			WaitingForTransition = false;
		}
	}

	private void CreateRenderers(string _text)
	{
		//Debug.Log("CreateRenderers | "+text);
		int characterCountPerLine = 0;
		int breakIndex = 0;
		int lastWrappedIndex = 0;
		int windowIndex = 0;
		int lineIndex = 0;
		int? nextSpace = null;
		var lines = new List<string>();
		for(int i =0; i<_text.Length; i++)
		{
			var breaking = false;
			var character = _text[i];
			var isLastCharacter = i == _text.Length-1;
			var os = character == ' ';
			var isnl = character=='\n';
			var lastCharacterInWindow = lastWrappedIndex + maxCharactersPerLine == i;
			if(os)
			{
				nextSpace = GetNextSpace(i+1,_text.Length,_text);
				//Debug.Log("Index = " + i + " | NS = " + nextSpace);
			}
			else
			{
				nextSpace = GetNextSpace(i,_text.Length,_text);
			}
			
			var siw = IsSpaceInWindow(i,maxCharactersPerLine,lastWrappedIndex);
			var nsnull = nextSpace == null;
			var nsow = nsnull == false && lastWrappedIndex + maxCharactersPerLine < nextSpace;
			var onchar = os == false && isnl == false;
			var nsiw = lastWrappedIndex + maxCharactersPerLine >= nextSpace;
			var ncharow = isLastCharacter == false && lastCharacterInWindow ;

			if(isnl)
			{
				breaking =  true;
			}
			else if(ncharow)
			{
				breaking = true;
			}
			else if( os && nsow && !nsnull && !onchar && !nsiw )
			{
				breaking = true;
			}
			else
			{
				breaking = false;
			}
			if(breaking )
			{
				if(lineIndex <= lines.Count-1)
				{
					lines[lineIndex]+=character;
				}
				
				breakIndex = i;
				lastWrappedIndex = breakIndex + 1;
				lineIndex++;
				windowIndex = lastWrappedIndex + characterCountPerLine;
				lines.Add("");
			}
			else
			{
				if(lines.Count==lineIndex)
				{
					lines.Add("");
				}
				lines[lineIndex]+=character;
			}
		}
		var charIndex = 0;
		for(int newLineIndex =0; newLineIndex<lines.Count; newLineIndex++)
		{
			var line = lines[newLineIndex];
			line = line.TrimStart();
			var lengthWithoutNewLine = line.Contains('\n')?line.Length-1:line.Length;
			for(int i =0; i<line.Length; i++)
			{
				var character = line[i];
				
				if(character!='\n')
				{
					RenderCharacter(character, newLineIndex, i, charIndex, lengthWithoutNewLine);
					charIndex++;
				}   
			} 
		}
		ReadyToShow = true;
	}

	private void RenderCharacter(char character,int lineIndex, int characterIndexInLine, int characterIndex, int numberOfCharsInLine)
	{
		if(renderers==null){renderers = new List<SpriteRenderer>();}
		SpriteRenderer newRenderer;
		if(renderers.Count-1<characterIndex)
		{
			//Debug.Log("Need More Renderers");
			var newGO = new GameObject();
			newRenderer = newGO.AddComponent<SpriteRenderer>();
			renderers.Add(newRenderer);
		}
		else
		{
			newRenderer = renderers[characterIndex];
		}
		if(newRenderer == null)
		{
			var newGO = new GameObject();
			newRenderer = newGO.AddComponent<SpriteRenderer>();
			renderers.Add(newRenderer);
		}
		if(ShowOneCharacterAtATime)
		{
			newRenderer.color = UnityEngine.Color.clear;
		}
		else
		{
			newRenderer.color = Color;
		}
		newRenderer.gameObject.transform.SetParent(gameObject.transform);
		var xPosition =characterIndexInLine * 
		((SpriteFont.defaultCharacterSpacing * scale) + kerning); 
		var yPosition = -lineIndex*lineHeight;
		if(IsCentered)
		{
			var centeredOffset =-((kerning + (SpriteFont.defaultCharacterSpacing * scale))*(numberOfCharsInLine)) / 2.0f;
			
			centeredOffset+=(kerning + (SpriteFont.defaultCharacterSpacing * scale))/2.0f;
			
			xPosition += centeredOffset;
		}
		
		var position = new Vector2(xPosition,yPosition);
		position += offset;
		newRenderer.gameObject.transform.localPosition = position;
	
		newRenderer.gameObject.transform.localScale = scale * Vector3.one;
		newRenderer.gameObject.transform.localRotation = Quaternion.identity;
		newRenderer.gameObject.layer = gameObject.layer;
		newRenderer.sprite = SpriteFont.GetSpriteForChar(character);
		//Debug.Log("RenderCharacter | "+character+"|"+newRenderer.gameObject.transform.localPosition.ToString());
		
	}
	
	private int BreakOnSpace(int characterIndex)
	{
		return characterIndex;
	}
	private int BreakOnWord(int maxCharacters, int lastWrappedIndex)
	{
		return maxCharacters + lastWrappedIndex -1;
	}
	private bool IsSpaceInWindow(int characterIndex, int maxCharacters, int lastWrappedIndex)
	{
		return (lastWrappedIndex + maxCharacters) >= characterIndex;
	}
	private int GetNewLastWrappedIndex(int breakIndex)
	{
		return breakIndex +1;
	}
	private int? GetNextSpace(int characterIndex, int textLength, string text)
	{
		int? nextSpaceIndex = null;
		for(var i = characterIndex; i<textLength;i++)
		{
			if(text[i]==' '){
				nextSpaceIndex = i;
				i = textLength;
			}
		}
		return nextSpaceIndex;
	}
	
	private int? GetNextNewLine(int characterIndex, int textLength, string text)
	{
		var subString = text.Substring(characterIndex);
		return GetIndex(subString,"\n");
	}
	static int? GetIndex(string text, string substr)
	{
		int index = text.IndexOf(substr);
		return index >= 0 ? (int?)index : null;
	}
	private void HandleModifier()
	{
		//	foreach(var modifier in modifiers)
		//	{
		//    	var lowercaseModifier = modifier.ToLower();
		//    	switch(modifier)
		//    	{
		//    	case "bold":
		//        	newRenderer.material = outlineMaterial;
		//        	break;	
		//    	case "wavvy":
		//    	case "wavy":
		//        	newGO.AddComponent<TextFXWavvy>();
		//        	newGO.GetComponent<TextFXWavvy>().WaveIndex = characterIndex;
		//        	newGO.GetComponent<TextFXWavvy>().SetOriginalPosition();
		//        	break;	
		//    	case "small":
		//        	newGO.AddComponent<TextFXSmall>();
		//        	break;
		//    	case "outline":
		//        	newRenderer.material = outlineMaterial;
		//        	break;
		//    	case "rainbow":
		//        	newGO.AddComponent<TextFXRainbow>();
		//        	newGO.GetComponent<TextFXRainbow>().UpdateIndex(characterIndex);
		//        	break;	
		//    	default:
		//        	break;
		//    	}
		//	}
	}

	public void Reset()
	{
		Debug.Log("Reset On Trigger Sprite Font Panel");
		
		CurrentlyShowingIndex = 0;
		foreach (Transform child in transform)
		{
			if(child!=null)
			{
				child.GetComponent<SpriteRenderer>().sprite = SpriteFont.space;
			}
		}

		
		//#if UNITY_EDITOR
		//renderers = new List<SpriteRenderer>();
		//try
		//{
			
		//	int maxWhileLoops = 20;
		//	int WhileLoops = 0;
		//	while(WhileLoops<maxWhileLoops && transform!=null && transform.childCount>0)
		//	{
		//		WhileLoops++;
		//		foreach (Transform child in transform)
		//		{
		//			GameObject.Destroy(child.gameObject);
		//		}
		//	}
		//}
		//catch(System.Exception e)
		//{
		//	try
		//	{
			
		//		int maxWhileLoops = 20;
		//		int WhileLoops = 0;
		//		while(WhileLoops<maxWhileLoops && transform!=null && transform.childCount>0)
		//		{
		//			WhileLoops++;
		//			foreach (Transform child in transform)
		//			{
		//				GameObject.DestroyImmediate(child.gameObject);
		//			}
		//		}
		//	}
		//		catch(System.Exception e2)
		//	{
		//		Debug.Log("Couldn't destroy renderers for SpriteFontPanel");
				
		//	}
		//}
	  
//#else
	   
//			foreach (Transform child in transform)
//			{
//			child.GetComponent<SpriteRenderer>().sprite = SpriteFont.space;
//			}
	  
//			//renderers = new List<SpriteRenderer>();
//#endif
	}
	
}
#if UNITY_EDITOR

[CustomEditor(typeof(SpriteFontPanel))]
[CanEditMultipleObjects]
 public class SpriteFontPanelButton : Editor {
    
	
	 public override void OnInspectorGUI()
	 {
		 DrawDefaultInspector();

		 SpriteFontPanel spriteFontPanel = (SpriteFontPanel)target;
		 if (GUILayout.Button("Render Text"))
		 {
			 spriteFontPanel.Reset();
			 spriteFontPanel.RenderText();
		 }
		 if (GUILayout.Button("Reset"))
		 {
			 spriteFontPanel.Reset();
		 }
	 }
 }
#endif
public enum SpriteFontPanelTransitions
{
	FadeOneLetterAtATime,
	FadeEverything,
	ScaleEverything,
	None
}
