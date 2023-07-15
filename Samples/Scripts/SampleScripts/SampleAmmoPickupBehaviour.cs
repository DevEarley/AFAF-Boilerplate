using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleAmmoPickupBehaviour : MonoBehaviour
{
	protected void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Player")
		{
			var Player  = FindObjectOfType<SampleTopdownPlayerController>();
			Player.OnGetAmmoPickup();
			this.gameObject.SetActive(false);
		}
	}
}
