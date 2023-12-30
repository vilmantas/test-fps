using Godot;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using testfps.Scripts;
using Vector2 = Godot.Vector2;
using Vector3 = Godot.Vector3;

public partial class PlayerController : CharacterBody3D
{
	[Export] public PackedScene AttackHitbox;
	[Export] public Mesh PlayerModel;
	[Export] public int Speed = 10;
	[Export] public int LookSpeed = 10;

	[Export] public Vector3 sync_Velocity;
	[Export] public bool sync_Aiming;
	[Export] public Vector2 sync_MovementInput;

	[Export] private float JumpHeight = 200f;
	[Export] private float JumpTimeToPeak = 2f;
	[Export] private float JumpTimeToDescend = 2f;
	
	private float JumpVelocity = 0f;
	private float JumpGravity = 0f;
	private float FallGravity = 0f;
    
	private int horizontalModifier = 0;
	private int verticalModifier = 0;
	private Vector3 velocity = Vector3.Zero;
	private bool JumpQueued = false;

	public CharacterController Character;
	public RayCast3D GroundDistanceRay;
	public CollisionShape3D Hitbox;
	public Node3D AttackHitboxSpawn;
	public Label3D DebugLabel;
    
	public override void _Ready()
	{
		DebugLabel = GetNode<Label3D>("debug");
		GroundDistanceRay = GetNode<RayCast3D>("ground_distance_ray");
		Hitbox = GetNode<CollisionShape3D>("core_hitbox");
		Character = GetNode<CharacterController>("character");
		AttackHitboxSpawn = GetNode<Node3D>("spawn_attack_hitbox");

		Character.SetModel(PlayerModel);

		JumpVelocity = 2 * JumpHeight / JumpTimeToPeak;
		JumpGravity = (-2 * JumpHeight) / Mathf.Pow(JumpTimeToPeak, 2);
		FallGravity = (-2 * JumpHeight) / Mathf.Pow(JumpTimeToDescend, 2);
		
		if (!IsMultiplayerAuthority()) return;
		
		PlayerInputManager.Instance.OnJumpPressed += OnJumpPressed;

		PlayerInputManager.Instance.OnRotationEnabled += OnRotationEnabled;
	}

	public override void _PhysicsProcess(double delta)
	{
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

	private void ProcessInput(double delta)
	{
		var cameraBasis = GameManager.CameraController.GlobalTransform.Basis;

		velocity = Vector3.Zero;
		
		horizontalModifier = PlayerInputManager.Instance.MovementInput.Horizontal;
		
		verticalModifier = PlayerInputManager.Instance.MovementInput.Vertical;
		
		sync_MovementInput = new Vector2(horizontalModifier, verticalModifier).Normalized();
        
		if (PlayerInputManager.Instance.MovementInputEngaged)
		{
			velocity = cameraBasis.Z * verticalModifier;
			velocity += cameraBasis.X * horizontalModifier;

			velocity = velocity.Normalized();
			
			float angle;
			
			if (PlayerInputManager.Instance.RotationEnabled)
			{
				var vect = -cameraBasis.Z;
				
				angle = new Vector2(vect.Z, vect.X).Angle();
			}
			else
			{
				angle = new Vector2(velocity.Z, velocity.X).Angle();	
			}

			var newRot = Rotation;
		
			newRot.Y = (float)Mathf.LerpAngle(newRot.Y, angle - Math.PI, delta * LookSpeed);
		
			Rotation = newRot;
		}

		velocity.Y = Velocity.Y;
		
		velocity.Y += GetGravity() * (float)delta;

		if (JumpQueued)
		{
			JumpQueued = false;
			Rpc("TriggerJump");
			velocity.Y = JumpVelocity;
		}
        
		velocity.X *= Speed;
		velocity.Z *= Speed;
		
		Velocity = velocity;

		sync_Velocity = Velocity;
		
		SetAnimationData();
		
		MoveAndSlide();
	}

	public float GetGravity()
	{
		return Velocity.Y > 0 ? JumpGravity : FallGravity;
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
		
		Character.SetFloorDistance(groundDistance, GetGravity());
        
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

	[Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true)]
	private void TriggerJump()
	{
		Character.TriggerJump();
	}
}
