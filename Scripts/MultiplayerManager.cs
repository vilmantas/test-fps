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

    public void StartHost()
    {
        var peer = new ENetMultiplayerPeer();
        peer.CreateServer(DEFAULT_PORT, 4);

        Multiplayer.MultiplayerPeer = peer;
        
        GameManager.Instance.OnPlayerConnected(Multiplayer.GetUniqueId(), "Player 1");
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
        Debug.Print($"Connected to peer {id}");
        Debug.Print($"{Multiplayer.IsServer()}:{Multiplayer.GetUniqueId()}");
        
        GameManager.Instance.OnPlayerConnected(id, $"Player {id}");
    }

    private void OnConnectedToServer()
    {
        Debug.Print("Connected to server");
        Debug.Print($"{Multiplayer.IsServer()}:{Multiplayer.GetUniqueId()}");
        
        GameManager.Instance.OnPlayerConnected(Multiplayer.GetUniqueId(), "Player 1");
        GameManager.Instance.OnPlayerConnected(Multiplayer.GetUniqueId(), $"Player {Multiplayer.GetUniqueId()}");
    }
}