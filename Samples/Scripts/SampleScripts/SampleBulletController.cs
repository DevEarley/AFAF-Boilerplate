using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleBulletController : MonoBehaviour
{
	public List<SampleBulletBehaviour> Bullet1Pool;
	public List<SampleBulletBehaviour> Bullet2Pool;

	public void SpawnBullet1(Vector3 target, Vector3 direction)
	{
		var bullet  = Bullet1Pool.FirstOrDefault(x=>x.enabled == false);
		if(bullet != null)
		{
			bullet.enabled = true;
			bullet.Restart(target,direction);
			
		}
		else
		{
			
			Bullet1Pool.ForEach(x=>x.enabled =false);
			Bullet1Pool[0].enabled = true;
			Bullet1Pool[0].Restart(target,direction);
		}
	}
	public void SpawnBullet2(Vector3 target, Vector3 direction)
	{
		var bullet  = Bullet2Pool.FirstOrDefault(x=>x.enabled == false);
		if(bullet != null)
		{
			bullet.enabled = true;
			bullet.Restart(target,direction);
		}
		else
		{
			Bullet2Pool.ForEach(x=>x.enabled =false);
			Bullet2Pool[0].enabled = true;
			Bullet2Pool[0].Restart(target,direction);
		}
	}


	
	

}
