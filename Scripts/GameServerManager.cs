using Godot;
using Godot.Collections;

namespace testfps.Scripts;

public partial class GameServerManager : Node
{
    public static GameServerManager Instance;

    public override void _Ready()
    {
        Instance = this;
    }
}