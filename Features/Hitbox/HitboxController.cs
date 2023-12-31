using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

public partial class HitboxController : Node3D
{
	private Area3D HitboxArea;

	private Node Parent;

	public bool AllowMultipleCollisions;

	public List<Node> Hits = new();
    
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
		
		Hits.Add(body);
        
		Debug.Print($"Attacked {body.Name}");
	}
}
