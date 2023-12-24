using Godot;

namespace testfps.DebugScripts.Node;

public partial class PlayerAttackDebug : Godot.Node
{
    public override void _Process(double delta)
    {
        if (GameManager.PlayerController == null) return;
        if (GameManager.CameraController == null) return;
        
        
    }

    public override void _Input(InputEvent @event)
    {
        if (Input.IsKeyPressed(Key.Q))
        {
            var ch = GameManager.PlayerController;

            var e = ch.AttackHitbox.Instantiate<Node3D>();
            
            ch.AttackHitboxSpawn.AddChild(e);
        }
    }
}