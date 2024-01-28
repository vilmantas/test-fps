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

        // KRW NESURANDA KAZKODEL TARGETO BLET
        
        var s = GameManager.CurrentGameplay.ContainerPlayers.FindChild(source) ?? GameManager.CurrentGameplay.ContainerEnemies.FindChild(source);

        var t = GameManager.CurrentGameplay.ContainerPlayers.FindChild(target) ?? GameManager.CurrentGameplay.ContainerEnemies.FindChild(target);

        var ss = s as Node3D;
        
       var tt = t as Node3D;
        
        Debug.WriteLine($"#2 {ss} attacked {tt}");
        
        DamageTrue(ss, tt);
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