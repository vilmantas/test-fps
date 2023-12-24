using Godot;

namespace testfps.DebugScripts.Node;

public partial class PositionDebug : Node3D
{
    public override void _Process(double delta)
    {
        if (GameManager.PlayerController == null) return;
        if (GameManager.CameraController == null) return;

        Position = GameManager.CameraController.GlobalPosition + (GameManager.CameraController.GlobalTransform.Basis.Z * -5);
        Position = new Vector3(Position.X, 0, Position.Z);
    }
}