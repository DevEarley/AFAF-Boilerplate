using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobAndRotateBehaviour : MonoBehaviour
{
	private static float SpinSpeed = 120.0f;
	private static float BobHeight = 0.3f;
	private  float BobAngle = 0.0f;
	private static float BobSpeed = 2.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
	{
		BobAngle+=Time.deltaTime * BobSpeed;
		if(BobAngle>1)BobAngle=-1;
		gameObject.transform.Rotate(Vector3.up *Time.deltaTime* SpinSpeed);
		gameObject.transform.position = new Vector3(gameObject.transform.position.x, Mathf.Cos(BobAngle ) *BobHeight,gameObject.transform.position.z);
    }
}
