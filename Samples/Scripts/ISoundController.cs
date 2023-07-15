using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISoundController
{
	public void SetVolume(float volume);
	
	public void PlayClip(AudioClip clip, int channel);
	
	public void PlayOneShot(GameSounds sound, int channel);
	
	public void PlayOnLoop(GameSounds sound, int channel);
	
	public void QueueUpSong(GameSounds sound,  bool looping);
	
	public void Stop(int channel);

	public void Play(GameSounds sound, int channel);
}