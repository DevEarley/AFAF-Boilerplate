using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public static class RNGService 
{

	public static List<int> Shuffle (List<int> list)  
	{  
		for(var i = 0;i<list.Count;i++)
		{
			decimal a = i * 15485863;
			a = (a * a * a % 2038074743) / 2038074743;
			if(a<0)a*=-1;
			var fa = Mathf.Clamp01((float)a);
			int aIndex = (int)(fa * list.Count);
			int value = list[aIndex];  
			list[aIndex] = list[i];  
			list[i] = value;  
		}
		
		return list;
	}
	
	
}
