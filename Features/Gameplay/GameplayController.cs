using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Godot;
using testfps.Features.Multiplayer;
using testfps.Features.SpawnPoint;

namespace testfps.Scripts;

public partial class GameplayController : Node
{
    public List<PlayerController> Players = new();

    private Node3D Container;
    
    private SpawnPointController[] Spawns;
    
    public override void _Ready()
    {
        var player = GD.Load<PackedScene>("res://Features/Player/player.tscn");

        Container = GetParent().GetNode<Node3D>("container_players");
        
        Spawns = GetTree().GetNodesInGroup("location_spawn").Cast<SpawnPointController>().ToArray();

        for (var i = 0; i < GameManager.Core.Players.Length; i++)
        {
            var data = GameManager.Core.Players[i];
            
            var instance = player.Instantiate<PlayerController>();
            
            if (data == GameManager.CurrentPlayerData)
            {
                SetLocalInfo(instance);
            }
            
            InitializePlayer(instance, data, Spawns[i % Spawns.Length]);
            
            Players.Add(instance);
        }
    }

    private void InitializePlayer(PlayerController player, PlayerDataController data, SpawnPointController spawn)
    {
        player.SetMultiplayerAuthority(data.Id);

        player.Name = data.PlayerName;
        
        player.PlayerModel = GD.Load<Mesh>(data.SelectedSkin);
        
        player.Weapon = GD.Load<PackedScene>(data.SelectedWeapon);

        Container.AddChild(player, true);
            
        player.GlobalPosition = spawn.SpawnLocation.GlobalPosition;
    }

    private void SetLocalInfo(PlayerController player)
    {
        GameManager.PlayerController = player;

        GameManager.CameraController.Initialize(player);
    }
}