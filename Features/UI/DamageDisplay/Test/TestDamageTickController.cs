using Godot;
using System;

public partial class TestDamageTickController : Node
{
	[Export] Node3D SpawnLocation;
	
	[Export] PackedScene DamageTickPrefab;
	
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("player_attack"))
		{
			var damageTick = DamageTickPrefab.Instantiate<DamageTickController>();
			
			var r = new RandomNumberGenerator();
			
			damageTick.Initialize(r.RandiRange(1, 20).ToString(), SpawnLocation.GlobalPosition);
			
			GetTree().CurrentScene.AddChild(damageTick);
		}
	}
}
