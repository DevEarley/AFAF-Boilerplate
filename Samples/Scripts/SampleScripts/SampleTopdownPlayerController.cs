using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class SampleTopdownPlayerController : MonoBehaviour
{
	private SpriteFontPanel HealthSpriteFontPanel;
	private SpriteFontPanel AmmoSpriteFontPanel;
	private SpriteFontPanel ScoreSpriteFontPanel;
	private SpriteFontPanel GameoverSpriteFontPanel;
	private SampleBulletController BulletController;
	private SampleFXController FXController;
	private Rigidbody PlayerBall; 
	private GameObject PlayerCamera; 
	public GameObject Shotgun; 
	public GameObject BulletSpawn; 
	private ISoundController SoundController; 
	private InputController InputController; 
	private Vector2 MovementVector;
	private Vector3 LookVector;
	private AudioSource GameMusic;
	private Animator PlayerAnimator;
	public SkinnedMeshRenderer PlayerMeshRenderer;
	
	public bool Locked = false;
	private bool Dash_Locked = false;
	private float MaxSpeed = 10.0f;
	private float MoveSpeed = 40.0f;
	private float LookSpeed = 1.0f;
	private float Friction = 0.8f;
	private float Dash_MaxSpeed = 40.0f;
	private float Dash_MoveSpeed = 40.0f;
	private float Dash_Friction = 0.9f;
	private float Dash_Duration = 0.25f;
	private Quaternion targetRotation;
	private bool HasShotgun = false;
	public Material WhiteMaterial;
	public Material NormalMaterial;
	
	private int Health=10;
	private int Ammo=0;
	private int Score=0;
	
    void Start()
	{
		Shotgun.SetActive(false);
		GameMusic = GameObject.Find("GameMusic").GetComponent<AudioSource>();
		SoundController = GameUtility.GetAnyObjectThatImplementsInterface<ISoundController>();
		FXController = FindAnyObjectByType<SampleFXController>();
		BulletController = FindAnyObjectByType<SampleBulletController>();
		
		HealthSpriteFontPanel = GameObject.Find("HealthSpriteFontPanel").GetComponent<SpriteFontPanel>();
		AmmoSpriteFontPanel = GameObject.Find("AmmoSpriteFontPanel").GetComponent<SpriteFontPanel>();
		GameoverSpriteFontPanel = GameObject.Find("GameoverSpriteFontPanel").GetComponent<SpriteFontPanel>();
		ScoreSpriteFontPanel = GameObject.Find("ScoreSpriteFontPanel").GetComponent<SpriteFontPanel>();
		PlayerBall = GameObject.Find("PlayerBall").GetComponent<Rigidbody>();
	    PlayerCamera = GameObject.Find("PlayerCameraContainer");
		InputController = FindAnyObjectByType<InputController>();
		PlayerAnimator = gameObject.GetComponentInChildren<Animator>();
	}
	IEnumerator Gameover()
	{
		LockedMelee = true;
		LockedGun = true;
		Time.timeScale = 0.2f;
		GameMusic.pitch = 0.9f;
		GameoverSpriteFontPanel.Render("Gameover");
		yield return new WaitForSeconds(1.2f);
		SceneManager.LoadScene("PublishedScene");
		

	}
	public void DoDamage()
	{
		StartCoroutine(HurtPlayerFlash());
		Health --;
		HealthSpriteFontPanel.Render("Health: "+Health);
		if(Health<=0)
		{
			
			StartCoroutine(Gameover());
		}
		
	}
	private IEnumerator HurtPlayerFlash()
	{
		if(Locked==false)
		{
			
		Time.timeScale = 0.9f;
		yield return new WaitForSeconds(0.016f);
		Time.timeScale =1.0f;
		}
		
		PlayerMeshRenderer.material = WhiteMaterial;
		yield return new WaitForSeconds(1.0f);
		PlayerMeshRenderer.material = NormalMaterial;
		
	}
	
	void FixedUpdate()
	{
		if(Dash_Locked)
		{
			PlayerBall.velocity+= (Vector3.forward*InputController.MovementInputVector.x*Dash_MoveSpeed)+(Vector3.right*InputController.MovementInputVector.y*-1*Dash_MoveSpeed);
			PlayerBall.velocity*=Dash_Friction;
			PlayerBall.velocity = Vector3.ClampMagnitude(PlayerBall.velocity,Dash_MaxSpeed);
		}
		else
		{
			PlayerBall.velocity+= (Vector3.forward*InputController.MovementInputVector.x*MoveSpeed)+(Vector3.right*InputController.MovementInputVector.y*-1*MoveSpeed);
			PlayerBall.velocity*=Friction;
			PlayerBall.velocity = Vector3.ClampMagnitude(PlayerBall.velocity,MaxSpeed);
		}
	}
	
	public void OnGetAmmoPickup()
	{
		AddToScore(10);
		Ammo++;
		AmmoSpriteFontPanel.Render("Ammo: "+Ammo);
		
		SoundController.Play(GameSounds.AquireAmmo,0);
		Debug.Log("OnGetAmmoPickup");
	}
	
	public void AddToScore(int score)
	{
		Score+=score;
		ScoreSpriteFontPanel.Render("Score: "+Score);
	}
	
	public void OnGetShotgunPickup()
	{
		Ammo++;
		AmmoSpriteFontPanel.Render("Ammo: "+Ammo);
		AddToScore(100);
		Shotgun.SetActive(true);
		HasShotgun = true;
		SoundController.Play(GameSounds.AquireGun,0);
		Debug.Log("OnGetShotgunPickup");
	}
	
	public void OnGetBloodPickup()
	{
		AddToScore(5);
		Health++;
		HealthSpriteFontPanel.Render("Health: "+Health);
		
		SoundController.Play(GameSounds.AquireHealth,2);
		Debug.Log("OnGetBloodPickup");
	}
	
	private void CalculateLook()
	{
		if(Gamepad.current!=null && Gamepad.current.rightStick.magnitude>0)
		{
			var inputX = Gamepad.current.rightStick.x.value;
			var inputY = -Gamepad.current.rightStick.y.value;
			Vector3 lookDirection = new Vector3(inputY, 0, inputX);
			Quaternion lookRotation = Quaternion.LookRotation(lookDirection, Vector3.up);
			targetRotation = lookRotation;
			this.transform.rotation = (targetRotation);
			return;
		}
		
		if(Mouse.current.delta.value.magnitude > 0 )
		{
			var groundPlane = new Plane(Vector3.up, -transform.position.y);
			var mouseRay = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
			float hitDistance;

			if (groundPlane.Raycast(mouseRay, out hitDistance))
			{
				var lookAtPosition = mouseRay.GetPoint(hitDistance);
				targetRotation = Quaternion.LookRotation(lookAtPosition - transform.position, Vector3.up);
			}
		
			this.transform.rotation = (targetRotation);
		}
	}
	private bool LockedMelee;
	private IEnumerator MeleeAttack()
	{
		Shotgun.SetActive(false);
		LockedMelee = true;
		PlayerAnimator.Play("melee");
		yield return new WaitForSeconds(0.5f);
		Shotgun.SetActive(HasShotgun);
		
		LockedMelee = false;
		RaycastHit[] hitInfo = Physics.SphereCastAll(transform.position,0.6f,transform.forward,0.5f);
		RaycastHit? hitEnemy= hitInfo.FirstOrDefault(x=>x.collider.tag =="Enemy");
		if(hitEnemy != null)
		{
			SoundController.Play(GameSounds.MeleeAttack,2);
			
			var unarmed = hitEnemy.Value.collider.gameObject.GetComponent<SampleNPCUnarmedBehaviour>();
			var farmer = hitEnemy.Value.collider.gameObject.GetComponent<SampleNPCFarmerBehaviour>();
			if(unarmed != null)
			{
				unarmed.DoDamage();
			}
			if(farmer != null)
			{
				farmer.DoDamage();	
			}
		}
	}
	private bool LockedGun = false;
	private IEnumerator ShootGun()
	{
		LockedGun = true;
		SoundController.Play(GameSounds.GunFire2,1);
		Ammo --;
		AmmoSpriteFontPanel.Render("Ammo: "+Ammo);
			
		var v1 = Quaternion.AngleAxis(-10, Vector3.up) * transform.forward;
		var v2 = Quaternion.AngleAxis(-7, Vector3.up) * transform.forward;
		var v3 = Quaternion.AngleAxis(-3, Vector3.up) * transform.forward;
		var v4 = Quaternion.AngleAxis(0, Vector3.up) * transform.forward;
		var v5 = Quaternion.AngleAxis(3, Vector3.up) * transform.forward;
		var v6 = Quaternion.AngleAxis(7, Vector3.up) * transform.forward;
		var v7 = Quaternion.AngleAxis(10, Vector3.up) * transform.forward;
			
		BulletController.SpawnBullet2(BulletSpawn.transform.position,v1);
		BulletController.SpawnBullet2(BulletSpawn.transform.position,v2);
		BulletController.SpawnBullet2(BulletSpawn.transform.position,v3);
		BulletController.SpawnBullet2(BulletSpawn.transform.position,v4);
		BulletController.SpawnBullet2(BulletSpawn.transform.position,v5);
		BulletController.SpawnBullet2(BulletSpawn.transform.position,v6);
		BulletController.SpawnBullet2(BulletSpawn.transform.position,v7);
		yield return new WaitForSeconds(0.2f);
		SoundController.Play(GameSounds.BulletShell,1);
		yield return new WaitForSeconds(0.2f);
		SoundController.Play(GameSounds.AquireGun,1);
		yield return new WaitForSeconds(0.2f);
		LockedGun = false;
		
		
	}
	void Update()
	{
		CalculateLook();
		
		if((InputController.WasAction2Pressed || InputController.WasTargetLockPressed)&& LockedMelee == false)
		{
			StartCoroutine(MeleeAttack());
			
		}
		if((InputController.WasAction1Pressed || InputController.WasFreeLookPressed) && HasShotgun)
		{
			if(Ammo == 0)
			{
				SoundController.Play(GameSounds.AquireGun,1);
			}
			else if(LockedGun == false)
			{
				StartCoroutine(ShootGun());
			}
			
		}
		
		if(Dash_Locked == false && InputController.WasRunPressed)
		{
			StartCoroutine(Dash());
		}
		if(LockedMelee)
		{
			
		}
			else if(InputController.MovementInputVector.magnitude == 0 )
		{
			PlayerAnimator.Play("idle");
		}
		else 
		{
			PlayerAnimator.Play("run");
		}
	}
	
	private IEnumerator Dash()
	{
		PlayerAnimator.Play("dash");
		Dash_Locked = true;
		yield return new WaitForSeconds(Dash_Duration);
		Dash_Locked = false;
	}
	
	void LateUpdate()
    {
	    UpdatePlayerMesh();
    }
    
	private void UpdatePlayerCamera()
	{
		PlayerCamera.transform.position = PlayerBall.transform.position;
	}
    
	private void UpdatePlayerMesh()
	{
		transform.position = PlayerBall.transform.position;
	}
}
