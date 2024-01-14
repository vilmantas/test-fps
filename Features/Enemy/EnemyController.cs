using Godot;
using System;
using System.Linq;

public partial class EnemyController : CharacterBody3D
{
	private Node3D Target;

	public override void _Process(double delta)
	{
		var players = GetTree().GetNodesInGroup("player");

		Target = players.First() as Node3D;
	}

	public override void _PhysicsProcess(double delta)
	{
		if (Target == null) return;

		var direction = (Target.GlobalTransform.Origin - GlobalTransform.Origin).Normalized();
		
		direction.Y = 0;
		
		Velocity = direction * 5f;

		var lookDir = Target.GlobalTransform.Origin;

		lookDir.Y = GlobalPosition.Y;
		
		LookAt(lookDir, Vector3.Up);

		if (GlobalPosition.DistanceTo(Target.GlobalPosition) < 2f) return;
            
		MoveAndSlide();
	}
}
