using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxCameraBehaviour : MonoBehaviour
{
	private GameObject PlayerCamera;
    // Start is called before the first frame update
    void Start()
    {
	    PlayerCamera=   GameObject.Find("PlayerCamera");
    }

    // Update is called once per frame
	void LateUpdate()
	{
		if(PlayerCamera != null)
		{
			transform.rotation = PlayerCamera.transform.rotation;
		}
		else
		{
			PlayerCamera=	GameObject.Find("PlayerCamera");
			
		}
    }
}
