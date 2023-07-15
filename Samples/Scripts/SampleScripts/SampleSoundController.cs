using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameSounds
{
	EnemyScream = 1,
	GunFire = 2,
	AquireAmmo=3,
	AquireGun=4,
	HitTarget=5,
	BuzzerStart = 6,
	BuzzerEnd = 7,
	AquireHealth = 8,
	MeleeAttack = 9,
	GunFire2 = 10,
	BulletShell = 11,
	
	FastTimer = 24,
	GrappleRelease = 25,
	Intro = 26,
	Jogging = 27,
	MoonBeam = 28,
	NormalTimer = 29,
	PullUpGrunt = 30,
	PuzzleComplete = 31,
	PuzzleFailed = 32,
	PuzzleStart = 33,
	Running = 34,
	SlowTimer = 35,
	Smash = 36,
	Walking = 37,
	WallJump = 38,
	WallSliding = 39,
	ChargeRelease = 40,
	ChargingUp = 41,
	DoubleJump = 42,
	Fire = 43,
	GrappleActivate = 44,
	GrappleReeling = 45,
	Jump = 46,
	Land = 47,
	Pickup = 48,
	Respawn = 49,
	Crunch = 50,
	TakeDamage =51,
	Heal=52,
	GameOver=53,
	Bat=54,
	ShiftingStones=55,
	StonesSettleLoud=56
    
}

public class SampleSoundController : ISoundController
{
    public AudioClip Pickup;
	public AudioClip Land;
	public AudioClip Bee;
    public AudioClip Respawn;
    public AudioClip Jump;
    public AudioClip DoubleJump;
    public AudioClip ChargeRelease;
    public AudioClip ChargingUp;
    public AudioClip Fire;
	public AudioClip Smash;
	public AudioClip MoonBeam;
	public AudioClip GrappleActivate;
    public AudioClip GrappleReeling;
    public AudioClip GrappleRelease;
	public AudioClip WallSliding;
	public AudioClip WallJump;
	public AudioClip Walking;
	public AudioClip Jogging;
	public AudioClip Running;
	public AudioClip Jumping;
	public AudioClip PullUpGrunt;
	public AudioClip PuzzleStart;
	public AudioClip PuzzleComplete;
	public AudioClip PuzzleFailed;
	public AudioClip Intro;

	public AudioClip Crunch;
	public AudioClip TakeDamage;
	public AudioClip Heal;
	public AudioClip GameOver;
	public AudioClip Bat;
	public AudioClip ShiftingStones;
	public AudioClip StonesSettleLoud;
	
	
	
	private AudioClip GetSound(GameSounds sound)
	{
		AudioClip newClip;
		switch (sound)
		{	
		default:
		case GameSounds.Pickup: newClip = Pickup; break;
		case GameSounds.Crunch: newClip = Crunch; break;
		case GameSounds.Walking: newClip = Walking; break;
		case GameSounds.Jogging: newClip = Jogging; break;
		case GameSounds.Running: newClip = Running; break;
		case GameSounds.Land: newClip = Land; break;
		case GameSounds.MoonBeam: newClip = MoonBeam; break;
		case GameSounds.Respawn: newClip = Respawn; break;
		case GameSounds.Jump: newClip = Jump; break;
		case GameSounds.DoubleJump: newClip = DoubleJump; break;
		case GameSounds.ChargeRelease: newClip = ChargeRelease; break;
		case GameSounds.ChargingUp: newClip = ChargingUp; break;
		case GameSounds.Fire: newClip = Fire; break;
		case GameSounds.Smash: newClip = Smash; break;
		case GameSounds.GrappleActivate: newClip = GrappleActivate; break;
		case GameSounds.GrappleReeling: newClip = GrappleReeling; break;
		case GameSounds.GrappleRelease: newClip = GrappleRelease; break;
		case GameSounds.WallSliding: newClip = WallSliding; break;
		case GameSounds.WallJump: newClip = WallJump; break;
		case GameSounds.PullUpGrunt: newClip = PullUpGrunt; break;
		case GameSounds.PuzzleStart: newClip = PuzzleStart; break;
		case GameSounds.PuzzleComplete: newClip = PuzzleComplete; break;
		case GameSounds.PuzzleFailed: newClip = PuzzleFailed; break;
		case GameSounds.TakeDamage: newClip = TakeDamage; break;
		case GameSounds.GameOver: newClip = GameOver; break;
		case GameSounds.Heal: newClip = Heal; break;

		case GameSounds.Bat: newClip = Bat; break;
		case GameSounds.ShiftingStones: newClip = ShiftingStones; break;
		case GameSounds.StonesSettleLoud: newClip = StonesSettleLoud; break;
		case GameSounds.Intro: newClip = Intro; break;
		}
		return newClip;
	}
	
	private bool QueueMode = false;
	public List<AudioSource> Channels;
	public void SetVolume(float volume)
	{
		Channels.ForEach(x=>x.volume = volume);
	}
	
	public void PlayClip(AudioClip clip, int channel)
	{
		var AudioSource = Channels[channel];
		AudioSource.loop = false;
		AudioSource.PlayOneShot(clip);
	}
   
	
	public void PlayOneShot(GameSounds sound, int channel)
	{ 
		var AudioSource = Channels[channel];
		AudioSource.loop = false;
		AudioClip newClip = GetSound(sound);
		AudioSource.PlayOneShot(newClip);
	}
   
	public void PlayOnLoop(GameSounds sound, int channel)
	{
		var AudioSource = Channels[channel];
		AudioClip newClip = GetSound(sound);
		if(AudioSource.clip!=null && AudioSource.clip.name == newClip.name)return;
		AudioSource.clip = newClip;
		AudioSource.Stop();
		AudioSource.Play();
		AudioSource.loop = true;
	}
	
	public void QueueUpSong(GameSounds sound,  bool looping)
	{
		//start queue mode
		// if no song in queue, add it and play it.
		// if song in queue, add it.
		// when song is complete, pop queue. If last song, stop queue mode.
		int channel = 2;
		var AudioSource = Channels[channel];
		AudioClip newClip = GetSound(sound);
		if(AudioSource.clip!=null && AudioSource.clip.name == newClip.name)return;
		AudioSource.clip = newClip;
		AudioSource.Stop();
		AudioSource.Play();
		AudioSource.loop = looping;	
	}
	
	void Update(){
		if(QueueMode)
		{
			int channel = 2;
			var AudioSource = Channels[channel];
			if(AudioSource.isPlaying){return;}
			else
			{
				//pop and play next song.
			}
		}
	}
		
	public void Stop(int channel)
	{
		var AudioSource = Channels[channel];
		AudioSource.clip = null;
		AudioSource.Stop();
		AudioSource.loop = false;
	}

	public void Play(GameSounds sound, int channel)
	{
		var AudioSource = Channels[channel];
		AudioClip newClip = GetSound(sound);
		AudioSource.clip = newClip;
		AudioSource.Stop();
		AudioSource.Play();
		AudioSource.loop = false;
	
	
	}
}