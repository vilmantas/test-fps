using System;
using Godot;

namespace testfps.Features.Multiplayer;

public partial class PlayerDataController : Node
{
    public int Id => int.Parse(Name);
    
    [Export] public string PlayerName = "Player";

    [Export] public string SelectedSkin = String.Empty;
    
    public void Initialize(string name)
    {
        PlayerName = name;
    }
}