using System.Diagnostics;
using Godot;

namespace testfps.Scripts;

public partial class MultiplayerManager : Node
{
    public static MultiplayerManager Instance;
    
    public static int DEFAULT_PORT = 8007;
    public static string DEFAULT_IP = "127.0.0.1";
    
    public override void _Ready()
    {
        Instance = this;
        
        Multiplayer.ConnectedToServer += OnConnectedToServer;
        Multiplayer.PeerConnected += OnPeerConnected;
        Multiplayer.PeerDisconnected += OnPeerDisconnected;
        Multiplayer.ConnectionFailed += OnConnectionFailed;
    }

    public void StartHost(string name)
    {
        var peer = new ENetMultiplayerPeer();
        peer.CreateServer(DEFAULT_PORT, 4);

        Multiplayer.MultiplayerPeer = peer;

        ClientManager.Instance.ClientId = Multiplayer.GetUniqueId();
        
        GameServerManager.Instance.OnPlayerConnected(Multiplayer.GetUniqueId(), name);
    }

    public void ConnectToHost()
    {
        var peer = new ENetMultiplayerPeer();
        
        peer.CreateClient(DEFAULT_IP, DEFAULT_PORT);

        Multiplayer.MultiplayerPeer = peer;
    }
    
    private void OnConnectionFailed()
    {
        throw new System.NotImplementedException();
    }

    private void OnPeerDisconnected(long id)
    {
        throw new System.NotImplementedException();
    }

    private void OnPeerConnected(long id)
    {
        if (!Multiplayer.IsServer()) return;
        
        var name = $"Player {GameManager.Core.Players.Length + 1}";
         
        GameServerManager.Instance.OnPlayerConnected(id, name);
    }

    private void OnConnectedToServer()
    {
        ClientManager.Instance.OnJoin(Multiplayer.GetUniqueId());
    }
}