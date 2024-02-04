using Godot;
using System;
using testfps.Scripts;

public partial class AttackController : Node
{
	[Export] public Node3D HitboxSpawn;
	
	public Action<Node3D> OnHit;
	
	public bool AttackAvailable = true;

	public bool Attack(WeaponController weapon)
	{
		if (!AttackAvailable) return false;

		AttackAvailable = false;

		return weapon.MeleeWeaponConfiguration != null ? InitiateMeleeAttack(weapon) : InitiateRangedAttack(weapon);
	}

	private bool InitiateMeleeAttack(WeaponController weapon)
	{
		var attackStartTimer = GetTree().CreateTimer(weapon.MeleeWeaponConfiguration.HitboxConfiguration.Delay);

		var attackCooldownTimer = GetTree().CreateTimer(weapon.WeaponConfiguration.Cooldown);
		
		attackStartTimer.Timeout += () => AttackTimerOnTimeout(weapon);

		attackCooldownTimer.Timeout += EnableAttack;

		return true;
	}

	private bool InitiateRangedAttack(WeaponController weapon)
	{
		var projectile = weapon.RangedWeaponConfiguration.ProjectileModel.Instantiate<ProjectileController>();
		
		projectile.Initialize(this, -GetParent<Node3D>().GlobalTransform.Basis.Z, weapon.RangedWeaponConfiguration.ProjectileSpeed, HandleHit);
		
		GameManager.CurrentGameplay.Containers.ProjectilesContainer.AddChild(projectile, true);

		projectile.GlobalPosition = HitboxSpawn.GlobalPosition;
        
		var attackCooldownTimer = GetTree().CreateTimer(weapon.WeaponConfiguration.Cooldown);

		attackCooldownTimer.Timeout += EnableAttack;
		
		return true;
	}
	
	private void AttackTimerOnTimeout(WeaponController weapon)
	{
		var instance = weapon.MeleeWeaponConfiguration.HitboxConfiguration.HitboxModel.Instantiate<DamageHitboxController>();

		instance.AllowMultipleCollisions = false;
		
		instance.Name = "hitbox_attack";
        
		instance.OnHit += HandleHit;

		instance.Source = GetParent<Node3D>();

		HitboxSpawn.AddChild(instance);
		
		instance.GlobalPosition = HitboxSpawn.GlobalPosition;

		var despawnTimer = GetTree().CreateTimer(weapon.MeleeWeaponConfiguration.HitboxConfiguration.Duration);

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
