using Godot;
using System;

public partial class WeaponController : Node3D
{
    [Export] public WeaponConfiguration WeaponConfiguration;
    [Export] public HitboxConfiguration HitboxConfiguration;

    public override void _Ready()
    {
        var weaponModel = WeaponConfiguration.WeaponModel.Instantiate();
        AddChild(weaponModel);
    }
}
