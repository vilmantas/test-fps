using Godot;
using System;

public partial class AttackController : Node
{
	[Export] public Node3D HitboxSpawn;
	
	public Action<Node3D> OnHit;
	
	public bool AttackAvailable = true;
	
	public bool Attack(WeaponController weapon)
	{
		if (!AttackAvailable) return false;

		AttackAvailable = false;
        
		var attackStartTimer = GetTree().CreateTimer(weapon.HitboxConfiguration.Delay);

		var attackCooldownTimer = GetTree().CreateTimer(weapon.WeaponConfiguration.AttackCooldown);
		
		attackStartTimer.Timeout += () => AttackTimerOnTimeout(weapon);

		attackCooldownTimer.Timeout += EnableAttack;

		return true;
	}
	
	private void AttackTimerOnTimeout(WeaponController weapon)
	{
		var instance = weapon.HitboxConfiguration.HitboxModel.Instantiate<DamageHitboxController>();

		instance.Name = "hitbox_attack";
        
		instance.OnHit += HandleHit;

		instance.Source = GetParent<Node3D>();

		AddChild(instance);
		
		instance.GlobalPosition = HitboxSpawn.GlobalPosition;

		var despawnTimer = GetTree().CreateTimer(weapon.HitboxConfiguration.Duration);

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
