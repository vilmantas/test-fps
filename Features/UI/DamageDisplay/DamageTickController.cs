using Godot;
using System;
using System.Diagnostics;

public partial class DamageTickController : Node3D
{
	[Export] Label3D DamageLabel;

	[Export] public Vector3 StartingPos;

	private double TimePassed;
	
	private double MaxDuration = 1d;

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
		
		var timer = GetTree().CreateTimer(MaxDuration);
		
		timer.Timeout += QueueFree;
	}

	public override void _Process(double delta)
	{
		DamageLabel.GlobalBasis = GetViewport().GetCamera3D().GlobalTransform.Basis;

		TimePassed += delta;
		
		var newPos = NewPos();

		GlobalPosition = newPos;

		x();
	}

	private void x()
	{
		var scale = TimePassed / MaxDuration;

		scale *= Mathf.Pi;
        
		var z = MathF.Sin((float)scale * 2);

		z = Mathf.Max(0, z);
		
		DamageLabel.Scale = Vector3.One * (1 + z);
		
		Debug.WriteLine($"{scale:F2}:{z:F2} : {DamageLabel.Scale}");
	}

	private Vector3 NewPos()
	{
		var scale = TimePassed / MaxDuration;
		
		var heightAdjustment = Mathf.Sqrt(scale * 0.7f);
        
		var newPos = StartingPos;
		
		newPos.Y += (float)heightAdjustment;
		return newPos;
	}
}
