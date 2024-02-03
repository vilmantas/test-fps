using System.Diagnostics;
using Godot;
using System.Linq;

public partial class EnemyTestScene : Node3D
{
    private bool IsHidden = false;
    private Node3D Player;

    private PlayerController ActualPlayer;
    
    public override void _Ready()
    {
        Player = GetNode<Node3D>("entity_containers/players/target_player");

        ActualPlayer = GetNode<PlayerController>("entity_containers/players/player");
        
        GameManager.Instance.OnLevelLoaded(this);
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

            Player.GlobalTransform = spawn.GlobalTransform;
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
