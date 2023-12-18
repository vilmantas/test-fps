using Godot;
using System;

public partial class CameraRotationDebug : Label
{
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (GameManager.CameraController == null) return;

		Text = $"{GameManager.CameraController.RotationDegrees} : {GameManager.CameraController.Arm.RotationDegrees}";
	}
}
