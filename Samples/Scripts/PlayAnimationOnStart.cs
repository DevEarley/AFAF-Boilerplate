using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAnimationOnStart : MonoBehaviour
{
	public string Animation;
	private Animator Animator;
	// Awake is called when the script instance is being loaded.
	protected void Awake()
	{
		Animator = gameObject.GetComponent<Animator>();
		Animator.Play(Animation);
	}
}
