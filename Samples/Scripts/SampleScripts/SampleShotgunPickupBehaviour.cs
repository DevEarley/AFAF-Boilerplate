using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleShotgunPickupBehaviour : MonoBehaviour
{

	protected void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Player")
		{
			var Player  = FindObjectOfType<SampleTopdownPlayerController>();
			Player.OnGetShotgunPickup();
			this.gameObject.SetActive(false);
		}
	}
}
