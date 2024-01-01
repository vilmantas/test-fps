using Godot;
using System;

public partial class AttackController : Node
{
	[Export] public PackedScene Hitbox;
	[Export] public Node3D HitboxSpawn;
	[Export] public float HitboxDelay = 0.3f;
	[Export] public float HitboxDuration = 0.3f;
	[Export] public float Cooldown;
	
	public Action<Node3D> OnHit;
	
	public bool AttackAvailable = true;
	
	public void Initialize(AttackConfiguration configuration)
	{
		HitboxDelay = configuration.HitboxDelay;
		HitboxDuration = configuration.HitboxDuration;
		Cooldown = configuration.Cooldown;
		Hitbox = configuration.Hitbox;
		HitboxSpawn = configuration.HitboxSpawn;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	public bool Attack()
	{
		if (!AttackAvailable) return false;

		AttackAvailable = false;
        
		var attackTimer = GetTree().CreateTimer(0.3f);

		var attackCooldownTimer = GetTree().CreateTimer(1f);
		
		attackTimer.Timeout += AttackTimerOnTimeout;

		attackCooldownTimer.Timeout += EnableAttack;

		return true;
	}
	
	void AttackTimerOnTimeout()
	{
		var instance = Hitbox.Instantiate<DamageHitboxController>();

		instance.Name = "hitbox_attack";
        
		instance.OnHit += HandleHit;

		instance.Source = GetParent<Node3D>();

		AddChild(instance);
		
		instance.GlobalPosition = HitboxSpawn.GlobalPosition;

		var despawnTimer = GetTree().CreateTimer(1f);

		despawnTimer.Timeout += () => RemoveHitbox(instance);
	}
	
	private void HandleHit(Node3D obj)
	{
		OnHit?.Invoke(obj);
	}
	
	void EnableAttack()
	{
		AttackAvailable = true;
	}
	
	private void RemoveHitbox(Node3D instance)
	{
		instance.QueueFree();
	}
}

[Serializable]
public class AttackConfiguration
{
	public float HitboxDelay { get; set; }
	public float HitboxDuration { get; set; }
	public float Cooldown { get; set; }
	public PackedScene Hitbox { get; set; }
	public Node3D HitboxSpawn { get; set; }
}
