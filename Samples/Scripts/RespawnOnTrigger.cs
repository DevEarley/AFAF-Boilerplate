using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnOnTrigger : MonoBehaviour
{
	
	
	// When a GameObject collides with another GameObject, Unity calls OnTriggerEnter.
	protected void OnTriggerEnter(Collider other)
	{
		if(other.tag == "player")
		{
			var PlayerBall = GameObject.Find("PlayerBall").GetComponent<Rigidbody>();
			var PlayerController = GameObject.Find("PlayerController");
			PlayerBall.isKinematic = true;
			PlayerBall.transform.position = PlayerController.transform.position;
			PlayerBall.isKinematic = false;
			
		}
	}
}
