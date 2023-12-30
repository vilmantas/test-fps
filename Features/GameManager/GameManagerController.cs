using Godot;
using System;
using System.Linq;
using Godot.Collections;
using testfps.DebugScripts.UI;
using testfps.Features.Multiplayer;

public partial class GameManagerController : Node
{
	[Export] public PackedScene PlayerDataScene;

	[Export] private Dictionary<int, PlayerDataController> m_Players = new();
	
	public PlayerDataController[] Players => Container.GetChildren().Cast<PlayerDataController>().ToArray();

	private Node Container;
    
	public override void _Ready()
	{
		Container = GetNode<Node>("container_players");
	}

	public void AddNewPlayer(int id, string name)
	{
		var player = PlayerDataScene.Instantiate<PlayerDataController>();
		
		player.Initialize(name);
		
		player.Name = id.ToString();

		player.SelectedSkin = "res://Imports/meshes/sporty_male.res";
        
		Container.AddChild(player, true);
		
		m_Players.Add(id, player);
	}
	
	public void UpdatePlayerName(int id, string name)
	{
		if (!m_Players.ContainsKey(id)) return;

		m_Players[id].PlayerName = name;
	}
	
	public void UpdatePlayerModel(int id, string model)
	{
		if (!m_Players.ContainsKey(id)) return;

		m_Players[id].SelectedSkin = model;
	}
	
	public PlayerDataController GetByName(string name)
	{
		return Players.First(x => x.PlayerName == name);
	}
	
	public PlayerDataController GetById(int id)
	{
		return Players.First(x => x.Name == id.ToString());
	}
}
