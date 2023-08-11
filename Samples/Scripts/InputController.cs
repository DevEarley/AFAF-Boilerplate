using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public enum InputControllerState
{
	NO_INPUT_DEVICE_CONNECTED,
	NO_CONTROLLER_CONNECTED,
	NO_KEYBOARD_CONNECTED,
	BOTH_KEYBOARD_AND_CONTROLLER_CONNECTED
}
public class InputController : MonoBehaviour, IControlInput
{
	[HideInInspector]
	public List<GameObject> Subscribers = new List<GameObject>();
	[HideInInspector]
	public Vector2 MousePositionOnScreen;
	[HideInInspector]
	public Vector2 LookInputVector;
	[HideInInspector]
	public Vector2 RawLookInputVector;
	[HideInInspector]
	public Vector2 RawMovementInputVector;
	
	[HideInInspector]
	public Vector2 MovementInputVector;
	
	[HideInInspector]
	public Vector2 DPadVector;
	[HideInInspector]
	public bool WasSouthButtonReleased;
	[HideInInspector]
	public bool WasSouthButtonPressed;
	[HideInInspector]
	public bool WasPauseReleased;
	[HideInInspector]
	public bool WasPausePressed;
	[HideInInspector]
	public bool WasWestButtonReleased;
	[HideInInspector]
	public bool WasWestButtonPressed;
	[HideInInspector]
	public bool WasEastButtonReleased;
	
	[HideInInspector]
	public bool WasEastButtonPressed;
	[HideInInspector]
	public bool IsEastButtonDown;
	[HideInInspector]
	public bool IsWestButtonDown;
	[HideInInspector]
	public bool IsNorthButtonDown;
	[HideInInspector]
	public bool IsSouthButtonDown;
	[HideInInspector]
	public bool WasNorthButtonReleased; 
	[HideInInspector]
	public bool WasNorthButtonPressed;
	
	
	[HideInInspector]
	public bool IsLeftBumperPressed; 
	[HideInInspector]
	public bool WasLeftBumperPressed; 
	[HideInInspector]
	public bool WasLeftBumperReleased;

	[HideInInspector]
	public bool IsRightBumperPressed; 
	[HideInInspector]
	public bool WasRightBumperPressed; 
	[HideInInspector]
	public bool WasRightBumperReleased;
	
	
	[HideInInspector]
	public bool IsLeftTriggerPressed; 
	[HideInInspector]
	public bool WasLeftTriggerPressed; 
	[HideInInspector]
	public bool WasLeftTriggerReleased;

	[HideInInspector]
	public bool IsRightTriggerPressed; 
	[HideInInspector]
	public bool WasRightTriggerPressed; 
	[HideInInspector]
	public bool WasRightTriggerReleased;
	

	public InputControllerState InputControllerState;
	public Gamepad gamepad;
	public Keyboard keyboard;
	[HideInInspector]
	public float MouseSensitivity_y = 0.0008f; //TODO - move to data model & get from repo[HideInInspector]
	[HideInInspector]
	public float MouseSensitivity_x = 0.004f; //TODO - move to data model & get from repo
	//private static float dampen_webG.00L_mouse = 0.5f;
	[HideInInspector]
	
	public float controller_look_sensitivity_x = 1.5f;
	private float controller_look_sensitivity_y = 0.25f;
	private static float default_up_value = 1.0f;
	public bool InputDetected = false;	
	private Camera UICamera; 
	
	protected void Awake()
	{
		UICamera = GameObject.Find("UICamera").GetComponent<Camera>();
	}
	
	private void Start()
	{
		gamepad = Gamepad.current;
		keyboard = Keyboard.current;
	}

	public Vector3 MovementInput3Vector {get {return InputService.ConvertV2toV3(MovementInputVector,default_up_value);} }
	public Vector3 MovementInput3BinaryVector  {get {return InputService.GetBinaryVector3(InputService.ConvertV2toV3(MovementInputVector,default_up_value));} }
	public Vector2 MovementInputBinaryVector  {get {return InputService.GetBinaryVector2(MovementInputVector);} }
	
	private static float InputEventThrottleMax = 1.0f;
	private float InputEventThrottle = 0.0f;
	
	public void ResetInputThrottle()
	{
		InputEventThrottle = 0;
	}
	
