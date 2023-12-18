using Godot;

namespace testfps.DebugScripts.UI;

public partial class CameraForwardDebug : Label
{
    public override void _Process(double delta)
    {
        if (GameManager.CameraController == null) return;

        Text = GameManager.CameraController.Transform.Basis.Z.ToString();
    }
}