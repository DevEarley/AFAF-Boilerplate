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
	//private static float waitForSceneInit = 0.1f;
	private bool WaitingForScene = false;
	private ServiceLocator ServiceLocator;
	
	// Awake is called when the script instance is being loaded.
	protected void Awake()
	{
		DontDestroyOnLoad(this.gameObject);
		
	}
	
	void Start()
	{
		ServiceLocator = GameObject.FindAnyObjectByType<ServiceLocator>();
	
	
	}

	void Update()
	{
		if(WaitingForScene)
		{
			//Debug.Log("WaitingForScene | Start");
			ServiceLocator = GameObject.FindAnyObjectByType<ServiceLocator>();
			//if( ServiceLocator == null || ServiceLocator.PlayerController == null)
			//{
			//	ServiceLocator = GameObject.FindAnyObjectByType<ServiceLocator>();
			//	return;
			//}
			//Debug.Log("WaitingForScene | Done");
			
			var respawn = GameObject.Find(TargetSpawn);
			if(respawn == null)return;
			WaitingForScene = false;
			
			StartCoroutine(FinishTransition(respawn));
		}
	}
	public IEnumerator StartTransitionToNextScene(string sceneName,string targetSpawn, SceneTransitionTypes sceneTransitionType, GameObject door)
	{
		SceneTransitionType = sceneTransitionType;
		//Debug.Log("TransitionToNextScene | Start |" + sceneName+ " | " + targetSpawn);
		
		TargetSpawn = targetSpawn;
		
		ServiceLocator = GameObject.FindAnyObjectByType<ServiceLocator>();
		
		
		if(		SceneTransitionType == SceneTransitionTypes.Fall)
		{
			//Debug.Log("DontLookForward | Start");
			
			//ServiceLocator.PlayerController.LockInputAndFall();
			yield return new WaitForSeconds(fallingUnloadTime);
			//Debug.Log("LoadScene | Start");
			SceneManager.LoadScene(sceneName);
			WaitingForScene = true;
			
			
		
		}
		else if(		SceneTransitionType == SceneTransitionTypes.AutoMove)
		{
			//Debug.Log("LookForward | Start");
			
			//ServiceLocator.PlayerController.LockInputAndMoveForward(door);
			yield return new WaitForSeconds(walkingUnloadTime);
			//Debug.Log("LoadScene | Start");
			SceneManager.LoadScene(sceneName);
			WaitingForScene = true;
			
	
		}
		else //if(SceneTransitionType = SceneTransitionTypes.Still)
		{
			//Debug.Log("LookForward | Start");
			
			//ServiceLocator.PlayerController.LockInputAndStop();
			yield return new WaitForSeconds(walkingUnloadTime);
			//Debug.Log("LoadScene | Start");
			SceneManager.LoadScene(sceneName);
			WaitingForScene = true;
			
	
		}
		
	}
	
	public void TransitionToNextScene(string sceneName,string targetSpawn, SceneTransitionTypes sceneTransitionType, GameObject door)
	{
		SceneTransitionType = sceneTransitionType;
		
		//Debug.Log("TransitionToNextScene | Start");
		
		TargetSpawn = targetSpawn;
		StartCoroutine(StartTransitionToNextScene(sceneName,targetSpawn,sceneTransitionType, door));
	}
	
		
	public void TransitionToNextScene(string targetSceneName)
	{
		SceneManager.LoadScene(targetSceneName);

	}
	
	
	public IEnumerator FinishTransition(GameObject respawn)
	{
		//Debug.Log("FinishTransition | Wait");
		//ServiceLocator.PlayerController.MovePlayerForRespawn(respawn);
		
		if(SceneTransitionType == SceneTransitionTypes.Fall)
		{
			//Debug.Log("LockInputAndFall | Start");
				
			//ServiceLocator.PlayerController.LockInputAndFall();
		}
		else if(SceneTransitionType == SceneTransitionTypes.AutoMove)
		{
			//Debug.Log("LockInputAndMoveForward | Start");
				
			//ServiceLocator.PlayerController.LockInputAndMoveForward(respawn);
					
		}
		else if(SceneTransitionType == SceneTransitionTypes.Still)
		{
			//Debug.Log("LockInputAndBeStill | Start");
					
			//ServiceLocator.PlayerController.LockInputAndStop();
					
		}
		yield return new WaitForSeconds(loadTime);
		//Debug.Log("FinishTransition | Start");
		//ServiceLocator.PlayerController.UnlockInput();
		//Destroy(this.gameObject);
	}
}