	public bool CheckInputThrottle()
	{
		if(InputEventThrottle < InputEventThrottleMax)
		{
			return false;
		}
		else
		{
			return true;
		}
	}
	private void UpdateLookMovementDetected()
	{
		if( LookInputVector != Vector2.zero)
		{
			//Debug.Log("Look!");
		}
	}
	private void UpdateInputDetected()
	{
		InputDetected = false;
		
		if(WasAnyButtonReleased() || WasAnyButtonPressed() || DPadVector!= Vector2.zero || MovementInputVector!= Vector2.zero|| RawLookInputVector!= Vector2.zero)
		{
			InputDetected = true;
		}
		if( keyboard.anyKey.wasPressedThisFrame 
			|| keyboard.anyKey.wasReleasedThisFrame)
		{
			InputDetected = true;
				
		}
	}
	private void Update()
	{
		InputEventThrottle += Time.deltaTime;
		UpdateButtons();
		UpdateInputDetected();
		MovementInputVector = Vector2.zero;
		RawMovementInputVector = Vector2.zero;
		LookInputVector = Vector2.zero;
		RawLookInputVector = Vector2.zero;
		DPadVector = Vector2.zero;
		UpdateLookInput();
		UpdateMovementInput();
		//if(Subscribers.Contains(null))
		//{
		//	Subscribers=Subscribers.Where(x=>x!=null).ToList();
		//}
		//if(InputDetected )
		//{
			
		//	Subscribers.ForEach(subscriber =>{
		//		Debug.Log(subscriber.name);
		//		subscriber.SendMessage("OnInputEvent",SendMessageOptions.DontRequireReceiver);
		//	});
		//}
	}

	private void FixedUpdate()
	{
		UpdateInputDevices();
		CheckForCamera();
		
	
	}
	private void CheckForCamera()
	{
		if(UICamera == null)
		{
			UICamera = GameObject.Find("UICamera").GetComponent<Camera>();
		}
	}
	private void UpdateInputDevices()
	{
		if (gamepad == null)
		{
			gamepad = Gamepad.current;
		}
		if (keyboard == null)
		{
			keyboard = Keyboard.current;
		}
		if (gamepad == null && keyboard != null)
		{
#if UNITY_EDITOR
			if (InputControllerState != InputControllerState.NO_CONTROLLER_CONNECTED)
			{
				//Debug.Log("CONTROLLER DISCONNECTED");
			}
#endif
			InputControllerState = InputControllerState.NO_CONTROLLER_CONNECTED;
			return;
		}
		if (gamepad != null && keyboard != null)
		{
#if UNITY_EDITOR
			if (InputControllerState != InputControllerState.BOTH_KEYBOARD_AND_CONTROLLER_CONNECTED)
			{
				//Debug.Log("CONTROLLER CONNECTED");
			}
#endif
			InputControllerState = InputControllerState.BOTH_KEYBOARD_AND_CONTROLLER_CONNECTED;
			return;
		}
		if (gamepad == null && keyboard == null)
		{
#if UNITY_EDITOR
			if (InputControllerState != InputControllerState.NO_INPUT_DEVICE_CONNECTED)
			{
				//Debug.Log("CONTROLLER DISCONNECTED. KEYBOARD DISCONNECTED");
			}
#endif
			InputControllerState = InputControllerState.NO_INPUT_DEVICE_CONNECTED;
			return;
		}
		if (gamepad != null && keyboard == null)
		{
#if UNITY_EDITOR
			if (InputControllerState != InputControllerState.NO_KEYBOARD_CONNECTED)
			{
				//Debug.Log("KEYBOARD DISCONNECTED");
			}
#endif
			InputControllerState = InputControllerState.NO_KEYBOARD_CONNECTED;
			return;
		}
	}

	//public bool IsAnyButtonDown()
	//{
	//	if()
	//	return	gamepad.aButton.isPressed;
	//}
	public bool WasAnyButtonPressed()
	{
		return	WasWestButtonPressed || 
			WasNorthButtonPressed  || 
			WasSouthButtonPressed  || 
			WasPausePressed  ||
			WasEastButtonPressed ||
			WasRightBumperPressed||
			WasLeftBumperPressed;
	}
	public bool WasAnyButtonReleased()
	{
		return WasWestButtonReleased || 
			WasNorthButtonReleased ||
			WasSouthButtonReleased || 
			WasPauseReleased || 
			WasEastButtonReleased || 
			WasRightBumperReleased ||  
			WasLeftBumperReleased;
	}
	
