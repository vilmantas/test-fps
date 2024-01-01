using System;
using System.Diagnostics;
using Godot;
using testfps.Features.Combat;
using testfps.Scripts;
using Vector2 = Godot.Vector2;
using Vector3 = Godot.Vector3;

public partial class PlayerController : CharacterBody3D
{
	[Export] public PackedScene AttackHitbox;
	[Export] public Node3D AttackHitboxSpawn;
	[Export] public float AttackHitboxDelay = 0.3f;
	[Export] public float AttackHitboxDuration = 0.3f;
	[Export] public float AttackCooldown;
	
	[Export] public Mesh PlayerModel;
	[Export] public int Speed = 10;
	[Export] public int LookSpeed = 10;

	[Export] public Vector3 sync_Velocity;
	[Export] public bool sync_Aiming;
	[Export] public Vector2 sync_MovementInput;

	[Export] private float JumpHeight = 200f;
	[Export] private float JumpTimeToPeak = 2f;
	[Export] private float JumpTimeToDescend = 2f;
    
	private bool JumpQueued;

	public CharacterController Character;
	public RayCast3D GroundDistanceRay;
	public CollisionShape3D Hitbox;

	public Label3D DebugLabel;
	
	public HealthController HealthModule;
	private AttackController AttackModule;

	private ThirdPersonController MovementController;

	public override void _Ready()
	{
		DebugLabel = GetNode<Label3D>("debug");
		GroundDistanceRay = GetNode<RayCast3D>("ground_distance_ray");
		Hitbox = GetNode<CollisionShape3D>("core_hitbox");
		Character = GetNode<CharacterController>("character");
		AttackHitboxSpawn = GetNode<Node3D>("spawn_attack_hitbox");
		HealthModule = GetNode<HealthController>("health_module");
		AttackModule = GetNode<AttackController>("attack_module");

		var attackConfiguration = new AttackConfiguration()
		{
			Hitbox = AttackHitbox,
			HitboxSpawn = AttackHitboxSpawn,
			Cooldown = AttackCooldown,
			HitboxDelay = AttackHitboxDelay,
			HitboxDuration = AttackHitboxDuration,
		};
        
		AttackModule.Initialize(attackConfiguration);

		AttackModule.OnHit += HandleHit;
		
		Character.SetModel(PlayerModel);
		
		MovementController = new ThirdPersonController(Speed, LookSpeed, JumpHeight, JumpTimeToPeak, JumpTimeToDescend);
		
		if (!IsMultiplayerAuthority()) return;
		
		PlayerInputManager.Instance.OnJumpPressed += OnJumpPressed;

		PlayerInputManager.Instance.OnRotationEnabled += OnRotationEnabled;

		PlayerInputManager.Instance.OnAttackPressed += OnAttackPressed;
	}

	public override void _PhysicsProcess(double delta)
	{
		DebugLabel.Text = $"{HealthModule.CurrentHealth}/{HealthModule.MaxHealth}";

		DebugLabel.GlobalBasis = GameManager.CameraController.GlobalTransform.Basis;
		
		if (IsMultiplayerAuthority())
		{
			ProcessInput(delta);
		}
		else
		{
			Character.SetAimingState(sync_Aiming);
			
			Velocity = sync_Velocity;
			
			SetAnimationData();
		}
	}

	private bool m_IsJumping;
	
	public override void _Process(double delta)
	{
		if (m_IsJumping && IsOnFloor())
		{
			m_IsJumping = false;
		}
	}

	private void ProcessInput(double delta)
	{
		var args = new ThirdPersonController.ThirdPersonControllerArgs()
		{
			Delta = delta,
			CurrentRotation = Rotation,
			CurrentVelocity = Velocity,
			CurrentMovementInput = sync_MovementInput,
			JumpQueued = JumpQueued,
			LookSpeed = LookSpeed,
			Speed = Speed,
		};
		
		var result = MovementController.GetVelocity(args);
		
		if (result.JumpEngaged)
		{
			Rpc("TriggerJump");
		}
        
		Velocity = result.Velocity;
		Rotation = result.Rotation;

		sync_Velocity = Velocity;

		sync_MovementInput = result.LerpedMovementInput;
		
		SetAnimationData();
		
		MoveAndSlide();
	}

	private void OnJumpPressed()
	{
		JumpQueued = true;
	}

	private void SetAnimationData()
	{
		Character.SetMovementInput(sync_MovementInput);

		var groundDistance = 5f;

		if (GroundDistanceRay.IsColliding())
		{
			groundDistance = GlobalPosition.DistanceTo(GroundDistanceRay.GetCollisionPoint());
		}
		
		Character.SetFloorDistance(groundDistance, MovementController.GetGravity(Velocity));
        
		SelectAnimation();
	}

	private void SelectAnimation()
	{
		if (Velocity is {X: 0, Z: 0})
		{
			Character.SetPlayerAnimation("Idle");
		}
		else
		{
			Character.SetPlayerAnimation("Running");
		}
	}

	private void OnRotationEnabled(bool flag)
	{
		sync_Aiming = flag;
		Character.SetAimingState(flag);
	}

	private bool AttackAvailable = true;
	
	private void OnAttackPressed()
	{
		if (!AttackModule.Attack()) return;

		Rpc("TriggerAttack");
	}
	
	void HandleHit(Node3D obj)
	{
		CombatManager.Instance.Damage(this, obj);
	}

	[Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true)]
	private void TriggerAttack()
	{
		Character.TriggerAttack();
	}

	[Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true)]
	private void TriggerJump()
	{
		Character.TriggerJump();
		JumpQueued = false;
		m_IsJumping = true;
	}
}
