using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleNextLevelTrigger : MonoBehaviour
{
	public int CurrentLevelIndex;
	private SampleLevelController LevelController;
	private bool IgnoringPlayer = false;
	private bool FirstTime = true;
	// When a GameObject collides with another GameObject, Unity calls OnTriggerEnter.
	protected void OnTriggerExit(Collider other)
	{
		if(other.tag == "Player" && IgnoringPlayer == false)
		{
			
			LevelController.LoadNextLevel(this.transform.parent);
			StartCoroutine(DisableThis());
		}
	}
	IEnumerator DisableThis()
	{
		IgnoringPlayer = true;
		yield return new WaitForSeconds(1.0f);
		IgnoringPlayer = false;
		
	}
    // Start is called before the first frame update
    void Start()
    {
	    LevelController = FindObjectOfType<SampleLevelController>();
    }

  
}
