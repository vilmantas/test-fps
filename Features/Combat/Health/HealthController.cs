using Godot;
using System;

public partial class HealthController : Node
{
	[Export] public int MaxHealth = 10;
	[Export] public int CurrentHealth = 8;
	
	public Action<int , int> OnHealthChanged;
	
	public override void _Ready()
	{
		CurrentHealth = Math.Min(CurrentHealth, MaxHealth);
	}

	public void Damage(int amount)
	{
		if (amount < 0) return;
		if (IsExpended) return;
		
		Rpc("TakeDamage", amount);
	}
	
	[Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true)]
	private void TakeDamage(int amount)
	{
		var previousHealth = CurrentHealth;
		
		CurrentHealth = Math.Max(CurrentHealth - amount, 0);
		
		OnHealthChanged?.Invoke(previousHealth, CurrentHealth);
	}
	
	public bool IsExpended => CurrentHealth <= 0;
	
	public bool IsAlive => !IsExpended;
}
