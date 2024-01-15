using Godot;
using System.Linq;

public partial class EnemyTestScene : Node3D
{
    private bool IsHidden = false;
    private Node3D Player;

    public override void _Ready()
    {
        Player = GetNode<Node3D>("target_player");
    }

    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("player_jump"))
        {
            var spawns = GetTree().GetNodesInGroup("spawn");

            var casted = spawns.Cast<Node3D>().ToList();

            var rng = new RandomNumberGenerator();

            var index = rng.RandiRange(0, casted.Count - 1);
            
            var spawn = casted[index];

            var player = GetNode<Node3D>("target_player");
            
            player.GlobalTransform = spawn.GlobalTransform;
        }

        if (Input.IsActionJustPressed("player_attack"))
        {
            
            
            if (IsHidden)
            {
                GetTree().CurrentScene.AddChild(Player);
                IsHidden = false;
            }
            else
            {
                GetTree().CurrentScene.RemoveChild(Player);
                IsHidden = true;
            }
        }
    }
}
