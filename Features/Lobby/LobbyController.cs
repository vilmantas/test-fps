using Godot;
using System;

public partial class LobbyController : Node
{
	[Export] private PackedScene PlayerPrefab;
    
	private Node PlayerContainer;
	
	public override void _Ready()
	{
		PlayerContainer = GetNode<Node>("container_players");
	}

	public override void _Process(double delta)
	{
		foreach (var child in PlayerContainer.GetChildren())
		{
			PlayerContainer.RemoveChild(child);
			
			child.QueueFree();
		}
        
		foreach (var players in GameManager.ConnectedPlayers)
		{
			var instance = PlayerPrefab.Instantiate<LobbyPlayerController>();

			instance.Name = players.Value;
			
			PlayerContainer.AddChild(instance, true);
			
			instance.Initialize(players.Value);
		}
	}
}
