using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SamplePickupController : MonoBehaviour
{
	public List<MeshRenderer> BloodPickups;
	
	public List<GameObject> ShotgunPickups;
	public List<GameObject> AmmoPickups;
	

	public void SpawnShotgunPickup(Vector3 target)
	{
		var pickup  = ShotgunPickups.FirstOrDefault(x=>x.active == false);
		if(pickup == null)
		{
			//oops
		}
		else
		{
			pickup.transform.position = target;
			pickup.SetActive(true);

		}
	}
	
	public void SpawnAmmoPickup(Vector3 target)
	{
		var pickup  = AmmoPickups.FirstOrDefault(x=>x.active == false);
		if(pickup == null)
		{
			//oops
		}
		else
		{
			pickup.transform.position = target;
			pickup.SetActive(true);

		}
	}
	
	public void SpawnBloodPickup(Vector3 target)
	{
		if(BloodPickups == null || BloodPickups.All(x=>x.enabled))return;
		var pickup  = BloodPickups.FirstOrDefault(x=>x.enabled == false);
		if(pickup == null)
		{
			//oops
		}
		else
		{
			pickup.transform.position = target;
			pickup.enabled = (true);
		}
	}
}
