using Godot;
using System;

public partial class EnemyTargetTest : Node3D
{
    [Export] Label3D HealthLabel;

    [Export] HealthController Health;

    public override void _Process(double delta)
    {
        HealthLabel.GlobalBasis = GetViewport().GetCamera3D().GlobalTransform.Basis;
        
        HealthLabel.Text = $"{Health.CurrentHealth}/{Health.MaxHealth}";
    }
}
