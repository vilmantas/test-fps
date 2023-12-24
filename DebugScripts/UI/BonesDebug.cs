using Godot;

namespace testfps.DebugScripts.Node;

public partial class BonesDebug : Label
{
    private bool Spawned = false;

    [Export] private PackedScene ItemPrefab;

    private Node3D LeftArmSlot;
    
    public override void _Process(double delta)
    {
        if (GameManager.PlayerController == null) return;
        if (GameManager.CameraController == null) return;

        var c = GameManager.PlayerController.Character;

        LeftArmSlot = c.SlotLeftArm;

        var skelly = GameManager.PlayerController.Character.m_Skeleton3D;

        Text = $"{skelly.GetBoneCount()}:{skelly.FindBone("LeftHand")}:{c.SlotLeftArm.GlobalPosition}";
    }

    public override void _Input(InputEvent @event)
    {
        if (Spawned) return;
        
        if (Input.IsKeyPressed(Key.E))
        {
            Spawned = true;

            var item = ItemPrefab.Instantiate<Node3D>();

            item.Scale = Vector3.One * 10;
            
            LeftArmSlot.AddChild(item);
        }
    }
}