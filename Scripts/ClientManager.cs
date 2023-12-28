using System.Diagnostics;
using Godot;
using Godot.Collections;

namespace testfps.Scripts;

public partial class ClientManager : Node
{
    public static ClientManager Instance;
    
    public override void _Ready()
    {
        Instance = this;
    }

    public void OnJoin(long id)
    {
        Debug.Print("Requesting Players");
        Rpc("GetConnectedPlayers", id);
    }
    
        
    [Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = false, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
    public void GetConnectedPlayers(long id)
    {
        Debug.Print($"Player {id} is requesting players list");
        Debug.Print(Multiplayer.IsServer().ToString());
        
        RpcId(id, "SetConnectedPlayers", GameManager.ConnectedPlayers);
    }
    
    [Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = false, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
    public void SetConnectedPlayers(Dictionary<long, string> players)
    {
        GameManager.ConnectedPlayers = players;
    }
}