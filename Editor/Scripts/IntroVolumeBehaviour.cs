using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroVolumeBehaviour : MonoBehaviour
{
 // OnTriggerExit is called when the Collider other has stopped touching the trigger.
	protected void OnTriggerExit(Collider other)
	{
		if(other.tag == "player")
		{
			//Debug.Log("OnExitIntroVolume");
			var lootLocker = FindAnyObjectByType<LootLockerService>();
			lootLocker.OnExitIntroVolume();
			Destroy(this.gameObject);
			
		}
	}
}

