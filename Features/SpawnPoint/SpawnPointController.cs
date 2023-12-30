using Godot;

namespace testfps.Features.SpawnPoint;

public partial class SpawnPointController : Node3D
{
    public Node3D SpawnLocation;
    
    public override void _Ready()
    {
        SpawnLocation = GetNode<Node3D>("location_spawn");
    }
}