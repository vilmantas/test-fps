using Godot;
using System;
using System.Diagnostics;
using System.Linq;

public partial class DungeonMasterController : Node3D
{
    [Export] public CameraFreeLookController CameraFreeLook;

    [Export] public PackedScene DebugScene;
    
    [Export] public PackedScene EnemyPrefab;
    
    private RayCast3D RayCast;

    private Node3D DebugInstance;
    
    public override void _Ready()
    {
        DebugInstance = DebugScene.Instantiate<Node3D>();
        
        AddChild(DebugInstance); ;

        CallDeferred("ReparentCamera");
    }

    private void ReparentCamera()
    {
        CameraFreeLook.GlobalRotation = Vector3.Zero;
        CameraFreeLook.Reparent(GetParent());
    }


    public override void _PhysicsProcess(double delta)
    {
        var camera3D = CameraFreeLook.Camera;
        var from = camera3D.ProjectRayOrigin(GetViewport().GetMousePosition());
        var to = from + camera3D.ProjectRayNormal(GetViewport().GetMousePosition()) * 10f;

        var result = GetWorld3D().DirectSpaceState.IntersectRay(new PhysicsRayQueryParameters3D() { From = from, To = to, CollisionMask = 1 << 0 });

        if (result.Count <= 0) return;
        
        var sb = result["collider"].As<StaticBody3D>();

        if (sb == null) return;
            
        var normal = result["normal"].As<Vector3>();
            
        DebugInstance.GlobalPosition = result["position"].As<Vector3>();

        var x = DebugInstance.Basis;

        x.Y = normal;
        x.X = -DebugInstance.Basis.Z.Cross(normal);

        DebugInstance.GlobalRotation = normal;
        
        if (Input.IsActionJustPressed("player_attack"))
        {
            var enemy = EnemyPrefab.Instantiate<EnemyController>();

            enemy.GlobalTransform = DebugInstance.GlobalTransform;

            GetParent().AddChild(enemy);
        }
    }
}
