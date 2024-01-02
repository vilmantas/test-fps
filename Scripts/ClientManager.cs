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
        
        OnJoined?.Invoke();
    }

    [Rpc(CallLocal = false)]
    public void OnPlayerSpawned()
    {
        GameManager.Instance.SetCurrentPlayerData(ClientId);
    }
    
    [Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true)]
    public void StartGame()
    {
        GetTree().ChangeSceneToFile("res://Scenes/Levels/level_00.tscn");
    }
    
    public void SendClientMessage(long id, string method, params Variant[] args)
    {
        RpcId(id, method, args);
    }
    
    public void SendClientMessage(string method, params Variant[] args)
    {
        Rpc(method, args);
    }
}