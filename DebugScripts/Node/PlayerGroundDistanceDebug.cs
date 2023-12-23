using Godot;

namespace testfps.DebugScripts.Node;

public partial class PlayerGroundDistanceDebug : Label
{
    public override void _Process(double delta)
    {
        var player = GameManager.PlayerController;

        var ray = player.GroundDistanceRay;
        
        var groundDistance = 5f;

        if (player.GroundDistanceRay.IsColliding())
        {
            var point = ray.GetCollisionPoint();

            groundDistance = player.GlobalPosition.DistanceTo(point);
        }

        Text = $"{groundDistance}; {player.GetGravity()}";
    }
}