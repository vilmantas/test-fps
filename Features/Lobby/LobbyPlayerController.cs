using Godot;
using System;
using testfps.Features.Multiplayer;
using testfps.Scripts;

public partial class LobbyPlayerController : Node
{
	private Label PlayerNameLabel;

	private Label PlayerModelLabel;
	
	private Label PlayerWeaponLabel;
    
	public override void _Ready()
	{
		PlayerNameLabel = GetNode<Label>("container/label_name");

		PlayerModelLabel = GetNode<Label>("container/label_model");
		
		PlayerWeaponLabel = GetNode<Label>("container/label_weapon");
	}

	public void Initialize(PlayerDataController data)
	{
		PlayerNameLabel.Text = data.PlayerName;
		
		PlayerModelLabel.Text = data.SelectedSkin;
		
		PlayerWeaponLabel.Text = data.SelectedWeapon;
	}
}
