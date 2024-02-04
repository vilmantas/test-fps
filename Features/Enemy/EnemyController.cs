using Godot;
using System;
using System.Diagnostics;
using System.Linq;
using testfps.Features.Combat;

public enum EnemyState
{
	Idle,
	Chase,
	Attack
}

public partial class EnemyController : CharacterBody3D
{
	[Export] public WeaponController Weapon;
	
	[Export] public float ChaseRange = 2f;
    
	public Node3D Target;
	
	public NavigationAgent3D NavigationAgent;
	
	public Label3D DebugLabel;

	public EnemyState CurrentState;

	private AttackController AttackModule;
	
	public HealthController HealthModule;

	public override void _Ready()
	{
		HealthModule = GetNode<HealthController>("health_module");
		
		DebugLabel = GetNode<Label3D>("debug_label");
		
		AttackModule = GetNode<AttackController>("attack_module");
		
		AttackModule.OnHit += DamageTarget;
		
		Weapon ??= GetNode<WeaponController>("weapon_slicer");
		
		NavigationAgent = GetNode<NavigationAgent3D>("navigation_agent");
	}

	public override void _Process(double delta)
	{
		if (DebugLabel != null)
		{
			DebugLabel.Text = HealthModule.CurrentHealth.ToString();

			DebugLabel.GlobalBasis = GetViewport().GetCamera3D().GlobalTransform.Basis;			
		}

		if (!IsMultiplayerAuthority()) return;
		
		CheckTarget();
		
		CheckState();
        
		HandleState();
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
		
		if (GlobalPosition.DistanceTo(Target.GlobalPosition) < ChaseRange)
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
				NavigationAgent.TargetPosition = Target.GlobalTransform.Origin;
				break;
			case EnemyState.Attack:
				HandleAttack();
				break;
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		if (CurrentState == EnemyState.Attack)
		{
			LookAt(Target.GlobalPosition, Vector3.Up);
		}
		
		if (CurrentState != EnemyState.Chase) return;

		var destination = NavigationAgent.GetNextPathPosition();

		if (destination == GlobalTransform.Origin) return;
		
		var direction = (destination - GlobalTransform.Origin).Normalized();
		
		direction.Y = 0;
		
		Velocity = direction * 5f;

		var lookDir = destination;

		lookDir.Y = GlobalPosition.Y;
		
		LookAt(lookDir, Vector3.Up);

		if (GlobalPosition.DistanceTo(Target.GlobalPosition) < ChaseRange) return;
            
		MoveAndSlide();
	}

	public void HandleAttack()
	{
		AttackModule.Attack(Weapon);
	}

	private void DamageTarget(Node3D target)
	{
		if (target is EnemyController) return;
		
		CombatManager.Instance.Damage(this, target);
	}

	public void HandleDeath(Node3D killer)
	{
		ProcessMode = ProcessModeEnum.Disabled;
	}
}
