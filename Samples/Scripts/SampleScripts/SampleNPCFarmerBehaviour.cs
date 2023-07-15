using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleNPCFarmerBehaviour : MonoBehaviour, INPC
{
	public int Health = 6;
	public float MoveSpeed = 1.0f;
	public bool DropsShotgun = false;
	public bool Attacking;
	public bool Moving;
	public bool Idle;
	private SampleFXController FXController;
	private ISoundController SoundController;
	private RNGPoll NPCFarmerRNGPoll;
	private SampleTopdownPlayerController PlayerController;
	private RNGController RNGController;
	private SamplePickupController PickupController;
	public int id=0;
	private static float sucessRate=0.8f;
	private static float pollingInterval=1.2f;
	private GameObject PlayerBall;
	private SampleBulletController BulletController;
	private SampleLevelController LevelController;	public static float distance = 45.0f;
	private Vector3 Target;
	protected void Start()
	{
		PickupController = FindObjectOfType<SamplePickupController>();
	
		PlayerController = FindAnyObjectByType<SampleTopdownPlayerController>();
		FXController = FindAnyObjectByType<SampleFXController>();
		SoundController = GameUtility.GetAnyObjectThatImplementsInterface<ISoundController>();
		LevelController = FindAnyObjectByType<SampleLevelController>();
		BulletController = FindAnyObjectByType<SampleBulletController>();
		PlayerBall = GameObject.Find("PlayerBall");
		RNGController = FindAnyObjectByType<RNGController>();
		NPCFarmerRNGPoll = new RNGPoll(pollingInterval, sucessRate, id);
		
	}
	public void DoDamage()
	{
		Health --;
		if(Health <= 0)
		{
			FXController.ShowBlood(transform.position);
						
			//PickupController.SpawnBloodPickup(transform.position);
			PickupController.SpawnAmmoPickup(transform.position);
			if(DropsShotgun)
			{
				PickupController.SpawnShotgunPickup(transform.position);
								
			}
			SoundController.Play(GameSounds.EnemyScream,2);
			PlayerController.AddToScore(200);
	
			Destroy(gameObject);
		}
	}
	protected void Update()
	{
		Vector3 center = (PlayerBall.transform.position + transform.position) * 0.5F;
		Vector3 riseRelCenter = PlayerBall.transform.position - center;
		Vector3 setRelCenter = transform.position - center;
		
		Target = LevelController.index*distance*Vector3.forward + Vector3.Slerp(riseRelCenter, setRelCenter,Time.deltaTime*MoveSpeed);
		transform.position = Vector3.Lerp(transform.position, Target,Time.deltaTime);
		if(RNGController.PollRNG(NPCFarmerRNGPoll))
		{
			if(Idle)
			{
				Idle = false;
				Moving = true;
			}
			if(Attacking)
			{
				SoundController.Play(GameSounds.GunFire,1);
				
				transform.LookAt(PlayerBall.transform.position);
				var v1 = Quaternion.AngleAxis(-30, Vector3.up) * transform.forward;
				var v2 = Quaternion.AngleAxis(-20, Vector3.up) * transform.forward;
				var v3 = Quaternion.AngleAxis(-10, Vector3.up) * transform.forward;
				var v4 = Quaternion.AngleAxis(0, Vector3.up) * transform.forward;
				var v5 = Quaternion.AngleAxis(10, Vector3.up) * transform.forward;
				var v6 = Quaternion.AngleAxis(20, Vector3.up) * transform.forward;
				var v7 = Quaternion.AngleAxis(30, Vector3.up) * transform.forward;
			
				BulletController.SpawnBullet1(transform.position,v1);
				BulletController.SpawnBullet1(transform.position,v2);
				BulletController.SpawnBullet1(transform.position,v3);
				BulletController.SpawnBullet1(transform.position,v4);
				BulletController.SpawnBullet1(transform.position,v5);
				BulletController.SpawnBullet1(transform.position,v6);
				BulletController.SpawnBullet1(transform.position,v7);
			}
		}
		else if(Attacking)
		{
			Idle = true;
			
		}
	}
	
	protected void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Player")
		{
			Attacking = true;
		}
	}
}



