using System.Diagnostics;
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

    public void StartGame()
    {
        ClientManager.Instance.OnStartGame();
    }
    
    public void OnPlayerConnected(long id, string name)
    {
        GameManager.Instance.SpawnPlayer((int)id, name);
    }
    
    public void UpdateClientName(string name)
    {
        SendHostMessage("UpdatePlayerName", name);
    }
    
    public void UpdateClientModel(string model)
    {
        SendHostMessage("UpdatePlayerModel", model);
    }
    
    [Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
    private void UpdatePlayerName(string name)
    {
        var id = Multiplayer.GetRemoteSenderId();

        GameManager.Instance.UpdatePlayerName(id, name);
    }
    
    [Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
    private void UpdatePlayerModel(string model)
    {
        var id = Multiplayer.GetRemoteSenderId();

        GameManager.Instance.UpdatePlayerModel(id, model);
    }
    
    private void SendHostMessage(string method, params Variant[] args)
    {
        RpcId(1, method, args);
    }
}