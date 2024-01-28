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
        if (Multiplayer.IsServer())
        {
            DamageTrue(source, target);
        }
        else
        {
            RpcId(1, nameof(DamagePrivate), source.Name, target.Name);
        }
    }
    
    [Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = false)]
    private void DamagePrivate(string source, string target)
    {
        Debug.WriteLine($"#1 {source} attacked {target}");

        var s = GameManager.CurrentGameplay.ContainerPlayers.GetNode<Node3D>(source) ?? GameManager.CurrentGameplay.ContainerEnemies.GetNode<Node3D>(source);

        var t = GameManager.CurrentGameplay.ContainerPlayers.GetNode<Node3D>(target) ?? GameManager.CurrentGameplay.ContainerEnemies.GetNode<Node3D>(target);
        
        Debug.WriteLine($"#2 {t} attacked {t}");
        
        DamageTrue(t, t);
    }

    private void DamageTrue(Node3D source, Node3D target)
    {
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