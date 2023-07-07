using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public static class GameUtility 
{
	public static void LoadScene(string sceneNameAndSpawnName)
	{
		if(sceneNameAndSpawnName.Contains(","))
		{
			string[] options = ScenarioUtility.GetRestOfLine(sceneNameAndSpawnName,"[").TrimEnd(']').Split(',');
			string sceneName = options[0];
			string spawnName = options[1];
			//Debug.Log("LoadNewScene | "+sceneName +" | "+spawnName);
			var sceneTransitioner = GameObject.Find("SceneTransitioner").GetComponent<SceneTransitioner>();
			sceneTransitioner.TransitionToNextScene(sceneName,spawnName,SceneTransitionTypes.Still, null);
		}
		else
		{
			SceneManager.LoadScene(sceneNameAndSpawnName);
			//var sceneTransitioner = GameObject.Find("SceneTransitioner").GetComponent<SceneTransitioner>();
			//sceneTransitioner.TransitionToNextScene(sceneNameAndSpawnName);
		}
	}
	
	public static float LookAt(Vector3 origin, Vector3 target)
	{
		Vector2 from = Vector2.right;
		Vector3 to = target - origin;

		float angle = Vector2.Angle(from, to);
		Vector3 cross = Vector3.Cross(from, to);

		if (cross.z > 0)
			angle = 360 - angle;

		angle *= -1f;

		return angle;
	}
	
	public static void AnimateObject(string animatorAndAnimationName)
	{
		string[] options = ScenarioUtility.GetRestOfLine(animatorAndAnimationName,"[").TrimEnd(']').Split(',');
		string animatorName = options[0];
		string animationName = options[1];
		//Debug.Log("AnimateObject | "+animatorName +" | "+animationName);
		var Animator = GameObject.Find(animatorName).GetComponent<Animator>();
		Animator.Play(animationName);
	}
	  
	public static float WebGLDampenMovement(float value)
	{
		var returnValue = value;
		if (Mathf.Abs(returnValue) > 1000)
		{
			returnValue = returnValue / 2.0f;
		}
		if (Mathf.Abs(returnValue) > 100)
		{
			returnValue = returnValue / 2.0f;
		}
		if (Mathf.Abs(returnValue) > 50)
		{
			returnValue = returnValue / 2.0f;
		}

		if (Mathf.Abs(returnValue) > 35)
		{
			returnValue = Mathf.Sign(value) * 35.0f;
		}

		return returnValue;
	}
	
	//public static object GetBehaviourOfType (System.Type type)
	//{
	//	MonoBehaviour[] allScripts = GameObject.FindObjectsOfType<MonoBehaviour>();
	//	return allScripts.First(monoBehaviour=> {if(typeof(monoBehaviour) == type)return monoBehaviour as type;});
	//}
}
