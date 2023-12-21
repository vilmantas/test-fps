using Godot;
using System;

public partial class PlayerPositionDebug : Label
{
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (GameManager.PlayerController == null) return;
		
		Text = GameManager.PlayerController.Position.ToString();
	}
}
