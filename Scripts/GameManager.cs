using Godot;
using System;
using System.Diagnostics;
using Godot.Collections;

public partial class GameManager : Node
{
	public static GameManager Instance;
	
	public static PlayerController PlayerController;

	public static CameraController CameraController;

	public static Dictionary<long, string> ConnectedPlayers = new();
    
	public override void _Ready()
	{
		Instance = this;
	}
	
	public void OnPlayerConnected(long id, string name)
	{
		ConnectedPlayers.Add(id, name);
	}
	
	public void OnLevelLoaded(Node level)
	{
		PlayerController = level.GetNode<PlayerController>("player");
		CameraController = level.GetNode<CameraController>("camera");
	}
}
