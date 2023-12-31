using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

public partial class DamageHitboxController : Node3D
{
	private Area3D HitboxArea;

	private Node Parent;

	public bool AllowMultipleCollisions;

	public List<Node> Hits = new();

	public Action<Node3D> OnHit;
    
	public override void _Ready()
	{
		HitboxArea = GetNode<Area3D>("area");
		
		HitboxArea.BodyEntered += OnHitboxHit;

		Parent = GetParent();
	}
	
	void OnHitboxHit(Node3D body)
	{
		if (body == GetParent()) return;

		if (!AllowMultipleCollisions && Hits.Any(x => x == body)) return;
		
		TargetHit(body);
	}

	private void TargetHit(Node3D body)
	{
		Hits.Add(body);
		
		OnHit?.Invoke(body);
		
		Debug.Print($"Attacked {body.Name}");
	}
}
