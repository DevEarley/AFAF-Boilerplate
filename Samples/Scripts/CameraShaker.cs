using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShaker : MonoBehaviour
{
	private ISoundController SoundController;
	public float ShakeAmount = 0.0f;
	private bool ShakeEnabled;
	private static float PitchMultiplier = 10.0f;
	public float OriginalRotationSpeed = 1.0f;
	private Quaternion OriginalRotation = Quaternion.identity;
    void Start()
    {
	    SoundController = GameUtility.GetAnyObjectsThatImplementsInterface<ISoundController>()[0];
    }

    // Update is called once per frame
    void Update()
	{
		if(ShakeEnabled==false)return;
		
		var pitch = PitchMultiplier * ShakeAmount * GetPerlinNoise(20);
		//ServiceLocator.PlayerController.PlayerCamera.transform.Rotate(new Vector3(0.0f,pitch,0.0f)*Time.deltaTime,Space.Self);
		//ServiceLocator.PlayerController.PlayerCamera.transform.rotation = Quaternion.Lerp(ServiceLocator.PlayerController.PlayerCamera.transform.rotation,OriginalRotation, OriginalRotationSpeed *Time.deltaTime);
    }
    
	public void StartShaking()
	{
		ShakeEnabled = true;
		//ServiceLocator.PlayerController.PlayerState  = PlayerStates.Locked;
		//OriginalRotation = ServiceLocator.PlayerController.PlayerCamera.transform.rotation;

	}
    
	public void StopShaking()
	{
		ShakeEnabled = false;
	}
    
	private float GetPerlinNoise(int seed)
	{
		return Mathf.PerlinNoise(Time.time*seed,seed)-0.5f;
	}
	public void PlayLoopedSound(GameSounds sound)
	{
		SoundController.PlayOnLoop(sound,1);
	}
	public void PlaySound(GameSounds sound)
	{
		SoundController.PlayOneShot(sound,1);
	}
}
