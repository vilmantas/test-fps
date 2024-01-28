using Godot;
using System;
using System.Diagnostics;
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

	private AttackController AttackModule;
	
	private WeaponController Weapon;

	public override void _Ready()
	{
		DebugLabel = GetNode<Label3D>("debug_label");
		
		AttackModule = GetNode<AttackController>("attack_module");
		
		AttackModule.OnHit += DamageTarget;
		
		Weapon = GetNode<WeaponController>("weapon_slicer");
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
		if (CurrentState == EnemyState.Attack && !AttackModule.AttackAvailable) return;
        
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
		switch (CurrentState)
		{
			case EnemyState.Idle:
				break;
			case EnemyState.Chase:
				break;
			case EnemyState.Attack:
				HandleAttack();
				break;
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		if (CurrentState != EnemyState.Chase) return;

		var direction = (Target.GlobalTransform.Origin - GlobalTransform.Origin).Normalized();
		
		direction.Y = 0;
		
		Velocity = direction * 5f;

		var lookDir = Target.GlobalTransform.Origin;

		lookDir.Y = GlobalPosition.Y;
		
		LookAt(lookDir, Vector3.Up);

		if (GlobalPosition.DistanceTo(Target.GlobalPosition) < 2f) return;
            
		MoveAndSlide();
	}

	public void HandleAttack()
	{
		AttackModule.Attack(Weapon);
	}

	private void DamageTarget(Node3D target)
	{
		var health = target.GetNode<HealthController>("health_module");
		
		health.Damage(1);
	}
}
