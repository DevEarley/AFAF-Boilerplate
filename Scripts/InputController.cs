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
	public bool WasJumpReleased;
	[HideInInspector]
	public bool WasJumpPressed;
	[HideInInspector]
	public bool WasStartReleased;
	[HideInInspector]
	public bool WasPausePressed;
	[HideInInspector]
	public bool WasAction1Released;
	[HideInInspector]
	public bool WasAction1Pressed;
	[HideInInspector]
	public bool WasRunReleased;
	
	[HideInInspector]
	public bool WasRunPressed;
	[HideInInspector]
	public bool IsRunDown;
	[HideInInspector]
	public bool IsAction1Down;
	[HideInInspector]
	public bool IsAction2Down;
	[HideInInspector]
	public bool IsJumpDown;
	[HideInInspector]
	public bool WasAction2Released; 
	[HideInInspector]
	public bool WasAction2Pressed;
	
	[HideInInspector]
	public bool IsFreeLookPressed; 
	[HideInInspector]
	public bool WasFreeLookPressed; 
	[HideInInspector]
	public bool WasFreeLookReleased;

	[HideInInspector]
	public bool IsTargetLockPressed; 
	[HideInInspector]
	public bool WasTargetLockPressed; 
	[HideInInspector]
	public bool WasTargetLockReleased;

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
		return	WasAction1Pressed || 
			WasAction2Pressed  || 
			WasJumpPressed  || 
			WasPausePressed  ||
			WasRunPressed ||
			WasTargetLockPressed||
			WasFreeLookPressed;
	}
	public bool WasAnyButtonReleased()
	{
		return WasAction1Released || 
			WasAction2Released ||
			WasJumpReleased || 
			WasStartReleased || 
			WasRunReleased || 
			WasTargetLockReleased ||  
			WasFreeLookReleased;
	}
	
	public bool IsAnyButtonDown()
	{
		return IsAction1Down || 
			IsAction2Down ||
			IsRunDown || 
			IsJumpDown ;
	}
	private void UpdateButtons()
	{
		WasJumpReleased = false;
		WasJumpPressed = false;
		WasStartReleased= false;
		WasPausePressed= false;
		WasAction1Pressed= false;
		WasAction1Released= false;
		WasRunPressed= false;
		WasRunReleased= false;
		WasAction2Pressed= false;
		WasAction2Released= false;
		//IsTargetLockPressed =false;
		WasTargetLockPressed =false;
		WasTargetLockReleased = false;
		//IsFreeLookPressed =false;
		WasFreeLookReleased = false;
		WasFreeLookPressed =false;
		IsRunDown = false;
		IsAction1Down = false;
		IsAction2Down = false;
		IsJumpDown = false;
			
		//TODO - check DataRepo for custom inputs
		if (gamepad != null)
		{
			WasJumpReleased = gamepad.buttonSouth.wasReleasedThisFrame;
			WasJumpPressed = gamepad.buttonSouth.wasPressedThisFrame;
			IsJumpDown = gamepad.buttonSouth.isPressed;
			
			WasStartReleased = gamepad.startButton.wasReleasedThisFrame;
			WasPausePressed = gamepad.startButton.wasPressedThisFrame;

			WasAction1Pressed = gamepad.buttonWest.wasPressedThisFrame;
			WasAction1Released = gamepad.buttonWest.wasReleasedThisFrame;
			IsAction1Down = gamepad.buttonWest.isPressed;
			
			WasRunPressed = gamepad.buttonEast.wasPressedThisFrame;
			WasRunReleased = gamepad.buttonEast.wasReleasedThisFrame;
			IsRunDown = gamepad.buttonEast.isPressed;
			
			WasTargetLockReleased = gamepad.rightShoulder.value==0 && IsTargetLockPressed;
			WasTargetLockPressed = gamepad.rightShoulder.wasPressedThisFrame;//|| (IsTargetLockPressed == false && gamepad.rightShoulder.value>0 );
			IsTargetLockPressed = gamepad.rightShoulder.value>0;
			
			WasFreeLookReleased = gamepad.leftShoulder.value==0 && IsFreeLookPressed;
			WasFreeLookPressed =gamepad.leftShoulder.wasPressedThisFrame;//|| (IsFreeLookPressed == false && gamepad.leftShoulder.value>0);
			IsFreeLookPressed = gamepad.leftShoulder.value>0;

			WasAction2Pressed = gamepad.buttonNorth.wasPressedThisFrame;
			WasAction2Released = gamepad.buttonNorth.wasReleasedThisFrame;
			IsAction2Down = gamepad.buttonNorth.isPressed;
			
		}
		if (keyboard != null)
		{
			WasJumpReleased = WasJumpReleased || keyboard.spaceKey.wasReleasedThisFrame;
			WasJumpPressed = WasJumpPressed || keyboard.spaceKey.wasPressedThisFrame;
			IsJumpDown = IsJumpDown || keyboard.spaceKey.isPressed;

			WasStartReleased = WasStartReleased || keyboard.escapeKey.wasReleasedThisFrame;
			WasPausePressed = WasPausePressed || keyboard.escapeKey.wasPressedThisFrame;

			WasAction1Pressed = WasAction1Pressed|| keyboard.eKey.wasPressedThisFrame || Mouse.current.leftButton.wasPressedThisFrame;
			WasAction1Released = WasAction1Released|| keyboard.eKey.wasReleasedThisFrame|| Mouse.current.leftButton.wasReleasedThisFrame;
			IsAction1Down = IsAction1Down || keyboard.eKey.isPressed;
			
			WasRunPressed = WasRunPressed||keyboard.shiftKey.wasPressedThisFrame;
			WasRunReleased = WasRunReleased|| keyboard.shiftKey.wasReleasedThisFrame;
			IsRunDown = IsRunDown || keyboard.shiftKey.isPressed;
        
			WasAction2Pressed = WasAction2Pressed || keyboard.fKey.wasPressedThisFrame|| Mouse.current.rightButton.wasPressedThisFrame;
			WasAction2Released = WasAction2Released|| keyboard.fKey.wasReleasedThisFrame|| Mouse.current.rightButton.wasReleasedThisFrame;
			IsAction2Down = IsAction2Down || keyboard.fKey.isPressed;
			
			
			WasTargetLockReleased = WasTargetLockReleased || keyboard.rKey.wasReleasedThisFrame ||  Mouse.current.middleButton.wasReleasedThisFrame;
			WasTargetLockPressed = WasTargetLockPressed || keyboard.rKey.wasPressedThisFrame ||  Mouse.current.middleButton.wasPressedThisFrame;
			//IsTargetLockPressed = IsTargetLockPressed;
				
			WasFreeLookReleased = WasFreeLookReleased || keyboard.leftCtrlKey.wasReleasedThisFrame ||  Mouse.current.forwardButton.wasReleasedThisFrame;
			WasFreeLookPressed =  WasFreeLookPressed || keyboard.leftCtrlKey.wasPressedThisFrame ||  Mouse.current.forwardButton.wasPressedThisFrame;
			//	IsFreeLookPressed = IsFreeLookPressed || Mouse.current.forwardButton.value>0;
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
