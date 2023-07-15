using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SampleFXController : MonoBehaviour
{
	private static float ExplosionTime = 1.5f;
	private static float BloodFXTime = 1.5f;
	public List<AnimateUVs> ExplosionAnimators;
	public List<AnimateUVs> BloodFXAnimators;
	private InputController InputController;
	// Start is called before the first frame update
	void Start()
	{
		InputController = FindAnyObjectByType<InputController>();
	}
	private IEnumerator DisableAnimatorAfterTime(AnimateUVs animator,float someTime)
	{
		yield return new WaitForSeconds(someTime);
		animator.gameObject.SetActive(false);
	}
	public void ShowExplosion(Vector3 target)
	{
		var animator = ExplosionAnimators.FirstOrDefault(x=>x.gameObject.active == false);
		if(animator == null)
		{
			ExplosionAnimators[0].enabled = true;
			ExplosionAnimators[0].gameObject.SetActive(true);
			ExplosionAnimators[0].transform.position = target;
			ExplosionAnimators[0].Play("explode");
			StartCoroutine(DisableAnimatorAfterTime(ExplosionAnimators[0],ExplosionTime));
		}
		else
		{
			animator.enabled = true;
			animator.gameObject.SetActive(true);
			animator.transform.position = target;
			animator.Play("explode");
			StartCoroutine(DisableAnimatorAfterTime(animator,ExplosionTime));
			
		}
	}
	
	public void ShowBlood(Vector3 target)
	{
	
		var animator = BloodFXAnimators.FirstOrDefault(x=>x.gameObject.active == false);
		if(animator == null)
		{
			BloodFXAnimators[0].enabled = true;
			BloodFXAnimators[0].gameObject.SetActive(true);
			BloodFXAnimators[0].transform.position = target;
			BloodFXAnimators[0].Play("blood-squirt");
			StartCoroutine(DisableAnimatorAfterTime(BloodFXAnimators[0],BloodFXTime));
		}
		else
		{
			animator.enabled = true;
			animator.gameObject.SetActive(true);
			animator.transform.position = target;
			animator.Play("blood-squirt");
			StartCoroutine(DisableAnimatorAfterTime(animator,BloodFXTime));
			
		}
	}
	
}
