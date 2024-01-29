using System.Diagnostics;
using Godot;

namespace testfps.Features.Combat;

public partial class CombatManager : Node
{
    public static CombatManager Instance;

    public override void _Ready()
    {
        Instance = this;
    }
    
    public void Damage(Node3D source, Node3D target)
    {
        RpcId(1, nameof(DamageRPC), source.Name, target.Name);
    }
    
    [Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true)]
    private void DamageRPC(string sourceName, string targetName)
    {
        if (!Multiplayer.IsServer()) return;
        
        var sourceNode = GameManager.CurrentGameplay.FindPlayerOrEnemy(sourceName);

        var targetNode = GameManager.CurrentGameplay.FindPlayerOrEnemy(targetName);
        
        Debug.WriteLine($"#2 {sourceNode.Name} attacked {targetNode.Name}");
        
        DamageTrue(sourceNode, targetNode);
    }

    private void DamageTrue(Node3D source, Node3D target)
    {
        if (source == null || target == null) return;
        
        var health = target.GetNode<HealthController>("health_module");
        
        if (health == null) return;
        
        health.Damage(3);

        if (health.IsExpended)
        {
            if (target is EnemyController enemy)
            {
                GameManager.CurrentGameplay.OnEnemyDeath(enemy, source);
            }
            else if (target is PlayerController player)
            {
                GameManager.CurrentGameplay.OnPlayerDeath(player);
            }
        }
    }
}