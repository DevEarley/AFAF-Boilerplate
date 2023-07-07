using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SamplePlayerController : MonoBehaviour
{
	public PlayerStates PlayerState;
	private bool CanStillJump;
	private float CanStillJump_delay = 0.15f;
	private float LeftGround_ForCanStillJump_time = 0.0f;
	
	private bool ObservedGoat;
	private bool ObservedSnail;
	private bool ObservedBear;
	private bool ObservedRabbit;
	private bool ObservedFish;
	private bool HasFish;
	private bool DroppedFish;
	private GameObject PlayerAnimatorContainer;
	[HideInInspector]
	public Animator PlayerAnimatorAnimator;
	private ServiceLocator ServiceLocator;
	public Rigidbody PlayerBall;
	private GameObject PlayerCamera;
	private GameObject SkyboxCamera;
	private static float TopSpeed_Horizontal = 10.0f;
	private static float TopSpeed_VerticalUp = 10.0f;
	
	private static float Acceleration_Horizontal = 10.0f;
	private static float ImpluseForce_Jump = 13.0f;
	private static float ImpluseForce_SmallJump = 8.0f;
	private static float ImpluseForce_WaterJump =0.1f;
	private static float Acceleration_GravityDuringJump = 8.0f;
	private static float Acceleration_GravityInWater = 40.0f;
	private static float Acceleration_GravityOnWater = 4.0f;
	private static float Acceleration_GravityAfterJump = 120.0f;
	private static float Acceleration_GravityWhileGrounded = 10.0f;
	private float JumpLockTime = 0.2f;
	private float JumpFallTime = 0.30f;
	private float LastJump = 0.0f;
	
	private static float Jump_Control_Release_Delay = 0.4f;
	public bool IsGrounded = false;
	private Vector3 OriginalPosition = Vector3.zero;

	[HideInInspector]
	private float trackingAngleX;
	[HideInInspector]
	private float trackingAngleY = -0;
	[HideInInspector]
	private bool wallJumpAvailable = true;

	private LayerMask OnlyStaticGeometryLayerMask;
	private LayerMask StaticGeometryLayerMask;

	private LayerMask ClimbableLayerMask;
	
	public bool InScenarioTrigger = false;

	public Vector3 lookVector;

	private GameObject CameraFollow;
	//private bool CameraFollowMode = false;
	//private float CameraFollowModeMultiplyer = 1.5f;


	[HideInInspector]
	private float automove_speed_multiplyer = 2.0f;
	[HideInInspector]
	private float ClimbOverLedge_CameraForwardForce = 6f;
	[HideInInspector]
	private float ClimbOverLedge_UpwardForce = 16.0f;
	[HideInInspector]
	private bool ClimbOverLedgeAvailable = true;
	[HideInInspector]
	private float ClimbOverLedgeCooldown = 0.1f;
	[HideInInspector]
	private float ClimbOverLedgePause = 0.25f;
	[HideInInspector]
	private float in_honey_acceleration_multiplyer = 15.0f;
	[HideInInspector]
	private float in_lava_acceleration_multiplyer = 15.0f;
	[HideInInspector]
	private float in_water_acceleration_multiplyer =12.0f;
	[HideInInspector]
	private  Vector3 LedgeOffset = new Vector3(0.0f, -1.8f, 0.2f);
	[HideInInspector]
	private float max_speed = 18.0f;
	[HideInInspector]
	private float normal_run_acceleration_multiplyer = 150.0f;
	
	private float after_jump_gravity = 70.0f;
	[HideInInspector]
	private float air_control_speed = 20.0f;
	[HideInInspector]
	private bool singleStickCamera_active = false;
	[HideInInspector]
	private bool StartSingleStickMoving_TriggerInitiated = false;

	[HideInInspector]
	private  Vector3 AutoMoveDirection = Vector3.forward;
	[HideInInspector]
	private float camera_distance = 5.6f;
	[HideInInspector]
	private float camera_height = 10.00f;
	[HideInInspector]
	private float cameraHeightCompensator = 1.0f;
	[HideInInspector]
	private  Vector2 cameraHeightCompensatorVector = new Vector2(270.0f, 65.54f);
	[HideInInspector]
	private  Vector3 cameraOffset = new Vector3(0.0f, -1.18f, 0.0f);
	[HideInInspector]
	private float CancleLedge_Delay = 0.5f;
	[HideInInspector]
	private float check_sphere_distance = 0.5f;
	[HideInInspector]
	private float check_sphere_radius = 0.25f;
	[HideInInspector]
	private float CheckForLedge_Delay = 0.05f;
	[HideInInspector]
	private float CheckForLedge_Value = 1.25f;
	[HideInInspector]
	private float CheckForLedge_FeetDistance = 0.75f;
	[HideInInspector]
	private float CheckForLedge_FeetRaycastLength = 1.75f;
	[HideInInspector]
	private float CheckForLedge_HeadDistance = 1.75f;
	[HideInInspector]
	private float CheckForLedge_HeadRaycastLength = 1.75f;
	[HideInInspector]
	private float CheckForLedge_InFrontOfPlayerDistance = 0.8f;
	[HideInInspector]
	private float CheckForLedge_InFrontOfPlayerRaycastLength = 1.75f;
	private float grounded_but_no_user_input_friction = 0.2f;
	private float grounded_friction = 0.999f;	
	private float water_friction = 0.95f;
	[HideInInspector]
	private float jog_min_velocity = 4.5f;
	[HideInInspector]
	private float jump_FallDelay = 0.24f;
	[HideInInspector]
	private float jump_force = 5.0f;
	[HideInInspector]
	private  Vector3 LastVelocity = Vector3.zero;
	[HideInInspector]
	private float super_light_gravity = 9.81f;
	private float light_gravity = 25.0f;
	[HideInInspector]
	private float LookMultiplyer = 120.0f;
	[HideInInspector]
	private float MovePlayerForRespawn_CameraDistance = 3.0f;
	[HideInInspector]
	private float MovePlayerForRespawn_CameraHeight = 1.5f;
	[HideInInspector]
	private float MovePlayerForRespawn_CameraOffset = 5.0f;
	[HideInInspector]
	private float normal_gravity = 35.0f;
	[HideInInspector]
	private float StartMoonBeam_delay = 0.2f;
	[HideInInspector]
	private Vector2 TrackingAngleClamp = new Vector2(-9.0f, 23.5f);
	[HideInInspector]
	private float walking_speed_acceleration_multiplyer = 35.0f;
	[HideInInspector]
	private float web_gl_sensitivity = 4.0f;
	private float IsGrounded_Distance = 0.51f;
	private float IsOnSlant_Distance = 0.75f;
	public bool IsOnSlant =false;
	
	private static float WaterFloatSpeed = 180.0f;
	public float TargetSpeed = 0.0f;
	
	// Awake is called when the script instance is being loaded.
	protected void Awake()
	{
		PlayerCamera = GameObject.Find("PlayerCamera");
		PlayerAnimatorAnimator = GameObject.Find("PlayerAnimatorAnimator").GetComponent<Animator>();
		PlayerAnimatorContainer = GameObject.Find("PlayerAnimatorContainer");
		PlayerBall = GameObject.Find("PlayerBall").GetComponent<Rigidbody>();
		ServiceLocator = GameObject.FindAnyObjectByType<ServiceLocator>();
		StaticGeometryLayerMask = LayerMask.GetMask("StaticGeometry","Climbable");
		OnlyStaticGeometryLayerMask = LayerMask.GetMask("StaticGeometry");
		ClimbableLayerMask = LayerMask.GetMask("Climbable");
		PlayerState = PlayerStates.falling;
		PlayerAnimatorAnimator.Play("falling");
		Physics.gravity = Acceleration_GravityAfterJump * Vector3.down;
		
	}

	private void UpdatePlayerAnimatorPosition()
	{
		PlayerAnimatorContainer.transform.position = PlayerBall.transform.position;
		
	}
	
	public bool LockedCamera = false;
	public bool LockedPlayer = false;
	
	#region LateUpdate
	protected void LateUpdate()
	{
		if(LockedCamera)return;
		UpdateCamera();
	}
	#endregion
	
	#region FixedUpdate
	protected void FixedUpdate()
	{
		if(LockedPlayer)return;
		UpdatePlayerBallRigidbody();
		
	}
	#endregion
	
	#region Update
	protected void Update()
	{
		LockMouseIfNeeded();
		if(LockedPlayer)return;
		//CameraFollowMode = ServiceLocator.InputController.IsTargetLockPressed|| ServiceLocator.InputController.IsFreeLookPressed;
			
			
		if(ServiceLocator.InputController.WasTargetLockPressed || ServiceLocator.InputController.WasFreeLookPressed)
		{
			trackingAngleX = PlayerAnimatorContainer.transform.rotation.eulerAngles.y +90.0f;
		}
		UpdatePlayerAnimatorPosition();
		UpdateLookVector();
		UpdateTrackingAngle();
		if(Time.time < LastJump+JumpLockTime)
		{
			return;
		}
		//else if(PlayerState == PlayerStates.jump || PlayerState == PlayerStates.jumping)
		//{
		//	Physics.gravity = Acceleration_GravityAfterJump*Vector3.down;	
		//}

		if(PlayerState == PlayerStates.hanging)
		{
			CheckForPullUpWhileHaningFromLedge();
			CheckForClimbingCancelJump();
		
			return;
		}
		CheckIsOnSlant();
		CheckIsGrounded();
		if(IsOnSlant == false && IsGrounded == false )
		{
			
			CheckForLedge();
		}
		if(PlayerState == PlayerStates.hanging)
		{
			CheckForPullUpWhileHaningFromLedge();
			CheckForClimbingCancelJump();
		
			return;
		}
		if((IsGrounded == false ) && ServiceLocator.InputController.MovementInputVector.magnitude==0)
		{
			//PlayerAnimatorContainer.transform.forward = InputService.Vector3FromAngle(trackingAngleX);
		}
		else if(ServiceLocator.InputController.MovementInputVector.magnitude>0)
		{
			Vector3 vector = PlayerBall.velocity.normalized;
			PlayerAnimatorContainer.transform.forward = new Vector3(vector.x, 0, vector.z);
		}
		
		if(PlayerState == PlayerStates.jump && (IsGrounded == true ))
		{
			UpdatePlayerGroundedState();
			
			
		}
		if(PlayerState == PlayerStates.jumping&& (IsGrounded == true))
		{
			UpdatePlayerGroundedState();
			
			
		}
		if(CanStillJump || IsGrounded || IsOnSlant || InWaterVolume)
		{
			CheckJump();
		
		}
		if(PlayerState == PlayerStates.jumping || PlayerState == PlayerStates.jump )
		{
			IsOnSlant = false;
			IsGrounded = false;
			return;
		}
		
	
		if (InWaterVolume)
		{
			PlayerBall.velocity = PlayerBall.velocity * water_friction;
		}
		else if((IsGrounded ==true ) && ServiceLocator.InputController.MovementInputVector.magnitude>0 )
		{
			PlayerBall.velocity = PlayerBall.velocity * grounded_friction;
		}
		else if((IsGrounded ==true ) && ServiceLocator.InputController.MovementInputVector.magnitude==0 )
		{
			PlayerBall.velocity = PlayerBall.velocity * grounded_but_no_user_input_friction;
		}
		if((IsGrounded ==true ) )
		{
			//Physics.gravity = Acceleration_GravityWhileGrounded*Vector3.down;
			
			UpdatePlayerGroundedState();
		}
		
		UpdatePlayerAnimation();
		
		
		
	}
		
	private void UpdatePlayerGroundedState()
	{
		var oldState = PlayerState;
		if(InWaterVolume){return;}
		if(ServiceLocator.InputController.MovementInputVector.magnitude>0.0f && IsOnSlant == false)
			{
				PlayerState = PlayerStates.running;			
				if(ServiceLocator.InputController.MovementInputVector.magnitude>0.5f )
				{
					PlayerAnimatorAnimator.Play("running");
					
				}
				else
				{
					PlayerAnimatorAnimator.Play("walking");
					
				}
			}
			else
			{
				PlayerState = PlayerStates.idle;
				PlayerAnimatorAnimator.Play("idle");
				
			}
		
	
	}
	private void UpdatePlayerAnimation()
	{
		switch (PlayerState)
		{
		
		case PlayerStates.running:
			
		
			break;
		}
	}
	
	private void LockMouseIfNeeded()
	{
		if (
			ServiceLocator.InputController.WasStartReleased
			&& Cursor.lockState != CursorLockMode.None
		)
		{
			Cursor.lockState = CursorLockMode.None;
		}
		else
		{
			Cursor.lockState = CursorLockMode.Locked;
		}
	}

	#endregion
	#region Jump

	private IEnumerator Jump()
	{
		LastJump = Time.time;
	
		if(InWaterVolume)
		{
			//Physics.gravity = Acceleration_GravityDuringJump*Vector3.down;
			PlayerBall.AddForce(ImpluseForce_WaterJump*Vector3.up,ForceMode.Impulse);
			yield return new WaitForSeconds(JumpFallTime);
			//Physics.gravity = Acceleration_GravityAfterJump*Vector3.down;
		}
		else
		{
			PlayerState = PlayerStates.jumping;
			PlayerAnimatorAnimator.Play("jumping");
			Physics.gravity = Acceleration_GravityDuringJump*Vector3.down;
			PlayerBall.AddForce(ImpluseForce_Jump*Vector3.up,ForceMode.Impulse);
			yield return new WaitForSeconds(JumpFallTime);
			Physics.gravity = Acceleration_GravityAfterJump*Vector3.down;
		}
	}
	
	private IEnumerator JumpInPlace()
	{
		LastJump = Time.time;
	
		if(InWaterVolume)
		{
			//Physics.gravity = Acceleration_GravityDuringJump*Vector3.down;
			PlayerBall.AddForce(ImpluseForce_WaterJump*Vector3.up,ForceMode.Impulse);
			yield return new WaitForSeconds(JumpFallTime);
			//Physics.gravity = Acceleration_GravityAfterJump*Vector3.down;
			
		}else
		{
			PlayerState = PlayerStates.jump;
			PlayerAnimatorAnimator.Play("jump");
			Physics.gravity = Acceleration_GravityDuringJump*Vector3.down;
			PlayerBall.AddForce(ImpluseForce_SmallJump*Vector3.up,ForceMode.Impulse);
			yield return new WaitForSeconds(JumpFallTime);
			Physics.gravity = Acceleration_GravityAfterJump*Vector3.down;
		}
	}
	private void CheckJump()
	{
		if(ServiceLocator.InputController.WasJumpPressed && ServiceLocator.InputController.MovementInputVector.magnitude>0.0f)
		{
			StartCoroutine(Jump());
			
		}
		if(ServiceLocator.InputController.WasJumpPressed && ServiceLocator.InputController.MovementInputVector.magnitude==0.0f)
		{
			StartCoroutine(JumpInPlace());
		}
	
		
	}
	
	#endregion
	
	
	
    #region Climb
    

	private Vector3 LedgeDirection;
	private bool InClimbableVolume = false;
	private void CheckForLedge()
	{
		if (ClimbOverLedgeAvailable == false ||  InWaterVolume)
			return;
		
		if( CheckForLedgeInDirection(PlayerAnimatorContainer.transform.forward))
		{
	    	
			//	Debug.Log("Ledge In Front");
			
			return;
		}
		var PlayerVectorDirection = InputService.Vector3FromAngle(trackingAngleX);
		if( CheckForLedgeInDirection(PlayerVectorDirection))
		{
	    	
			//	Debug.Log("Ledge In Front");
			
			return;
		}
		
		else
		{
			return;
		}
      
	}
	
	
	private bool CheckForLedgeInDirection(Vector3 PlayerVectorDirection)
	{
		var HeadOrigin =
			PlayerBall.transform.position + Vector3.up * CheckForLedge_HeadDistance;
		var FeetOrigin =
			PlayerBall.transform.position + Vector3.up * CheckForLedge_FeetDistance;
		var SlightlyInFrontOrigin =
			HeadOrigin + PlayerVectorDirection * CheckForLedge_HeadRaycastLength;
		RaycastHit wallOppositeTheFeet_info;
		RaycastHit wallOppositeTheHead_info;
		RaycastHit floorSlightlyInfrontOfPlayer_info;
		LayerMask mask = InClimbableVolume ? OnlyStaticGeometryLayerMask : ClimbableLayerMask;
			
		var wallOppositeTheFeet = Physics.Raycast(
			FeetOrigin,
			PlayerVectorDirection,
			out wallOppositeTheFeet_info,
			CheckForLedge_FeetRaycastLength,
			mask,
			QueryTriggerInteraction.Ignore
		);
		var wallOppositeTheHead = Physics.Raycast(
			HeadOrigin,
			PlayerVectorDirection,
			out wallOppositeTheHead_info,
			CheckForLedge_HeadRaycastLength,
			mask,
			QueryTriggerInteraction.Ignore
		);
		var celingAboveHead = Physics.Raycast(
			HeadOrigin,
			PlayerVectorDirection,
			CheckForLedge_HeadRaycastLength,
			mask,
			QueryTriggerInteraction.Ignore
		);
		var floorSlightlyInfrontOfPlayer = Physics.Raycast(
			SlightlyInFrontOrigin,
			Vector3.down,
			out floorSlightlyInfrontOfPlayer_info,
			CheckForLedge_InFrontOfPlayerRaycastLength,
			mask,
			QueryTriggerInteraction.Ignore
		);

		if (
			wallOppositeTheFeet == true
			&& floorSlightlyInfrontOfPlayer == true
			&& wallOppositeTheHead == false
			&& celingAboveHead == false
		)
		{
			var y = floorSlightlyInfrontOfPlayer_info.point.y;
			var x = wallOppositeTheFeet_info.point.x;
			var z = wallOppositeTheFeet_info.point.z;
			var forward = Quaternion.Euler(0,180,0)*wallOppositeTheFeet_info.normal;
			var newPlayerVector =
				new Vector3(x, y, z) + wallOppositeTheFeet_info.normal * LedgeOffset.z + Vector3.up * LedgeOffset.y;
			wallJumpAvailable = false;
			PlayerAnimatorAnimator.Play("hanging");
			//	ServiceLocator.SoundController.PlayOneShot(GameSounds.PullUpGrunt, 0);
			PlayerState = PlayerStates.hanging;
			PlayerBall.velocity = Vector3.zero;
			PlayerBall.isKinematic = true;
			PlayerBall.transform.position = (newPlayerVector);
			PlayerAnimatorContainer.transform.position = (newPlayerVector);
			PlayerAnimatorContainer.transform.forward = forward;
			LedgeDirection =forward;
			
			return true;
		}
		return false;
	}
	
	private void CheckForPullUpWhileHaningFromLedge()
	{
		if (ServiceLocator.InputController.WasJumpPressed && ClimbOverLedgeAvailable)
		{
			ClimbOverLedgeAvailable = false;
			StartCoroutine(ClimbOverLedge());
		}
	}

	IEnumerator ClimbOverLedge()
	{
		//Debug.Log("ClimbOverLedge");
		
		
		PlayerAnimatorAnimator.Play("falling");
		PlayerBall.isKinematic = false;
		//add force up and then after a short pause, add force camera-forward
		Physics.gravity = Vector3.down * Acceleration_GravityDuringJump;
		//ServiceLocator.SoundController.PlayOneShot(GameSounds.PullUpGrunt, 0);
		PlayerBall.AddForce(Vector3.up * ClimbOverLedge_UpwardForce, ForceMode.Impulse);
		yield return new WaitForSeconds(ClimbOverLedgePause);
		PlayerBall.AddForce(
			LedgeDirection * ClimbOverLedge_CameraForwardForce,
			ForceMode.Impulse
		);
		Physics.gravity = Vector3.down * Acceleration_GravityAfterJump;

		yield return new WaitForSeconds(ClimbOverLedgeCooldown);
		ClimbOverLedgeAvailable = true;
		PlayerState = PlayerStates.falling;
	}


	public void CheckForClimbingCancelJump()
	{
		if (ServiceLocator.InputController.WasAction2Pressed)
		{
			PlayerBall.isKinematic = false;
			//cancel climbing mode, jump away from surface normal.
		}
	}


    #endregion
	
	#region Movement

	private void UpdatePlayerBallRigidbody()
	{
		if(PlayerBall.isKinematic)return;
		Vector3 horizontal = Vector3.zero;
		Vector3 vertical = Vector3.zero;
		
		Vector3 _lookVector = InputService.Vector3FromAngle(trackingAngleX);
		float perpendicularAngleX = trackingAngleX + 90;
		Vector3 perpendicularLookingVector = InputService.Vector3FromAngle(perpendicularAngleX);
		horizontal =
			ServiceLocator.InputController.MovementInputVector.x * perpendicularLookingVector;
		vertical = ServiceLocator.InputController.MovementInputVector.y * _lookVector;
		Vector3 move = Vector3.Normalize(horizontal + vertical);

		TargetSpeed = ServiceLocator.InputController.MovementInputVector.magnitude;

		if (IsGrounded == false && IsOnSlant == false )
		{
			//TargetSpeed = 0;
			TargetSpeed *= air_control_speed;
		}
		else if (InWaterVolume)
		{
			TargetSpeed *= in_water_acceleration_multiplyer;
			PlayerBall.velocity += Vector3.up*Time.fixedDeltaTime*WaterFloatSpeed;
		    							
		}
		
		else if(PlayerState == PlayerStates.running) 
		{
			TargetSpeed *= normal_run_acceleration_multiplyer;
			
		}
		else
		{
			//TargetSpeed *= air_control_speed;
			
			//TargetSpeed = 0;
			 
		}
		
		
		PlayerBall.velocity +=
			Vector3.Normalize(horizontal + vertical) * TargetSpeed * Time.fixedDeltaTime;
            
	 if(PlayerState == PlayerStates.running && IsGrounded) 
		{
			PlayerBall.velocity = Vector3.ClampMagnitude(PlayerBall.velocity,max_speed);
			
		}
	}
	private void CheckIsGrounded()
	{
		if(InWaterVolume){return;}
		
		var grounded = Physics.Raycast(PlayerBall.transform.position,Vector3.down,IsGrounded_Distance,StaticGeometryLayerMask,QueryTriggerInteraction.Ignore);
		if(grounded == false && (IsGrounded == true || IsOnSlant == true))
		{
			//falling
			StartCoroutine(CanStillJumpTimer());
			PlayerAnimatorAnimator.Play("falling");
			Physics.gravity= Acceleration_GravityAfterJump*Vector3.down;
		}
		else if(grounded == true && (IsGrounded == false))
		{
			PlayerAnimatorAnimator.Play("idle");
			
		}
		IsGrounded = grounded;
	}
	
	private void CheckIsOnSlant()
	{
		if(InWaterVolume)return;
		IsOnSlant = Physics.Raycast(PlayerBall.transform.position,Vector3.down,IsOnSlant_Distance,StaticGeometryLayerMask,QueryTriggerInteraction.Ignore) && IsGrounded == false;
		
	}
	private IEnumerator CanStillJumpTimer()
	{
		CanStillJump = true;
		yield return new WaitForSeconds(CanStillJump_delay);
		CanStillJump = false;
		
	}
	

	private void UpdateTrackingAngle()
	{
		var inputVector = Vector2.zero;
		var altInputVector = Vector2.zero;
		
		//PlayerCamera.transform.LookAt(PlayerBall.transform.position);
		//trackingAngleX = Mathf.Lerp(trackingAngleX, PlayerCamera.transform.rotation.eulerAngles.y, Mathf.Clamp01(Time.deltaTime*UpdateTrackingAngle_LockSpeed));
		
		
		altInputVector = ServiceLocator.InputController.LookInputVector * LookMultiplyer;
		
		
		
			
		//if(ServiceLocator.InputController.IsFreeLookPressed ||ServiceLocator.InputController.IsTargetLockPressed )
	
		//if(CameraFollowMode && Mathf.Abs( ServiceLocator.InputController.RawMovementInputVector.x)>0.85f)
		//	{
		//		inputVector =  ServiceLocator.InputController.MovementInputVector*CameraFollowModeMultiplyer+ altInputVector;
		//	}
		//	else
		//		{
			inputVector = altInputVector;
		//		}
		trackingAngleX += inputVector.x ;
		trackingAngleX = InputService.ClampAngle(trackingAngleX);
		
	

		trackingAngleY += altInputVector.y ;
		trackingAngleY = Mathf.Clamp(trackingAngleY, TrackingAngleClamp.x, TrackingAngleClamp.y);
	}
	#endregion
	#region Look
	private void UpdateLookVector()
	{
		lookVector = InputService.Vector3FromAngle(trackingAngleX);
	}
	private float UpdateTrackingAngle_LockSpeed = 5.0f;
	private bool InWaterVolume;
	private bool CameraIsLocked;
	private void UpdateCamera()
	{
		if( CameraIsLocked)return;
		var camDistance = camera_distance;
		var scaledTrackingAngleY = 2.0f * trackingAngleY;

		RaycastHit hit;
		if (
			Physics.SphereCast(
			PlayerBall.transform.position,
			0.2f,
			lookVector * -1.0f,
			out hit,
			camera_distance,
			StaticGeometryLayerMask,
			QueryTriggerInteraction.Ignore
			)
		)
		{
			camDistance = Vector3.Distance(PlayerBall.transform.position, hit.point);
		}

		var invertedTrackingY = Mathf.Abs(scaledTrackingAngleY - 360.0f);
		cameraHeightCompensator =
		(
			(invertedTrackingY - cameraHeightCompensatorVector.x)
			/ cameraHeightCompensatorVector.y
		) - 0.8f;
		
		PlayerCamera.transform.position =
			PlayerBall.transform.position
			+ (Vector3.up * camera_height * cameraHeightCompensator)
			+ ((lookVector) * -camDistance);
		//var lookAtAngle = GameUtility.LookAt(this.transform.position,PlayerBall.transform.position + cameraOffset);
		//PlayerCamera.transform.Rotate(Vector3.up, lookAtAngle);	
		PlayerCamera.transform.LookAt(PlayerBall.transform.position + cameraOffset);
			PlayerCamera.transform.Rotate(Vector3.left, scaledTrackingAngleY);

		if (SkyboxCamera != null)
		{
			SkyboxCamera.transform.rotation = PlayerCamera.transform.rotation;
		}
	}
	
	#endregion
	#region Volume Logic
	public void OnEnterSwimmingVolume()
	{
		InWaterVolume = true;
		PlayerState = PlayerStates.swimming;
		PlayerAnimatorAnimator.Play("swim");
		Physics.gravity = Vector3.up * Acceleration_GravityInWater;
		
		//PlayerAnimatorAnimator.Play("swim");
	}
	
	public void OnExitSwimmingVolume()
	{
		InWaterVolume = false;
		if(ServiceLocator.InputController.IsJumpDown)
		{
			StartCoroutine(Jump());
		}
		else
		{
			Physics.gravity = Vector3.down * Acceleration_GravityOnWater;
			StartCoroutine(WaterSkip());
		}
	}
	private IEnumerator WaterSkip()
	{
		yield return new WaitForSeconds(0.05f);
		Physics.gravity = Vector3.down * Acceleration_GravityAfterJump;
		
	}
	public void OnEnter_FishingVolume()
	{
		
	}
	
	public void OnExit_FishingVolume()
	{
		
	}
		
	public void OnEnter_CreatureObservationVolume(CreatureType Type)
	{
		//ServiceLocator.UIController.SpriteFontPanel.Render("Press and hold any button to collect badge.");
	}
	
	public void OnExit_CreatureObservationVolume(CreatureType Type)
	{
		ServiceLocator.UIController.SpriteFontPanel.Reset();
		
	}
	//private bool ShowingScoresOnScreen = false;

	
	public void OnEnter_InfoSignVolume()
	{
		
	}
	
	public void OnExit_InfoSignVolume()
	{
		
	}	
	public void OnEnter_ClimbableVolume()
	{
		InClimbableVolume = true;
	}
	
	public void OnExit_ClimbableVolume()
	{
		InClimbableVolume = false;
	}

	public void OnExit_IntroAreaVolume()
	{
		
	}
	#endregion

}
public enum CreatureType
{
	snail,
	bear,
	goat,
	rabbit,fish
}
public enum PlayerStates
{
	idle, // no movement vector
	running, //full movement vector (normalized)
	jump, // no movement vector
	jumping, //yes movement vector
	falling, // jump, jumping and IsGrounded false lead to falling
	fishing, //action1 is down in fishing volume
	show_fish, // after fishing is done
	pickup_item, //??
	observing_creature,//action 1 is down in creature's obeservation volume
	hanging,//hanging on ledge,
	swimming
}