	public bool IsAnyButtonDown()
	{
		return IsWestButtonDown || 
			IsNorthButtonDown ||
			IsEastButtonDown || 
			IsSouthButtonDown ;
	}
	private void UpdateButtons()
	{
		WasSouthButtonReleased = false;
		WasSouthButtonPressed = false;
		WasPauseReleased= false;
		WasPausePressed= false;
		WasWestButtonPressed= false;
		WasWestButtonReleased= false;
		WasEastButtonPressed= false;
		WasEastButtonReleased= false;
		WasNorthButtonPressed= false;
		WasNorthButtonReleased= false;
		//IsTargetLockPressed =false;
		WasRightBumperPressed =false;
		WasRightBumperReleased = false;
		//IsFreeLookPressed =false;
		WasLeftBumperReleased = false;
		WasLeftBumperPressed =false;
		//IsRightTriggerPressed = false;
		WasRightTriggerPressed =false;
		WasRightTriggerReleased = false;
		//IsLeftTriggerPressed = false;
		WasLeftTriggerReleased =false;
		WasLeftTriggerPressed =false;
		IsEastButtonDown = false;
		IsWestButtonDown = false;
		IsNorthButtonDown = false;
		IsSouthButtonDown = false;
			
		//TODO - check DataRepo for custom inputs
		if (gamepad != null)
		{
			WasSouthButtonReleased = gamepad.buttonSouth.wasReleasedThisFrame;
			WasSouthButtonPressed = gamepad.buttonSouth.wasPressedThisFrame;
			IsSouthButtonDown = gamepad.buttonSouth.isPressed;
			
			WasPauseReleased = gamepad.startButton.wasReleasedThisFrame;
			WasPausePressed = gamepad.startButton.wasPressedThisFrame;

			WasWestButtonPressed = gamepad.buttonWest.wasPressedThisFrame;
			WasWestButtonReleased = gamepad.buttonWest.wasReleasedThisFrame;
			IsWestButtonDown = gamepad.buttonWest.isPressed;
			
			WasEastButtonPressed = gamepad.buttonEast.wasPressedThisFrame;
			WasEastButtonReleased = gamepad.buttonEast.wasReleasedThisFrame;
			IsEastButtonDown = gamepad.buttonEast.isPressed;
			
			WasNorthButtonPressed = gamepad.buttonNorth.wasPressedThisFrame;
			WasNorthButtonReleased = gamepad.buttonNorth.wasReleasedThisFrame;
			IsNorthButtonDown = gamepad.buttonNorth.isPressed;
			
			WasRightBumperReleased = gamepad.rightShoulder.value==0 && IsRightBumperPressed;
			WasRightBumperPressed = gamepad.rightShoulder.wasPressedThisFrame;
			IsRightBumperPressed = gamepad.rightShoulder.value>0;
			
			WasLeftBumperReleased = gamepad.leftShoulder.value==0 && IsLeftBumperPressed;
			WasLeftBumperPressed =gamepad.leftShoulder.wasPressedThisFrame;//
			IsLeftBumperPressed = gamepad.leftShoulder.value>0;

	
			WasRightTriggerReleased = gamepad.rightTrigger.wasReleasedThisFrame;
			WasRightTriggerPressed = gamepad.rightTrigger.wasPressedThisFrame;
			IsRightTriggerPressed = gamepad.rightTrigger.value>0;
			
			WasLeftTriggerReleased = gamepad.leftTrigger.wasReleasedThisFrame;
			WasLeftTriggerPressed =gamepad.leftTrigger.wasPressedThisFrame;
			IsLeftTriggerPressed = gamepad.leftTrigger.value>0;


			
		}
		if (keyboard != null)
		{
			WasPauseReleased = WasPauseReleased || keyboard.escapeKey.wasReleasedThisFrame;
			WasPausePressed = WasPausePressed || keyboard.escapeKey.wasPressedThisFrame;

			WasSouthButtonReleased = WasSouthButtonReleased || keyboard.spaceKey.wasReleasedThisFrame;
			WasSouthButtonPressed = WasSouthButtonPressed || keyboard.spaceKey.wasPressedThisFrame;
			IsSouthButtonDown = IsSouthButtonDown || keyboard.spaceKey.isPressed;

			WasWestButtonPressed = WasWestButtonPressed|| keyboard.fKey.wasPressedThisFrame;
			WasWestButtonReleased = WasWestButtonReleased|| keyboard.fKey.wasReleasedThisFrame;
			IsWestButtonDown = IsWestButtonDown || keyboard.fKey.isPressed;
			
			WasEastButtonPressed = WasEastButtonPressed||keyboard.shiftKey.wasPressedThisFrame;
			WasEastButtonReleased = WasEastButtonReleased|| keyboard.shiftKey.wasReleasedThisFrame;
			IsEastButtonDown = IsEastButtonDown || keyboard.shiftKey.isPressed;
        
			WasNorthButtonPressed = WasNorthButtonPressed || keyboard.eKey.wasPressedThisFrame;
			WasNorthButtonReleased = WasNorthButtonReleased|| keyboard.eKey.wasReleasedThisFrame;
			IsNorthButtonDown = IsNorthButtonDown || keyboard.eKey.isPressed;
			
			WasRightBumperReleased = WasRightBumperReleased || keyboard.rKey.wasReleasedThisFrame ||  Mouse.current.middleButton.wasReleasedThisFrame;
			WasRightBumperPressed = WasRightBumperPressed || keyboard.rKey.wasPressedThisFrame ||  Mouse.current.middleButton.wasPressedThisFrame;
		
			WasLeftBumperReleased = WasLeftBumperReleased || keyboard.leftCtrlKey.wasReleasedThisFrame ||  Mouse.current.forwardButton.wasReleasedThisFrame;
			WasLeftBumperPressed =  WasLeftBumperPressed || keyboard.leftCtrlKey.wasPressedThisFrame ||  Mouse.current.forwardButton.wasPressedThisFrame;
			
			WasRightTriggerReleased = WasRightTriggerReleased || keyboard.cKey.wasReleasedThisFrame|| Mouse.current.leftButton.wasReleasedThisFrame;
			WasRightTriggerPressed = WasRightTriggerPressed || keyboard.cKey.wasPressedThisFrame|| Mouse.current.leftButton.wasPressedThisFrame;
			IsRightTriggerPressed = IsRightTriggerPressed ||  keyboard.cKey.isPressed|| Mouse.current.leftButton.isPressed;
				
			WasLeftTriggerReleased = WasLeftTriggerReleased || keyboard.qKey.wasReleasedThisFrame || Mouse.current.rightButton.wasReleasedThisFrame;
			WasLeftTriggerPressed =  WasLeftTriggerPressed || keyboard.qKey.wasPressedThisFrame|| Mouse.current.rightButton.wasPressedThisFrame;
			IsLeftTriggerPressed = IsLeftTriggerPressed ||  keyboard.qKey.isPressed|| Mouse.current.rightButton.isPressed;
		}
		
	}

