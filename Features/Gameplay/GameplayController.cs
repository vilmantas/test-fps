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

    public EntityContainerController Containers;
    
    public Node3D ContainerPlayers;
    
    public Node3D ContainerEnemies;

    private Dictionary<string, int> PlayerKills = new();
    
    private SpawnPointController[] Spawns;
    
    public override void _Ready()
    {
        var player = GD.Load<PackedScene>("res://Features/Player/player.tscn");

        Containers = GetParent().GetNode<EntityContainerController>("entity_containers");

        ContainerPlayers = Containers.PlayersContainer;

        ContainerEnemies = Containers.EnemiesContainer;
        
        Spawns = GetTree().GetNodesInGroup("location_spawn").Cast<SpawnPointController>().ToArray();

        foreach (var data in GameManager.Core.Spectators) 
        {
            AddSpectator(data);
        }
        
        for (var i = 0; i < GameManager.Core.Players.Length; i++)
        {
            AddPlayer(i, player);
        }
        
        AddDungeonMaster();
    }
    
    private void AddDungeonMaster()
    {
        var data = GameManager.Core.Boss;

        if (data == null) return;
        
        var dungeonMasterScene = GD.Load<PackedScene>("res://Features/DungeonMaster/dungeon_master.tscn");
        
        var instance = dungeonMasterScene.Instantiate<DungeonMasterController>();
        
        GetParent().AddChild(instance);

        instance.CameraFreeLook.Initialize(GameManager.CurrentPlayerData == data);
        
        if (GameManager.CurrentPlayerData == data)
        {
            GameManager.CameraController = instance.CameraFreeLook;
        }
        else
        {
            instance.ProcessMode = ProcessModeEnum.Disabled;
        }
    }
    

    private void AddPlayer(int i, PackedScene player)
    {
        var data = GameManager.Core.Players[i];
            
        var instance = player.Instantiate<PlayerController>();
        
        InitializePlayer(instance, data, Spawns[i % Spawns.Length]);
        
        Players.Add(instance);
    }

    private void AddSpectator(PlayerDataController data)
    {
        var prefab = GD.Load<PackedScene>("res://Features/Camera/FreeLook/camera_free_look.tscn");
        
        var instance = prefab.Instantiate<CameraFreeLookController>();
        
        GetParent().AddChild(instance);

        if (data == GameManager.CurrentPlayerData)
        {
            instance.MouseSensitivity = 10f;
            GameManager.CameraController = instance;

            instance.Camera.Current = true;
            
            instance.Initialize(true);
        }
        else
        {
            instance.Initialize(false);
        }
    }
    
    private void InitializePlayer(PlayerController player, PlayerDataController data, SpawnPointController spawn)
    {
        player.SetMultiplayerAuthority(data.Id);

        player.Name = data.PlayerName;
        
        player.PlayerModel = GD.Load<Mesh>(data.SelectedSkin);
        
        player.EquippedWeapon = GD.Load<PackedScene>(data.SelectedWeapon);

        ContainerPlayers.AddChild(player, true);
            
        player.GlobalPosition = spawn.SpawnLocation.GlobalPosition;
        
        if (data == GameManager.CurrentPlayerData)
        {
            SetLocalInfo(player);
        }
    }

    private void SetLocalInfo(PlayerController player)
    {
        GameManager.PlayerController = player;

        var cameraScene = GD.Load<PackedScene>("res://Features/Camera/3rdPerson/camera_3rd_person.tscn");

        var cameraInstance = cameraScene.Instantiate<Camera3rdPersonController>();
        
        GetParent().AddChild(cameraInstance);
        
        cameraInstance.Initialize(player);
        
        cameraInstance.Camera.Current = true;

        GameManager.CameraController = cameraInstance;
    }

    public void SpawnEnemy(EnemyController enemy)
    {
        enemy.Name = "Nate";
        
        ContainerEnemies.AddChild(enemy, true);
    }
    
    public void OnEnemyDeath(EnemyController enemy, Node3D killer)
    {
        enemy.HandleDeath(killer);
        
        if (killer is PlayerController player)
        {
            if (PlayerKills.TryAdd(player.Name, 0) == false)
            {
                PlayerKills[player.Name] += 1;
            }
        }
        
        ContainerEnemies.RemoveChild(enemy);
        
        enemy.QueueFree();
    }

    public Node3D FindPlayerOrEnemy(string name) => AllEntities.FirstOrDefault(x => x.Name == name);

    public List<Node3D> AllEntities =>
        ContainerPlayers.GetChildren().Concat(ContainerEnemies.GetChildren()).ToList().Cast<Node3D>().ToList();
    
    public void OnPlayerDeath(PlayerController player)
    {
    }
}