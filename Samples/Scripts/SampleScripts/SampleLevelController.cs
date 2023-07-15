using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleLevelController : MonoBehaviour
{
	public GameObject ShotgunFarmerPrefab;
	public GameObject FarmerPrefab;
	public GameObject UnarmedPrefab;
	private SampleTopdownPlayerController PlayerController;
	private GameObject PlayerCamera;
	private Rigidbody PlayerBall;
	public static float distance = 45.0f;
	//private static float distance = 20.0f;
	public List<LevelModel>Levels;
	private GameObject CurrentLevel;
	private GameObject LastLevel;
	private GameObject NextLevel;
	private RNGController RNGController;
	private RNGPoll RNGPoll;
	public int index = 0;
	private float Transition_Time = 1.0f;
	private float Transition_CameraMovingTime = 1.0f;
	private bool Transition_CameraIsMoving = false;
	private float Transition_CameraSpeed = 100.0f;
	private float Transition_CameraDirection = 1.0f;

	// Start is called on the frame when a script is enabled just before any of the Update methods are called the first time.
	protected void Start()
	{
		RNGPoll = new RNGPoll(0.0f,0.5f,1);
		RNGController = FindObjectOfType<RNGController>();
		PlayerController = FindObjectOfType<SampleTopdownPlayerController>();
		PlayerCamera = GameObject.Find("PlayerCamera");
		PlayerBall = GameObject.Find("PlayerBall").GetComponent<Rigidbody>();
	}
	// Update is called every frame, if the MonoBehaviour is enabled.
	protected void Update()
	{
		if(Transition_CameraIsMoving)
		{
			PlayerCamera.transform.position += Time.deltaTime*Vector3.forward*Transition_CameraSpeed*Transition_CameraDirection;
			if((index*distance - PlayerCamera.transform.position.z )*Transition_CameraDirection<0)
			{
				PlayerCamera.transform.position = new Vector3(PlayerCamera.transform.position.x,PlayerCamera.transform.position.y,index*distance);
				Transition_CameraIsMoving = false;
			}
		}
	}
	
	public IEnumerator LoadLevel(int newIndex,Transform oldLevel,bool fromLeft) 
	{
		index = newIndex;
		var nextLevel = Levels[index];
		PlayerController.Locked = true;
		var newLevel = GameObject.Instantiate(nextLevel.LevelGeometry,new Vector3(0,0,distance*index), Quaternion.identity);
		PlayerBall.isKinematic = true;
		if(fromLeft)
		{
			
			PlayerBall.transform.position = newLevel.gameObject.transform.Find("PlayerSpawnFromLeft").transform.position;
		}
		else
		{
			PlayerBall.transform.position = newLevel.gameObject.transform.Find("PlayerSpawnFromRight").transform.position;
			
		}
		Transition_CameraIsMoving = true;
	
		DestroyOldNPCs();
		Debug.Log(index);
		yield return new WaitForSeconds(Transition_Time);
		GameObject.Destroy(oldLevel.gameObject);
		yield return new WaitForSeconds(0.01f);
		
		LoadFarmersAndUnarmedNPCs();
		PlayerBall.isKinematic = false;
		PlayerController.Locked = false;
	}
	private void LoadFarmersAndUnarmedNPCs()
	{
		var nextLevel = Levels[index];
		LoadNewNPCs("FarmerSpawn",ShotgunFarmerPrefab, nextLevel.NumberOfShotgunFarmers);
		LoadNewNPCs("FarmerSpawn",FarmerPrefab, nextLevel.NumberOfFarmers);
		LoadNewNPCs("UnarmedSpawn",UnarmedPrefab,nextLevel.NumberOfUnarmed);
	}
	private static float OffsetSpawnDistance =0.1f;
	private void LoadNewNPCs( string tag, GameObject prefab, int numberOfNPCs)
	{
		
		var spawns = GameObject.FindGameObjectsWithTag(tag);
	
		var SpawnIndex = 0;
		var SpawnOffset = 0;
		for(var i = 0; i<numberOfNPCs;i++)
		{
			if(RNGController.PollRNG(RNGPoll))
			{
				SpawnIndex++;
			
				if(SpawnIndex>spawns.Length-1)
				{
					SpawnIndex = 0;
				}
			}
			
			if(RNGController.PollRNG(RNGPoll))
			{
				SpawnOffset++;
			}
			else
			{
				SpawnOffset--;
			}
			var offset = Vector3.zero;
			if(RNGController.PollRNG(RNGPoll))
			{
				offset+=SpawnOffset*Vector3.forward*OffsetSpawnDistance;
			}
			else
			{
				offset+=SpawnOffset*Vector3.right*OffsetSpawnDistance;
			}
			var spawn = spawns[SpawnIndex];
			var offsetPosition = spawn.transform.position + offset;
			Instantiate(prefab, offsetPosition,Quaternion.identity);
		}
		
	}
	
	public void LoadPreviousLevel(Transform oldLevel)
	{
		var newIndex = index-1;
		Transition_CameraDirection = -1.0f;
		
		if(newIndex<0)
		{
			return;
		}
		StartCoroutine(LoadLevel(newIndex,oldLevel,true));
	}
	
	public void LoadNextLevel(Transform oldLevel) 
	{
		Transition_CameraDirection = 1.0f;
		var newIndex = index+1;
		if(Levels.Count-1<newIndex) {
			
			return;
		}
		StartCoroutine(LoadLevel(newIndex,oldLevel,false));
	}

	private void DestroyOldNPCs()
	{
		MonoBehaviour[] allScripts = FindObjectsOfType<MonoBehaviour>();
		for (int i = 0; i < allScripts.Length; i++)
		{
			if(allScripts[i] is INPC)
			{
				GameObject.Destroy(allScripts[i].gameObject);
			}			
		}
	}
}

[System.Serializable]
public class LevelModel
{
	public string name;
	public int index;
	public int NumberOfFarmers;
	public int NumberOfShotgunFarmers;
	public int NumberOfUnarmed;
	public GameObject LevelGeometry;
}