using Godot;
using System;
using testfps.Features.Multiplayer;
using testfps.Scripts;

public partial class LobbyPlayerController : Node
{
	private Label PlayerNameLabel;

	private Label PlayerModelLabel;
	public override void _Ready()
	{
		PlayerNameLabel = GetNode<Label>("container/label_name");

		PlayerModelLabel = GetNode<Label>("container/label_model");
	}

	public void Initialize(PlayerDataController data)
	{
		PlayerNameLabel.Text = data.PlayerName;
		
		PlayerModelLabel.Text = data.SelectedSkin;
	}
}
