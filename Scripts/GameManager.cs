using Godot;
using System;
using System.Diagnostics;
using System.Linq;
using Godot.Collections;
using testfps.Features.Multiplayer;

public partial class GameManager : Node
{
	public static GameManager Instance;

	public static GameManagerController Core;
	
	public static PlayerController PlayerController;

	public static PlayerDataController CurrentPlayerData;
	
	public static CameraController CameraController;

	public static string PlayerName = "Player";
    
	public override void _Ready()
	{
		var manager = GD.Load<PackedScene>("res://Features/GameManager/game_manager.tscn");

		Core = manager.Instantiate<GameManagerController>();
        
		AddChild(Core);
		
		Instance = this;
	}
	
	public void SpawnPlayer(int id, string name)
	{
		Core.AddNewPlayer(id, name);
	}
	
	public void UpdatePlayerName(int id, string newName)
	{
		Core.UpdatePlayerName(id, newName);
	}
	
	public void UpdatePlayerModel(int id, string newName)
	{
		Core.UpdatePlayerModel(id, newName);
	}
	
	public void SetCurrentPlayerData(int id)
	{
		CurrentPlayerData = Core.Players.First(x => x.Name == id.ToString());
	}
	
	public void OnLevelLoaded(Node level)
	{
		CameraController = level.GetNode<CameraController>("camera");
		
		var gameplay = GD.Load<PackedScene>("res://Features/Gameplay/gameplay.tscn");
		
		level.AddChild(gameplay.Instantiate());
	}
	
	public void SetPlayerName(string name)
	{
		PlayerName = name;
	}
}
