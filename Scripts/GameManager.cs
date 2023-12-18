using Godot;
using System;
using System.Diagnostics;

public partial class GameManager : Node
{
	public static GameManager Instance;
	
	public static PlayerController PlayerController;

	public static CameraController CameraController;
    
	public override void _Ready()
	{
		Instance = this;
	}
	
	public void OnLevelLoaded(Node level)
	{
		PlayerController = level.GetNode<PlayerController>("player");
		CameraController = level.GetNode<CameraController>("camera");
	}
}
