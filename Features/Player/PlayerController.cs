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
	[Export] private Mesh PlayerModel;
	
	public int horizontalModifier = 0;
	public int verticalModifier = 0;
    
	public int speed = 10;
	
	public int lookSpeed = 10;
    
	public Vector3 velocity = Vector3.Zero;

	public CharacterController Character;

	public AnimationPlayer AnimationPlayer;

	private bool JumpQueued = false;

	private float JumpPower = 10f;
	
	private float CurrentJumpPower = 0f;

	[Export] private float JumpHeight = 200f;
	[Export] private float JumpTimeToPeak = 2f;
	[Export] private float JumpTimeToDescend = 2f;
	
	private float JumpVelocity = 0f;
	private float JumpGravity = 0f;
	private float FallGravity = 0f;
	
	public override void _Ready()
	{
		JumpVelocity = 2 * JumpHeight / JumpTimeToPeak;
		JumpGravity = (-2 * JumpHeight) / Mathf.Pow(JumpTimeToPeak, 2);
		FallGravity = (-2 * JumpHeight) / Mathf.Pow(JumpTimeToDescend, 2);
        
		Character = GetNode<CharacterController>("character");
		
		Character.SetModel(PlayerModel);
		
		PlayerInputManager.Instance.OnJumpPressed += OnJumpPressed;
		
		
	}

	public override void _PhysicsProcess(double delta)
	{
		var cameraBasis = GameManager.CameraController.GlobalTransform.Basis;

		velocity = Vector3.Zero;
		
		if (PlayerInputManager.Instance.MovementInputEngaged)
		{
			horizontalModifier = PlayerInputManager.Instance.MovementInput.Horizontal;
		
			verticalModifier = PlayerInputManager.Instance.MovementInput.Vertical;
		
			velocity = cameraBasis.Z * verticalModifier;
			velocity += cameraBasis.X * horizontalModifier;
			
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

			var newRot = Character.Rotation;
		
			newRot.Y = (float)Mathf.LerpAngle(newRot.Y, angle - Math.PI, delta * lookSpeed);
		
			Character.Rotation = newRot;
			
			Character.PlayAnimation("Running");
		}
		else
		{
			Character.PlayAnimation("Idle");
		}

		velocity.Y = Velocity.Y;
		
		velocity.Y += GetGravity() * (float)delta;

		if (JumpQueued)
		{
			JumpQueued = false;
			velocity.Y = JumpVelocity;
		}
        
		velocity.X *= speed;
		velocity.Z *= speed;
		
		Velocity = velocity;
        
		MoveAndSlide();
	}

	private float GetGravity()
	{
		return Velocity.Y > 0 ? JumpGravity : FallGravity;
	}
    
	private void OnJumpPressed()
	{
		JumpQueued = true;
	}
}
