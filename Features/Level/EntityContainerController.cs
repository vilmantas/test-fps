using Godot;
using System;

public partial class EntityContainerController : Node
{
	[Export] public Node3D PlayersContainer;

	[Export] public Node3D EnemiesContainer;

	[Export] public MultiplayerSpawner EnemySpawner;
    
	[Export] public Node3D ProjectilesContainer;

	[Export] public MultiplayerSpawner ProjectileSpawner;
	
	[Export] public Node3D UIElementsContainer;
	
	[Export] public MultiplayerSpawner UIElementsSpawner;
}