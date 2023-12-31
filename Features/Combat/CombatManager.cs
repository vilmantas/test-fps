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
        var health = target.GetNode<HealthController>("health_module");
        
        if (health == null) return;
        
        health.Damage(3);
    }
}