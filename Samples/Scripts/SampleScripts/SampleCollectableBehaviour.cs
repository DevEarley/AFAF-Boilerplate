using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]

public enum CollectableTypes
{
	WarpGemShard,
	WarpGem,
	HoneyNut,
	MilkThistle,
	WallNut,
	MoonFlowerPedals,
	BlueTeaLeaves,
	RedTeaLeaves,
	
}

[System.Serializable]

public class Collectable
{
	public Collectable(string id, CollectableTypes _type)
	{
		ID = id;
		type = _type;
	}
	public string ID;
	public CollectableTypes type = CollectableTypes.WarpGemShard;
}
public class SampleCollectableBehaviour : MonoBehaviour
{
	[HideInInspector]
	public string ID;
	
	public bool MoveTowardPlayer = true;
	
	public CollectableTypes type;
	
	private bool CollectedAlready = false;
	private bool WasCollectedInPast = false;
	private DataRepository DataRepository;
		
	public bool Animated = true;
	public float Delay = 1.5f;
	public static float Speed = 25.0f;
	public bool DestroyAfterDelay = false;
	public string DefaultAnimationClip;
	public string AlreadyCollectedAnimation;
	
	private Animator Animator;
	//private bool Playing = false;
	
	private bool MovingTowardPlayer = false;
	
	protected void Awake()
	{
		if(Animated)
			Animator = gameObject.GetComponent<Animator>();
	}
	void Start()
	{
		var activeScene =SceneManager.GetActiveScene();
		var gameObjectName = gameObject.name;
		ID = activeScene.name+"_"+gameObjectName;
		//check data for existence 
		DataRepository = GameObject.FindAnyObjectByType<DataRepository>();
		DataRepository.Subscribers.Add(this.gameObject);
		//Player = GameObject.FindAnyObjectByType<PlayerController>();
	}
	void Update()
	{
		if(MovingTowardPlayer)
		{
			//	transform.position = Vector3.Lerp(transform.position, Player.PlayerBallRigidbody.transform.position,Mathf.Clamp01(Speed*Time.deltaTime));
		}
	}
	private IEnumerator PlayAnimation()
	{
		if(Animated)
			Animator.Play(DefaultAnimationClip);
		yield return new WaitForSeconds(Delay);
		if(DestroyAfterDelay)
		{
			Destroy(gameObject);
		}
	}

	public void OnDataLoaded()
	{
		WasCollectedInPast = DataRepository.WasThisCollectedInPastGame(ID);
		if(WasCollectedInPast)
		{
			if(Animated)
				Animator.Play(AlreadyCollectedAnimation);
		}
	}
	
	protected void OnTriggerEnter(Collider other)
	{
		if(other.tag =="Player" && CollectedAlready == false)
		{
			//	Player.OnCollectedItem(type);
			if(MoveTowardPlayer)
				MovingTowardPlayer = true;
			CollectedAlready = true;
			var SoundController = GameUtility.GetAnyObjectThatImplementsInterface<ISoundController>();
			SoundController.PlayOneShot(GameSounds.Pickup,1);
			StartCoroutine(PlayAnimation());
		}
		 if( WasCollectedInPast == false)
		{
			var collectable = new Collectable(ID,type);
			DataRepository.CollectItem(collectable);
		}
	}
}
