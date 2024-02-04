using Godot;
using System;

public partial class DamageTickController : Node3D
{
	[Export] Label3D DamageLabel;

	[Export] public Vector3 StartingPos;

	private double TimePassed;

	public void Initialize(string text, Vector3 position)
	{
		Rpc("DoInitialize", text, position);
	}
	
	[Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true)]
	private void DoInitialize(string text, Vector3 position)
	{
		DamageLabel.Text = text;

		StartingPos = position;
	}

	public override void _Ready()
	{
		DamageLabel.Show();

		if (!IsMultiplayerAuthority()) return;
		
		var timer = GetTree().CreateTimer(3f);
		
		timer.Timeout += QueueFree;
	}

	public override void _Process(double delta)
	{
		DamageLabel.GlobalBasis = GetViewport().GetCamera3D().GlobalTransform.Basis;

		TimePassed += delta;
		
		var some = 0.5f * Mathf.Sqrt(TimePassed);
        
		var newPos = StartingPos;
		
		newPos.Y += (float)some;
		
		GlobalPosition = newPos;
	}
}
