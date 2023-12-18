using Godot;
using System;

public partial class CameraController : Node3D
{
	public bool RotationEnabled = false;
	
	private Vector2 Offset = Vector2.Zero;

	public Node3D Arm;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Arm = GetNode<Node3D>("SpringArm3D");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (GameManager.PlayerController == null) return;

		var player = GameManager.PlayerController;
		
		Position = player.Position + new Vector3(0, 1.5f, 0);
		
		CheckTrigger();
		
		RotateMouse(delta);
		
		Offset = Vector2.Zero;
	}

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseMotion mouseMotion)
		{
			Offset = mouseMotion.Relative;
		}
	}
	
	private void CheckTrigger()
	{
		if (Input.IsActionJustPressed("player_camera_rotate"))
		{
			EnableRotation();
		}

		if (Input.IsActionJustReleased("player_camera_rotate"))
		{
			DisableRotation();
		}
	}
	
	private void RotateMouse(double delta)
	{
		if (!RotationEnabled) return;
        
		var x = -Offset.X;
		var y = -Offset.Y;
		
		Arm.RotateX(Mathf.DegToRad(y * 0.1f));
		
		if (Arm.RotationDegrees.X > 20)
		{
			Arm.RotationDegrees = new Vector3(20, Arm.RotationDegrees.Y, Arm.RotationDegrees.Z);
		}
		else if (Arm.RotationDegrees.X < -60)
		{
			Arm.RotationDegrees = new Vector3(-60, Arm.RotationDegrees.Y, Arm.RotationDegrees.Z);
		}
        
		RotateY(Mathf.DegToRad(x * 0.1f));
	}
	
	private void EnableRotation()
	{
		RotationEnabled = true;
		HideMouse();
	}
	
	private void DisableRotation()
	{
		RotationEnabled = false;
		Offset = Vector2.Zero;
		ShowMouse();
	}
	
	private void HideMouse()
	{
		Input.MouseMode = Input.MouseModeEnum.Captured;
	}
	
	private void ShowMouse()
	{
		Input.MouseMode = Input.MouseModeEnum.Visible;
	}
}
