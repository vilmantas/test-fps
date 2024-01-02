using System.Diagnostics;
using System.Linq;
using Godot;
using Godot.Collections;
using testfps.Features.Multiplayer;

public partial class GameManagerController : Node
{
	[Export] public PackedScene PlayerDataScene;

	[Export] private Dictionary<long, PlayerDataController> m_Players = new();
	
	public PlayerDataController[] Players => Container.GetChildren().Cast<PlayerDataController>().Where(x => !x.IsFreeLook).ToArray();
	
	public PlayerDataController[] Spectators => Container.GetChildren().Cast<PlayerDataController>().Where(x => x.IsFreeLook).ToArray();

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

		player.SelectedSkin = "res://Imports/meshes/characters/sporty_male.res";
		
		player.SelectedWeapon = "res://Assets/Weapons/sword_bobo.tscn";
        
		Container.AddChild(player, true);
		
		m_Players.Add(id, player);
	}
	
	public void UpdatePlayerName(int id, string name)
	{
		Debug.WriteLine(m_Players.Count);
		
		if (!m_Players.ContainsKey(id)) return;

		m_Players[id].PlayerName = name;
	}
	
	public void UpdatePlayerModel(int id, string model)
	{
		if (!m_Players.ContainsKey(id)) return;

		m_Players[id].SelectedSkin = model;
	}

	public void UpdatePlayerWeapon(int id, string model)
	{
		if (!m_Players.ContainsKey(id)) return;

		m_Players[id].SelectedWeapon = model;
	}
	
	
	public PlayerDataController GetByName(string name) => Players.First(x => x.PlayerName == name);
	
	public PlayerDataController GetById(int id) => Players.First(x => x.Name == id.ToString());
	
}