	private void UpdateMovementInput()
	{
		
		RawMovementInputVector = (gamepad != null ? gamepad.leftStick.ReadValue() : Vector2.zero) + getVectorFromKeyboard();
		MovementInputVector = RawMovementInputVector.normalized;
		DPadVector = (gamepad != null ? gamepad.dpad.ReadValue() : Vector2.zero) ;
		
	}

	private void UpdateLookInput()
	{
		MousePositionOnScreen =UICamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
		var MouseVector = Mouse.current.delta.ReadValue();
		MouseVector = new Vector2(MouseVector.x * MouseSensitivity_x, MouseVector.y * MouseSensitivity_y);
#if UNITY_WEBGL && !UNITY_EDITOR
			MouseVector.x = GameUtility.WebGLDampenMovement(MouseVector.x);
			MouseVector.y = GameUtility.WebGLDampenMovement(MouseVector.y);
		
#endif
		RawLookInputVector = (gamepad != null ? gamepad.rightStick.ReadValue() : Vector2.zero) + MouseVector;
		var vectorRS = (gamepad != null ? gamepad.rightStick.ReadValue() : Vector2.zero);
		vectorRS = new Vector2(vectorRS.x*controller_look_sensitivity_x,vectorRS.y*controller_look_sensitivity_y)*Time.deltaTime;
		LookInputVector =  vectorRS + MouseVector;
	}
  
	private Vector2 getVectorFromKeyboard()
	{
		if (keyboard == null) return Vector2.zero;
		//TODO - check DataRepo for custom inputs
		var forwardKey = keyboard.wKey.IsPressed();
		var leftKey = keyboard.aKey.IsPressed();
		var rightKey = keyboard.dKey.IsPressed();
		var downKey = keyboard.sKey.IsPressed();
		return InputService.Vector2FromBinaryInput(forwardKey, leftKey, rightKey, downKey);
	}
}
