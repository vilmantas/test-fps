using System;
using System.Diagnostics;
using Godot;
using Godot.Collections;

namespace testfps.Scripts;

public partial class ClientManager : Node
{
    public static ClientManager Instance;

    [Export] public long ClientId;
    
    public Action OnJoined;
        
    
    public override void _Ready()
    {
        Instance = this;
    }

    public void OnJoin(long id)
    {
        ClientId = id;
        
        GameServerManager.Instance.UpdateClientName(GameManager.PlayerName);
        
        OnJoined?.Invoke();
    }

    public void OnStartGame()
    {
        Rpc("StartGame");
    }
    
    [Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true)]
    private void StartGame()
    {
        GameManager.Instance.SetCurrentPlayerData((int)ClientId);
        
        GetTree().ChangeSceneToFile("res://Scenes/Levels/level_00.tscn");
    }
}