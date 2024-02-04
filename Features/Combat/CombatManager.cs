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
    
    public void Damage(Node3D source, Node3D target, int damage)
    {
        RpcId(1, nameof(DamageRPC), source.Name, target.Name, damage);
    }
    
    [Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true)]
    private void DamageRPC(string sourceName, string targetName, int damage)
    {
        if (!Multiplayer.IsServer()) return;
        
        var sourceNode = GameManager.CurrentGameplay.FindPlayerOrEnemy(sourceName);

        var targetNode = GameManager.CurrentGameplay.FindPlayerOrEnemy(targetName);
        
        Debug.WriteLine($"#2 {sourceNode.Name} attacked {targetNode.Name}");
        
        DamageTrue(sourceNode, targetNode, damage);
    }

    private void DamageTrue(Node3D source, Node3D target, int damage)
    {
        if (source == null || target == null) return;
        
        var health = target.GetNode<HealthController>("health_module");
        
        if (health == null) return;
        
        health.Damage(damage);

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