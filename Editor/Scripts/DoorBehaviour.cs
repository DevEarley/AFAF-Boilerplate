using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;


public class DoorBehaviour : MonoBehaviour
{
	public SceneTransitionTypes SceneTransitionTypes;

	public bool available = true;
	public bool dataReady = false;
	
	public string TargetSceneString;
	public string TargetSpawn;
	private SceneTransitioner SceneTransitioner;
	
	private DataRepository DataRepository;
	
	void Start()
	{
		DataRepository = GameObject.FindAnyObjectByType<DataRepository>();
		DataRepository.Subscribers.Add(this.gameObject);
	}
	public void OnDataLoaded()
	{
		dataReady =  true;
	}
	
	protected void OnTriggerEnter(Collider other)
	{
		if(other.tag == "player")
		{
			if(available==false)return;
			available = false;
		
			SceneTransitioner = GameObject.FindAnyObjectByType<SceneTransitioner>();
			
			DataRepository.SaveGame();
			
			SceneTransitioner.TransitionToNextScene(TargetSceneString,TargetSpawn, SceneTransitionTypes, this.gameObject);
		}
	}
	
	
	//private bool WaitingForScene = false;
}