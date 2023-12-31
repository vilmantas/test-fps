using Godot;
using System;

public partial class HealthController : Node
{
	[Export] public int MaxHealth = 10;
	[Export] public int StartingHealth = 8;

	public int CurrentHealth;
	
	public override void _Ready()
	{
		CurrentHealth = Math.Min(StartingHealth, MaxHealth);
	}

	public void Damage(int amount)
	{
		if (amount < 0) return;

		CurrentHealth = Math.Max(CurrentHealth - amount, 0);
	}
	
	public bool IsExpended => CurrentHealth <= 0;
}
