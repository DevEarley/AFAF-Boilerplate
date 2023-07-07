using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IControlInput
	{
		public void ResetInputThrottle();
		public bool CheckInputThrottle();
		public bool WasAnyButtonPressed();
		public bool WasAnyButtonReleased();
	}
