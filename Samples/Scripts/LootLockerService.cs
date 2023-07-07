using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using LootLocker.Requests;
public class LootLockerService : MonoBehaviour
{
	private ServiceLocator ServiceLocator;
	private float TimeStarted;
	private float TimeFinished;
	private string Name = "WEBGL";
	[HideInInspector]
	public List<string> CompletedGoals =  new List<string>();
	public string memberId = "unregistered";
	
	public void SetName(string name)
	{
		Name = name;
	}
	
	void Awake()
	{
		ServiceLocator = FindObjectOfType<ServiceLocator>();
	}
	void Start()
	{
		LootLockerSDKManager.StartGuestSession((response) =>
		{
			if (!response.success)
			{
				Debug.Log("error starting LootLocker session");

				return;
			}
			OnGuestSessionStart(response);
			Debug.Log("successfully started LootLocker session");
		});
	}
		
	private void OnGuestSessionStart(LootLockerGuestSessionResponse response)
	{
		memberId = response.player_id.ToString();
	
	}
	
	public void WriteScore(int _score, string leaderboardID)
	{
		LootLockerSDKManager.SubmitScore(memberId, _score, leaderboardID,Name, (response) =>
		{
			if (response.statusCode == 200) {
				Debug.Log("Successful");
				CompletedGoals.Add(leaderboardID);
				ServiceLocator.UIController.SpriteFontPanel.Render("You did it "+Name+"!\nYour score is "+GetFormattedTime(_score/1000.0f));
				
			} else {
				Debug.Log("failed: " + response.Error);
				ServiceLocator.UIController.SpriteFontPanel.Render("ERROR SUBMITTING SCORE");
				
			}
		});
	}
	public void ShowScoresOnScreen(string LeaderBoardID, string LeaderBoardName)
	{
		Time.timeScale = 0.0f;
		float timeScore = TimeFinished - TimeStarted;
		int timeScoreConvertedToPoints = Mathf.RoundToInt(timeScore* 1000.0f);		
		LootLockerSDKManager.GetScoreList(LeaderBoardID,10,0,(response)=>
		{
			Time.timeScale =1.0f;
			
			if (response.statusCode == 200) {
				if(CompletedGoals.Contains(LeaderBoardID))
				{
					ServiceLocator.UIController.SpriteFontPanel.text = "YOUR SCORE -  "+ GetFormattedTime(timeScoreConvertedToPoints/1000.0f) ;
				}
				ServiceLocator.UIController.SpriteFontPanel.text = "--- TIMES FOR "+ LeaderBoardName +"---\n(LOWER IS BETTER)\n";
				ServiceLocator.UIController.SpriteFontPanel.text += string.Concat(response.items.Select(x=>{
					var scoreToDisplay = GetFormattedTime(x.score/1000.0f);
					return "- "+ x.metadata+" - "+scoreToDisplay+" - " +x.rank+" -\n";
				}));
				ServiceLocator.UIController.SpriteFontPanel.RenderText();
			}
			else
			{
				ServiceLocator.UIController.SpriteFontPanel.Render("Error Getting Scores");
				
			}
	});
		
	}
	public void OnExitIntroVolume()
	{
		TimeStarted = Time.time;
	}

	public void OnEnterEndingVolume(string leaderboardID)
	{
		leaderboardID = leaderboardID;
		TimeFinished = Time.time;
		float timeScore = TimeFinished - TimeStarted;
		int timeScoreConvertedToPoints = Mathf.RoundToInt(timeScore* 1000.0f);		
		WriteScore(timeScoreConvertedToPoints,leaderboardID);
		
	}
	
	
	public static string GetFormattedTime(float timeInSeconds)
	{
		var ms = Mathf.FloorToInt(timeInSeconds * 100) % 60;
		var gameTimeInt = Mathf.FloorToInt(timeInSeconds);
		var seconds = gameTimeInt % 60;
		var minutes = (gameTimeInt - (gameTimeInt % 60)) / 60;
		var formatedSeconds = seconds < 10 ? seconds.ToString("0#") : seconds.ToString();
		var formatedMS = ms < 10 ? ms.ToString("0#") : ms.ToString();
		var formatedMinutes = minutes < 10 ? minutes.ToString("0#") : minutes.ToString();
		string formattedTime = $"{formatedMinutes}m {formatedSeconds}s {formatedMS}ms";
		return formattedTime;
	}
	
}
