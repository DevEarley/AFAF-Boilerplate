using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ScenarioService : MonoBehaviour
{
	private List<IReactToAllScenarios> BehavioursThatReactToAllScearios = new List<IReactToAllScenarios>();
	private IDrawText BehaviourThatDrawsText;
	
	
	[HideInInspector]
	public bool active = true;
	
	[HideInInspector]
	public ScenarioDataSet ScenarioDataSet;
    private bool isWaiting = false;
    private DateTime? WaitExpiration = null;
    private List<string> lines;
	private int lineIndex;
	private bool shouldContinueRunningScenario = false;
	private string CapturedText = "";
	
	private DataRepository DataRepository;
	private InputController InputController;
	private OptionPickerService OptionPickerService;
	private TypedInputService TypedInputService;
	private SliderService SliderService;
	
	private string LastLine = "";
	
	void LateUpdate()
    {
        if (active == false) return;
	    if (lines == null || lines.Count == 0) return;
	    if(lines.Count<=lineIndex && shouldContinueRunningScenario)
	    {
	    	Finish();
	    	return;
	    }
	    
	    if(shouldContinueRunningScenario )
	    {
		    ContinueRunningScenario();
	    }
    }

	void Awake()
    {
	    DataRepository = FindObjectOfType<DataRepository>();
	    InputController = FindObjectOfType<InputController>();
	    OptionPickerService = FindObjectOfType<OptionPickerService>();
	    TypedInputService = FindObjectOfType<TypedInputService>();
	    
	    var countOfScripts = 0;
	    MonoBehaviour[] allScripts = FindObjectsOfType<MonoBehaviour>();
	    for (int i = 0; i < allScripts.Length; i++)
	    {
		    if(allScripts[i] is IReactToAllScenarios)
		    {
		    	countOfScripts++;
			    BehavioursThatReactToAllScearios.Add((allScripts[i] as IReactToAllScenarios));
		    }
		    else if(allScripts[i] is IDrawText)
		    {
			    BehaviourThatDrawsText = (allScripts[i] as IDrawText);
		    }
	    }
    }

	public void StartScenario(string scriptContents)
	{
		lines = scriptContents.Split("\n").ToList();
		lineIndex = 0;
		active = true;
		shouldContinueRunningScenario = true;
		BehavioursThatReactToAllScearios.ForEach(x=>x.OnStartScenario());
	}
    
	private void CaptureText(string text)
	{
		CapturedText += text+"\n";
	}
	
	public void ContinueRunningScenario()
	{
		shouldContinueRunningScenario = false;
		if(lineIndex == -1 || lineIndex >= lines.Count)
		{
			Finish();
			return;
		}
		var line = lines[lineIndex];
		LastLine = line;
        if (line.StartsWith(".next"))
        {
	        HandleDotNext();
        }
        else
        {
	        if (line.StartsWith(".options"))
	        {
	        	ShowOptions(line);
	        }
	        else if (line.StartsWith(".go"))
	        {
		        HandleDotGo(line);
	        }
	        else if (line.StartsWith(".end"))
	        {
		        HandleDotEnd();
	        }   
	        else if (line.StartsWith(".if"))
	        {
	        	HandleDotIf(line);
	        }  
	        else if (line.StartsWith(".clear"))
	        {
	        	HandleDotClear();
	        }
	        else if (line.StartsWith(".input"))
	        {
	        	HandleDotInput(line);
	        }
	        else if (line.StartsWith(".set"))
	        {
	        	HandleDotSet(line);
	        }
	        else if (line.StartsWith(".get"))
	        {
	        	HandleDotGet(line);
	        }
	       
	        else if (line.StartsWith(".wait-for-call"))
	        {
	        	HandleCall(line);
	        }
	        else if (line.StartsWith(".call"))
	        {
	        	HandleCall(line);
		        IncrementLineAndRunNextLine();
	        	
	        }
	        else if (line.StartsWith(".wait"))
	        {
	        	StartCoroutine(HandleWait(line));
	        }
	        else if (line.StartsWith("//") || line.StartsWith("\\\\"))
	        {
		        HandleComment();
	        }
	        else if(IsNewLine(line))
	        {
		        HandleNewLine();
	        }
	        else
	        {
		        HandleStaticTextLine(line);
	        }
		}
	}
	public void Continue()
	{
		IncrementLineAndRunNextLine();
		
	}
	private void HandleCall(string line)
	{
		string param1 = ScenarioUtility.GetSubstring(line,"[","]");
		string param2 = ScenarioUtility.GetRestOfLine(line,"]").TrimEnd('\n').TrimEnd('\r');
		param2 = HandleExpression(param2);
		BehavioursThatReactToAllScearios.ForEach(x=>x.OnScenarioCall(param1,param2));
		
		
	}
	
	IEnumerator HandleWait(string line)
	{
		string timeToWait = ScenarioUtility.GetSubstring(line,"[","]");
		yield return new WaitForSeconds(float.Parse(timeToWait));
		IncrementLineAndRunNextLine();
	}
	
	private bool IsNewLine(string line)
	{
		return line=="\n"||line == "\r\n"||line == "\n\r" || line == "\\r" || line == "\\n" || line == "\\r\\n"|| line == "\\n\\r";
	}
	
	private void HandleDotIf(string line)
	{
		string dataIdentifier = ScenarioUtility.GetSubstring(line,"[","]");
		string marker = ScenarioUtility.GetRestOfLine(line,"]");
		var dataItem = ScenarioDataSet.Data.First(x=>x.DataIdentifier == dataIdentifier);
		if(dataItem.Value == "true")
		{
			GoToLine(marker);
		}
		else
		{
			IncrementLineAndRunNextLine();
		}
	}
	

	
	private void HandleDotSet(string line)
	{
		string dataIdentifier = ScenarioUtility.GetSubstring(line,"[","]");
		string newValue = ScenarioUtility.GetRestOfLine(line,"]");
		newValue = HandleExpression(newValue);
		newValue = newValue ==""? "false":newValue;
		bool dataItemIsNew = ScenarioDataSet.Data.Any(x=>x.DataIdentifier == dataIdentifier) == false;
		if(dataItemIsNew)
		{
			ScenarioDataSet.Data.Add(new ScenarioDataSetItem( dataIdentifier,  newValue));
		}
		else
		{
			foreach(var dataItem in ScenarioDataSet.Data.Where(x=>x.DataIdentifier == dataIdentifier))
			{
				dataItem.Value = newValue;
			}	
		}
		IncrementLineAndRunNextLine();
	}
	private void HandleDotClear()
	{
		BehaviourThatDrawsText.ClearText();
		IncrementLineAndRunNextLine();
	}
	private string CurrentSetDataId = "";
	private string CurrentUserInput = "";
	private bool WaitingForUserInput;
	private void HandleDotInput(string line)
	{
		if(WaitingForUserInput)return;
		WaitingForUserInput = true;
		Debug.Log("HandleDotInput: CaptureUserInput");
			TypedInputService.CaptureUserInput(OnUserTypedInput);
		
		CurrentSetDataId = ScenarioUtility.GetSubstring(line,"[","]");
	}
	private void SetData(string dataIdentifier, string newValue)
	{
		bool dataItemIsNew = ScenarioDataSet.Data.Any(x=>x.DataIdentifier == dataIdentifier) == false;
		if(dataItemIsNew)
		{
			ScenarioDataSet.Data.Add(new ScenarioDataSetItem( dataIdentifier,  newValue));
		}
		else
		{
			foreach(var dataItem in ScenarioDataSet.Data.Where(x=>x.DataIdentifier == dataIdentifier))
			{
				dataItem.Value = newValue;
			}	
		}
		DataRepository.UpdateScenarioScriptDataRepoAndSave();
	}
	public void OnUserTypedInput(string value)
	{
		SetData(CurrentSetDataId,value);
		WaitingForUserInput = false;
		IncrementLineAndRunNextLine();
		
	}
	public void UserAdjustedTheValue(float value)
	{
		SetData(CurrentSetDataId,value.ToString());
		WaitingForUserInput = false;
	}
	public void UserPickedOption(string option)
	{
		option = option.TrimEnd().TrimStart();
		var line = lines.First(x=>x.StartsWith("\\\\"+option)||x.StartsWith("\\\\"+option+"\n"));
		lineIndex = lines.IndexOf(line);
		//Debug.Log("UserPickedOption Line|TRY AGAIN|"+lineIndex);
		shouldContinueRunningScenario=true;
	}
	
	
	private void HandleDotGet(string line)
	{
	
		IncrementLineAndRunNextLine();
	}
	
	private string HandleExpression(string line)
	{
		if(line.Contains("[")&&line.Contains("]"))
		{
			var dataId = ScenarioUtility.GetSubstring(line,"[","]");
			var data = GetData();
			var dataValue = data.Data.Find(x=>x.DataIdentifier == dataId);
			return line.Replace("["+dataId+"]",dataValue.Value);
		}
		return line;
	}
	private void HandleStaticTextLine(string line)
	{
		line = HandleExpression(line);
		
		if(String.IsNullOrWhiteSpace(line)==false)
		{
			CaptureText(line);
		}
		if(lines.Count <= lineIndex+1)
		{
			Write();
			IncrementLineAndRunNextLine();
			CapturedText = "";
			return;
		}
		var nextLineIsAControlLine = lines[lineIndex+1].StartsWith(".");
		if(nextLineIsAControlLine)
		{
			Write();
			CapturedText = "";
		}
		IncrementLineAndRunNextLine();
	}
    
	public void HandleComment()
	{
		IncrementLineAndRunNextLine();
	}
	
	public void HandleNewLine()
	{
		IncrementLineAndRunNextLine();
	}
	
	public void HandleDotEnd()
	{
		var line = lines.FirstOrDefault(x=>x.StartsWith("\\\\end")||x.StartsWith("\\\\end\n"));
		if(line == null)
		{
			Finish();
			return;
		}
		var newIndex = lines.IndexOf(line);
		if(newIndex == -1)
		{
			Finish();
			return;
		}
		else
		{
			lineIndex = newIndex;
			shouldContinueRunningScenario=true;
		}
		
	}
	
	private void HandleDotNext()
	{
		if (InputController.CheckInputThrottle() &&
			InputController.WasJumpPressed || InputController.WasAction1Pressed|| InputController.WasAction2Pressed)
		{
			InputController.ResetInputThrottle();
			BehaviourThatDrawsText.ClearText();
			CapturedText ="";
			IncrementLineAndRunNextLine();
		}
		else
		{
			shouldContinueRunningScenario=true;
		}
	}
	
	private void IncrementLineAndRunNextLine()
	{
		lineIndex++; 
		shouldContinueRunningScenario=true;
	}
	
	private void ShowOptions(string line)
	{
		//Debug.Log("Show Options " + line);
		string[] options = ScenarioUtility.GetSubstring(line,"[","]").Split(',');
		OptionPickerService.ShowOptions_ForScenario(options);
	}
	
	public void HandleDotGo(string line)
	{
		string marker = ScenarioUtility.GetSubstring(line,"[","]");
		GoToLine(marker);
	}
	
	public void GoToLine(string marker)
	{
		Debug.Log("GOTO | "+marker);
		var line = lines.First(x=>x.StartsWith("\\\\"+marker)||x.StartsWith("\\\\"+marker+"\n"));
		lineIndex= lines.IndexOf(line);
		shouldContinueRunningScenario=true;
	}
	
    public bool IsWaiting()
    {
        if (WaitExpiration != null && WaitExpiration <= DateTime.Now)
        {
            isWaiting = false;
            WaitExpiration = null;
        }
        return isWaiting;
    }

    public bool Wait(int timeToWait)
    {
        if (isWaiting == false)
        {
            isWaiting = true;
            WaitExpiration = DateTime.Now.AddMilliseconds(timeToWait);
        }
        return isWaiting;
    }

    //public string ReadUserInput()
    //{
    //    return null;
    //}

    //public string[] GetLinesFromFile(string filename)
    //{
    //    var dir = System.IO.Path.GetFullPath(filename);
    //    //Debug.Log(dir);
    //    if (System.IO.File.Exists(dir))
    //    {
    //        try
    //        {
    //            return System.IO.File.ReadAllLines(dir);
    //        }
    //        catch
    //        {
    //            return null;
    //        }
    //    }
    //    else
    //    {
    //        return null;
    //    }
    //}

    //public string GetLineFromFile(string filename)
    //{
    //    if (System.IO.File.Exists(filename))
    //    {
    //        try
    //        {
    //            return System.IO.File.ReadAllLines(filename).FirstOrDefault();
    //        }
    //        catch (Exception e)
    //        {
	          
	//            return "ERROR"+e.Message;
    //        }
    //    }
    //    else
    //    {
    //        return null;
    //    }
    //}

    public void Write()
	{
		//Debug.Log("Scenario Service - Write");
		// BehaviourThatDrawsText.ShowPanel();
	    BehaviourThatDrawsText.SetText(CapturedText);
	    BehavioursThatReactToAllScearios.ForEach(x=>x.OnProcessedLine(CapturedText));
	    
    }

    public void Call(string callparams)
    {
        isWaiting = true;
    }
	
    private void Finish()
	{
		if (InputController.CheckInputThrottle() &&
			InputController.WasAnyButtonReleased())
		{
			////Debug.Log("Finished Scenario");
			
			active = false;
			BehavioursThatReactToAllScearios.ForEach(x=>x.OnCompletedScenario());
			BehaviourThatDrawsText.HidePanel();
			lineIndex = 0;
		}
		else
		{
			shouldContinueRunningScenario=true;
		}
	}
    
    //public string GetUserInput()
    //{
    //    var line = ReadUserInput();
    //    if (line == null) return "";
    //    return line;
    //}
	public void SetDataValue(string dataId, string value)
	{
		ScenarioDataSet.UpsertDataValue(dataId,value);
	}
	public void LoadData(ScenarioDataSet _ScenarioDataSet)
	{
		ScenarioDataSet = _ScenarioDataSet;
	}
	
	public ScenarioDataSet GetData()
	{
		return ScenarioDataSet;
	}
}

