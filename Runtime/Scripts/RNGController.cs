using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RNGPoll
{
	public RNGPoll( float _pollingInterval, float _sucessRate, int _RNG_ID)
	{
		pollingInterval = _pollingInterval;
		sucessRate = _sucessRate;
		RNG_ID = _RNG_ID;
	}
	public float pollingInterval;
	public float sucessRate;
	public float timer = 0.0f;
	public int RNG_ID;
}

public class RNGController : MonoBehaviour
{
	private int index = 0;
	private int lastLargestIndex = 0;
	private int max_index = 1000; 
	private List <int> values = new List<int>();
	
	public int Next(int RNG_ID)
	{
		var offsetIndex = RNG_ID + index;
		if (offsetIndex >= max_index) {
			offsetIndex = offsetIndex - max_index;
		}
		lastLargestIndex = Mathf.Max(lastLargestIndex,offsetIndex);
		return values[offsetIndex];
	}
	
	public bool PollRNG (RNGPoll poll)
	{
		poll.timer += Time.deltaTime;
		if(poll.timer > poll.pollingInterval)
		{
			poll.timer = poll.timer - poll.pollingInterval;
			var number = Next(poll.RNG_ID);
			if(number < poll.sucessRate * max_index)
			{
				return true;
			}
			return false;
			
		}
		return false;
	}

	void Awake()
	{
		for(var i = 0;i<max_index;i++)
		{
			values.Add(i);
		}
		values = RNGService.Shuffle(values);
	}
	
	void LateUpdate()
	{
		index = lastLargestIndex;
	}
}
