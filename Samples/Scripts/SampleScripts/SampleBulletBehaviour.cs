using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleBulletBehaviour : MonoBehaviour
{
	private MeshRenderer MeshRenderer;
	private SamplePickupController PickupController;
	private SampleFXController FXController;
	private ISoundController SoundController;
	private SampleTopdownPlayerController PlayerController;
	public Vector3 Direction;
	private float acceleration = 150.0f;
	private float initial_speed = 150.0f;
	
	private float farmer_acceleration = 25.0f;
	private float farmer_initial_speed = 10.0f;
	
	private float speed = 1.0f;
	public Vector3 PreviousPosition;
	public bool TargetingPlayer = false;
	
	public void Start()
	{
		MeshRenderer = GetComponent<MeshRenderer>();
		PickupController = FindObjectOfType<SamplePickupController>();
		SoundController = GameUtility.GetAnyObjectsThatImplementsInterface<ISoundController>()[0];
		PlayerController = FindAnyObjectByType<SampleTopdownPlayerController>();
		
		FXController = FindAnyObjectByType<SampleFXController>();
	}
	 
	public void Restart(Vector3 NewPosition, Vector3 NewDirection)
	{
		if(MeshRenderer == null)MeshRenderer = GetComponent<MeshRenderer>();
		if(MeshRenderer == null)MeshRenderer = GetComponentInChildren<MeshRenderer>();
		MeshRenderer.enabled = true;
		Direction = NewDirection;
		transform.position = NewPosition;
		PreviousPosition = NewPosition;
		if(TargetingPlayer==false)
		{
			speed = initial_speed;
		}
		else
		{
			speed = farmer_initial_speed;
		}
	}
	
	public void Update()
	{
		speed += acceleration *Time.deltaTime *0.5f;
		transform.position += Direction *speed *Time.deltaTime;
		speed += acceleration *Time.deltaTime *0.5f;
		
		RaycastHit[] hits = Physics.RaycastAll(new Ray(PreviousPosition,Direction),(PreviousPosition-transform.position).magnitude);
		if(hits.Length>0)
		{
		
			foreach(var hit in hits)
			{
				if(hit.collider.tag =="Player" && TargetingPlayer)
				{
					FXController.ShowExplosion(hit.point);
					MeshRenderer.enabled = false;
					PlayerController.DoDamage();
					this.enabled = false; 
				}
				else if(hit.collider.tag =="Enemy"&& TargetingPlayer==false)
				{
					FXController.ShowBlood(hit.point);
					MeshRenderer.enabled = false;
					var unarmed = hit.collider.gameObject.GetComponent<SampleNPCUnarmedBehaviour>();
					var farmer = hit.collider.gameObject.GetComponent<SampleNPCFarmerBehaviour>();
					if(unarmed != null)
					{
						PlayerController.AddToScore(1);
						
						unarmed.DoDamage();
					}
					if(farmer != null)
					{
						PlayerController.AddToScore(1);
						
						farmer.DoDamage();	
					}
					this.enabled = false;
					
				}
				else if(hit.collider.tag !="Player")
				{
					FXController.ShowExplosion(hits[0].point);
					MeshRenderer.enabled = false;
				
					this.enabled = false;
				}
			}
		
		}
	}
}
