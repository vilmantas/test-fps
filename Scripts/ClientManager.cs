using System;
using System.Diagnostics;
using Godot;
using Godot.Collections;

namespace testfps.Scripts;

public partial class ClientManager : Node
{
    public static ClientManager Instance;

    private long ClientId;
    
    public Action OnJoined;
        
    
    public override void _Ready()
    {
        Instance = this;
    }

    public void OnJoin(long id)
    {
        ClientId = id;
        
        GameServerManager.Instance.RequestConnectedPlayers(id);
        
        OnJoined?.Invoke();
    }
    
    public void SetConnectedPlayers(Dictionary<long, string> players)
    {
        Debug.Print(players.Keys.ToString());
        GameManager.Instance.SetConnectedPlayers(players);
    }
}