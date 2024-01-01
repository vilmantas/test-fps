using Godot;
using System;

public partial class WeaponConfiguration : Node
{
    [Export] public string Name;
    [Export] public PackedScene WeaponModel;
    [Export] public float AttackCooldown = 0.5f;
    [Export] public HitboxConfiguration HitboxConfiguration;
}