[System.Serializable]
public class ScenarioDataSet
{
	public List<ScenarioDataSetItem> Data;
	public ScenarioDataSet(GameDataModel gameData)
	{
		var slot1 = gameData.Slots[0];
		Data = new List<ScenarioDataSetItem>();
		Data.Add(new ScenarioDataSetItem("name",slot1.name));
		Data.Add(new ScenarioDataSetItem("mouse sensitivity",slot1.mouseSensitivity));
		Data.Add(new ScenarioDataSetItem("volume",slot1.volume));
		
		//Data.AddRange(slot1.Collectables.Select(c=>new ScenarioDataSetItem(c.type.ToString()+c.ID.ToString(),"true")));
		//Data.Add(new ScenarioDataSetItem("last-scene",slot1.LastScene));
		//Data.Add(new ScenarioDataSetItem("last-spawn",slot1.LastSpawn));
		//	Data.Add(new ScenarioDataSetItem("current-health",slot1.CurrentHealth.ToString()));
	}
	
	public void UpsertDataValue(string dataId, string value)
	{
		if(Data.Any(x=>x.DataIdentifier == dataId))
		{
			
		}
	}
}

[System.Serializable]
public class ScenarioDataSetItem
{
	public string DataIdentifier;
	public string Value;
	public ScenarioDataSetItem(string name, string value)
	{
		DataIdentifier = name; Value = value;
	}
}