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
	
	public override void _Ready()
	{
		Character = GetNode<CharacterController>("character");
		
		Character.SetModel(PlayerModel);
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
		
		velocity.Y -= 98f * (float)delta;
		
		Velocity = velocity * speed;
		
		MoveAndSlide();
	}
}
