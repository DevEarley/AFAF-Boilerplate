using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextFXSmall : MonoBehaviour
{
	private static float smallSize = 0.5f;
    void Start()
    {
	    gameObject.transform.localScale = Vector3.one * smallSize;       
    }

 
}
