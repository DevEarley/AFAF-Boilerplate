using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class SampleTopdownGameController : MonoBehaviour
{
	
	private InputController InputController;
	//public Texture2D crosshair; 

	void Start(){
    
		InputController = FindAnyObjectByType<InputController>();
		//Vector2 cursorOffset = new Vector2(crosshair.width/2, crosshair.height/2);
		//	Cursor.SetCursor(crosshair, cursorOffset, CursorMode.Auto);
	
    }
	
	
    void Update()
    {
	     LockMouseIfNeeded();
    }
	private void LockMouseIfNeeded()
	{
		if (
			InputController.WasStartReleased
			&& Cursor.lockState != CursorLockMode.None
		)
		{
			Cursor.lockState = CursorLockMode.None;
		}
		else
		{
			Cursor.lockState = CursorLockMode.Confined;
		}
	}

}
