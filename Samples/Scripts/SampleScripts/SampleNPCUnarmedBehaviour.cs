using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleNPCUnarmedBehaviour : MonoBehaviour, INPC
{
	[HideInInspector]
	public int Health = 2;
	private NPCUnarmedStates NPCUnarmedState = NPCUnarmedStates.normal;
	private NPCUnarmedMovementStates NPCUnarmedMovementState = NPCUnarmedMovementStates.idle;
	private RNGPoll NPCUnarmedRNGPoll;
	private SampleFXController FXController;
	private RNGController RNGController;
	private SampleTopdownPlayerController PlayerController;
	private SamplePickupController PickupController;
	private ISoundController SoundController;
	public int id=0;
	private static float  sucessRate=0.5f;
	private static float  pollingInterval=0.0f;
	// Start is called on the frame when a script is enabled just before any of the Update methods are called the first time.
	protected void Start()
	{
		PlayerController = FindAnyObjectByType<SampleTopdownPlayerController>();
		RNGController = FindAnyObjectByType<RNGController>();
		NPCUnarmedRNGPoll = new RNGPoll(pollingInterval, sucessRate, id);
		PickupController = FindObjectOfType<SamplePickupController>();
		SoundController = GameUtility.GetAnyObjectThatImplementsInterface<ISoundController>();
		
		FXController = FindAnyObjectByType<SampleFXController>();
	}
	// Update is called every frame, if the MonoBehaviour is enabled.
	protected void Update()
	{
	}
	public void DoDamage()
	{
		Health --;
		if(Health<=0)
		{
							
			if(RNGController.PollRNG(NPCUnarmedRNGPoll))
			{
				PickupController.SpawnAmmoPickup(transform.position);

			}
			
		FXController.ShowBlood(transform.position);
		PickupController.SpawnBloodPickup(transform.position);
		SoundController.Play(GameSounds.EnemyScream,2);
			PlayerController.AddToScore(50);
							
		Destroy(gameObject);
		}
	}
}



public enum NPCUnarmedStates
{
	normal,
	scared
}

public enum NPCUnarmedMovementStates
{
	running,
	idle
}
