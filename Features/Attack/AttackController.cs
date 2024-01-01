using Godot;
using System;

public partial class AttackController : Node
{
	[Export] public PackedScene Hitbox;
	[Export] public Node3D HitboxSpawn;
	[Export] public float HitboxDelay = 0.3f;
	[Export] public float HitboxDuration = 0.3f;
	[Export] public float Cooldown = 0.5f;
	
	public Action<Node3D> OnHit;
	
	public bool AttackAvailable = true;
	
	public bool Attack()
	{
		if (!AttackAvailable) return false;

		AttackAvailable = false;
        
		var attackTimer = GetTree().CreateTimer(HitboxDelay);

		var attackCooldownTimer = GetTree().CreateTimer(Cooldown);
		
		attackTimer.Timeout += AttackTimerOnTimeout;

		attackCooldownTimer.Timeout += EnableAttack;

		return true;
	}
	
	private void AttackTimerOnTimeout()
	{
		var instance = Hitbox.Instantiate<DamageHitboxController>();

		instance.Name = "hitbox_attack";
        
		instance.OnHit += HandleHit;

		instance.Source = GetParent<Node3D>();

		AddChild(instance);
		
		instance.GlobalPosition = HitboxSpawn.GlobalPosition;

		var despawnTimer = GetTree().CreateTimer(HitboxDuration);

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
