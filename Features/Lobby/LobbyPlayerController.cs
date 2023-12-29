using Godot;
using System;

public partial class LobbyPlayerController : Node
{
	private Label PlayerNameLabel;
	public override void _Ready()
	{
		PlayerNameLabel = GetNode<Label>("label_name");
	}

	public void Initialize(string name)
	{
		PlayerNameLabel.Text = name;
	}
}
