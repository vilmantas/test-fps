using Godot;
using System;
using System.Security.Cryptography;

public partial class ProjectileController : Node3D
{
	[Export] public DamageHitboxController HitboxController;

	public Node Source;

	public Vector3 Direction;

	public float Speed;

	public override void _Ready()
	{
		if (!IsMultiplayerAuthority()) return;
		
		var attackCooldownTimer = GetTree().CreateTimer(3f);

		attackCooldownTimer.Timeout += QueueFree;
	}

	public void Initialize(Node source, Vector3 direction, float speed, Action<Node3D> hitCallback)
	{
		Source = source;
		
		Direction = direction;
		
		Speed = speed;
		
		HitboxController.OnHit += hit => WrappedCallback(hit, hitCallback);
	}
	
	private void WrappedCallback(Node3D target, Action<Node3D> callback)
	{
		callback(target);
		
		QueueFree();
	}

	public override void _PhysicsProcess(double delta)
	{
		LookAt(Direction, Vector3.Up);
		GlobalPosition += Direction * Speed * (float)delta;
	}
}
