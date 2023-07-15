using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneTransitionTypes
{
	AutoMove,
	Fall,
	Still
}
public class SceneTransitioner : MonoBehaviour
{
	private SceneTransitionTypes SceneTransitionType;
	
	[HideInInspector]
	public string TargetSpawn = "S1";
	
	
	private static float walkingUnloadTime = 3.0f;
	private static float fallingUnloadTime = 0.5f;
	private static float loadTime = 2.5f;
	private bool WaitingForScene = false;
	
	protected void Awake()
	{
		DontDestroyOnLoad(this.gameObject);
		
	}
	
	void Start()
	{
		
	
	
	}

	void Update()
	{
		if(WaitingForScene)
		{
			
			var respawn = GameObject.Find(TargetSpawn);
			if(respawn == null)return;
			WaitingForScene = false;
			
			StartCoroutine(FinishTransition(respawn));
		}
	}
	public IEnumerator StartTransitionToNextScene(string sceneName,string targetSpawn, SceneTransitionTypes sceneTransitionType, GameObject door)
	{
		SceneTransitionType = sceneTransitionType;
		TargetSpawn = targetSpawn;
		if(		SceneTransitionType == SceneTransitionTypes.Fall)
		{
			yield return new WaitForSeconds(fallingUnloadTime);
			SceneManager.LoadScene(sceneName);
			WaitingForScene = true;
		}
		else if(SceneTransitionType == SceneTransitionTypes.AutoMove)
		{
			yield return new WaitForSeconds(walkingUnloadTime);
			SceneManager.LoadScene(sceneName);
			WaitingForScene = true;
		}
		else 
		{
			yield return new WaitForSeconds(walkingUnloadTime);
			SceneManager.LoadScene(sceneName);
			WaitingForScene = true;
		}
		
	}
	
	public void TransitionToNextScene(string sceneName,string targetSpawn, SceneTransitionTypes sceneTransitionType, GameObject door)
	{
		SceneTransitionType = sceneTransitionType;
		TargetSpawn = targetSpawn;
		StartCoroutine(StartTransitionToNextScene(sceneName,targetSpawn,sceneTransitionType, door));
	}
	
	public void TransitionToNextScene(string targetSceneName)
	{
		SceneManager.LoadScene(targetSceneName);
	}
	
	
	public IEnumerator FinishTransition(GameObject respawn)
	{
		
		if(SceneTransitionType == SceneTransitionTypes.Fall)
		{
			//do something
		}
		else if(SceneTransitionType == SceneTransitionTypes.AutoMove)
		{
			
			//do something	
		}
		else if(SceneTransitionType == SceneTransitionTypes.Still)
		{
			
			//do something		
		}
		yield return new WaitForSeconds(loadTime);
		//do something
	}
}
