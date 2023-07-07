using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAnimationOnTrigger : MonoBehaviour
{
	
	public float Delay;
	public bool DestroyAfterDelay;
	public string AnimationClip;
	private Animator Animator;
	private bool Playing = false;
	protected void Awake()
	{
		Animator = gameObject.GetComponent<Animator>();
	}
	
	// OnTriggerExit is called when the Collider other has stopped touching the trigger.
	protected void OnTriggerEnter(Collider other)
	{
		if(other.tag == "player" && Playing == false)
		{
			
			StartCoroutine(PlayAnimation());
		}
	}
	private IEnumerator PlayAnimation()
	{
		Animator.Play(AnimationClip);
		yield return new WaitForSeconds(Delay);
		if(DestroyAfterDelay)
		{
			Destroy(gameObject);
		}
	}
}
