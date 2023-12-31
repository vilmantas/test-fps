using System;
using Godot;

namespace testfps.Features.Multiplayer;

public partial class PlayerDataController : Node
{
    public int Id => int.Parse(Name);
    
    [Export] public string PlayerName = "Player";

    [Export] public string SelectedSkin = String.Empty;
    
    [Export] public string SelectedWeapon = String.Empty;

    [Export] public bool IsPlayer = true;
    
    [Export] public bool IsSpectator;

    [Export] public bool IsDungeonMaster;
    
    public void Initialize(string name)
    {
        PlayerName = name;
    }
}