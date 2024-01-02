using Godot;
using System;
using System.Diagnostics;
using System.Linq;
using testfps.Scripts;

public partial class LobbyController : Node
{
	[Export] private PackedScene PlayerPrefab;
    
	private Node PlayerContainer;

	public override void _Ready()
	{
		PlayerContainer = GetNode<Node>("container_players");

		var StartButton = GetNode<Button>("container_buttons/start_button");
		
		var FreeLookButton = GetNode<Button>("container_buttons/set_free_look");

		var SetKnightButton = GetNode<Button>("container_buttons/set_knight");
		
		var SetSportyButton = GetNode<Button>("container_buttons/set_sporty");
		
		var SetJacketButton = GetNode<Button>("container_buttons/set_jacket");
		
		var Sword1Button = GetNode<Button>("container_buttons/set_sword_1");
		
		var Sword4Button = GetNode<Button>("container_buttons/set_sword_4");
        
		SetKnightButton.Pressed += () => UpdateClientModel("res://Imports/meshes/characters/knight_1.res");
		
		SetSportyButton.Pressed += () => UpdateClientModel("res://Imports/meshes/characters/sporty_male.res");
		
		SetJacketButton.Pressed += () => UpdateClientModel("res://Imports/meshes/characters/male_jacket.res");
		
		Sword1Button.Pressed += () => UpdateClientWeapon("res://Assets/Weapons/sword_bobo.tsnc");
		
		Sword4Button.Pressed += () => UpdateClientWeapon("res://Assets/Weapons/sword_slicer.tscn");

		StartButton.Pressed += GameServerManager.Instance.StartGame;
		
		FreeLookButton.Pressed += FreeLookButtonOnPressed;

		if (Multiplayer.IsServer()) return;
		
		StartButton.Hide();
		
		FreeLookButton.Hide();
	}

	private void FreeLookButtonOnPressed()
	{
		var data = GameManager.CurrentPlayerData;
		
		data.IsFreeLook = true;
		
		GameServerManager.Instance.UpdateClientData(data);
	}
	
	public void UpdateClientName(string name)
	{
		var data = GameManager.CurrentPlayerData;
		
		data.PlayerName = name;
		
		GameServerManager.Instance.UpdateClientData(data);
	}
	
	public void UpdateClientModel(string model)
	{
		var data = GameManager.CurrentPlayerData;
		
		data.SelectedSkin = model;
		
		GameServerManager.Instance.UpdateClientData(data);
	}
	
	void UpdateClientWeapon(string weapon)
	{
		var data = GameManager.CurrentPlayerData;
		
		data.SelectedWeapon = weapon;
		
		GameServerManager.Instance.UpdateClientData(data);
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
