using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextFXWavvy : MonoBehaviour
{
	public float WaveIndex;
	private static float WaveScale = 0.05f;
	private Vector3 OriginalPosition;
	public void SetOriginalPosition()
	{
		OriginalPosition = transform.localPosition;
	}
    // Update is called once per frame
    void Update()
    {
	    //move up and down
	    gameObject.transform.localPosition = OriginalPosition + new Vector3(0, Mathf.Sin((WaveIndex+Time.timeSinceLevelLoad)%360.0f)* WaveScale,0);
    }
}
