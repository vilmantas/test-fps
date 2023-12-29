using Godot;
using Godot.Collections;

namespace testfps.Scripts;

public partial class GameServerManager : Node
{
    public static GameServerManager Instance;

    public override void _Ready()
    {
        Instance = this;
    }
    
    public void RequestConnectedPlayers(long id)
    {
        Rpc("GetConnectedPlayers", id);
    }
    
    public void UpdateClientName(long id, string name)
    {
        Rpc("SetPlayerName", name);
    }
    
    [Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = false, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
    public void GetConnectedPlayers(long id)
    {
        RpcId(id, "SetConnectedPlayers", GameManager.ConnectedPlayers);
    }
    
    [Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = false, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
    public void SetConnectedPlayers(Dictionary<long, string> players)
    {
        ClientManager.Instance.SetConnectedPlayers(players);
    }
}