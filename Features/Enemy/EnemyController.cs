using Godot;
using System;
using System.Linq;

public enum EnemyState
{
	Idle,
	Chase,
	Attack
}

public partial class EnemyController : CharacterBody3D
{
	private Node3D Target;
	
	private Label3D DebugLabel;

	private EnemyState CurrentState;

	public override void _Ready()
	{
		DebugLabel = GetNode<Label3D>("debug_label");
	}

	public override void _Process(double delta)
	{
		CheckTarget();

		CheckState();
        
		HandleState();

		if (DebugLabel == null) return;
		
		DebugLabel.Text = CurrentState.ToString();
		
		DebugLabel.GlobalBasis = GetViewport().GetCamera3D().GlobalTransform.Basis;
	}

	private void CheckTarget()
	{
		var players = GetTree().GetNodesInGroup("player");

		if (!players.Any())
		{
			Target = null;

			return;
		}
		
		Target = players.First() as Node3D;
	}

	private void CheckState()
	{
		if (Target == null)
		{
			CurrentState = EnemyState.Idle;
			
			return;
		}
		
		if (GlobalPosition.DistanceTo(Target.GlobalPosition) < 2f)
		{
			CurrentState = EnemyState.Attack;
		}
		else
		{
			CurrentState = EnemyState.Chase;
		}
	}
	
	private void HandleState()
	{
		// switch (CurrentState)
		// {
		// 	case EnemyState.Idle:
		// 		HandleIdle();
		// 		break;
		// 	case EnemyState.Chase:
		// 		HandleChase();
		// 		break;
		// 	case EnemyState.Attack:
		// 		HandleAttack();
		// 		break;
		// }
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
