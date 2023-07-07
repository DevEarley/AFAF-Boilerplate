using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextFXOutline : MonoBehaviour
{
	
    void Start()
    {
	    Material newMat = Resources.Load("/Materials/SpriteOutline", typeof(Material)) as Material;
	    gameObject.GetComponent<SpriteRenderer>().material = newMat;
    }
}
