using Godot;
using System;
using System.Diagnostics;
using testfps.Scripts;

public partial class LobbyController : Node
{
	[Export] private PackedScene PlayerPrefab;
    
	private Node PlayerContainer;

	public override void _Ready()
	{
		PlayerContainer = GetNode<Node>("container_players");

		var StartButton = GetNode<Button>("container_buttons/start_button");

		var SetKnightButton = GetNode<Button>("container_buttons/set_knight");
		
		var SetSportyButton = GetNode<Button>("container_buttons/set_sporty");
		
		var SetJacketButton = GetNode<Button>("container_buttons/set_jacket");
		
		var Sword1Button = GetNode<Button>("container_buttons/set_sword_1");
		
		var Sword4Button = GetNode<Button>("container_buttons/set_sword_4");
        
		SetKnightButton.Pressed += () => GameServerManager.Instance.UpdateClientModel("res://Imports/meshes/characters/knight_1.res");
		
		SetSportyButton.Pressed += () => GameServerManager.Instance.UpdateClientModel("res://Imports/meshes/characters/sporty_male.res");
		
		SetJacketButton.Pressed += () => GameServerManager.Instance.UpdateClientModel("res://Imports/meshes/characters/male_jacket.res");
		
		Sword1Button.Pressed += () => GameServerManager.Instance.UpdateClientWeapon("res://Assets/Weapons/weapon_sword_1.tsnc");
		
		Sword4Button.Pressed += () => GameServerManager.Instance.UpdateClientWeapon("res://Assets/Weapons/weapon_sword_4.tscn");
		
		StartButton.Pressed += OnStartPressed;
		
		if (Multiplayer.IsServer()) return;
		
		StartButton.Hide();
	}

	private void OnStartPressed()
	{
		GameServerManager.Instance.StartGame();
	}

	public override void _Process(double delta)
	{
		foreach (var child in PlayerContainer.GetChildren())
		{
			PlayerContainer.RemoveChild(child);
			
			child.QueueFree();
		}
        
		foreach (var data in GameManager.Core.Players)
		{
			var instance = PlayerPrefab.Instantiate<LobbyPlayerController>();
		
			instance.Name = data.PlayerName;
			
			PlayerContainer.AddChild(instance, true);
			
			instance.Initialize(data);
		}
	}
}
