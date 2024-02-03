using Godot;
using System;

public partial class WeaponConfiguration : Node
{
    [Export] public new string Name;
    
    [Export] public PackedScene Model;
    
    [Export] public int DamageMin;
    [Export] public int DamageMax;
    
    [Export] public float Cooldown = 0.5f;
}
