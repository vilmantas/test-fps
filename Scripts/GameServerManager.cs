using System.Diagnostics;
using Godot;
using Godot.Collections;
using testfps.Features.Multiplayer;

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
        ClientManager.Instance.SendClientMessage(nameof(ClientManager.Instance.SendClientMessage));
    }
    
    public void OnPlayerConnected(long id, string name)
    {
        GameManager.Instance.SpawnPlayer((int)id, name);

        if (id == 1)
        {
            ClientManager.Instance.OnPlayerSpawned();
        }
        else
        {
            ClientManager.Instance.SendClientMessage(id, nameof(ClientManager.OnPlayerSpawned));    
        }
    }
    
    public void UpdateClientData(PlayerDataController data)
    {
        var dataBag = new Dictionary<string, Variant>()
        {
            {nameof(PlayerDataController.PlayerName), data.PlayerName},
            {nameof(PlayerDataController.SelectedSkin) , data.SelectedSkin},
            {nameof(PlayerDataController.SelectedWeapon), data.SelectedWeapon}
        };
        
        SendHostMessage("UpdatePlayerData", dataBag);
    }
    
    [Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
    private void UpdatePlayerData(Dictionary<string, Variant> data)
    {
        var id = Multiplayer.GetRemoteSenderId();

        GameManager.Instance.UpdatePlayerData(id, data);
    }
    
    public void SendHostMessage(string method, params Variant[] args)
    {
        RpcId(1, method, args);
    }
}