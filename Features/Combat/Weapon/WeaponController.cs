using Godot;
using System;

public partial class WeaponController : Node3D
{
    [Export] public WeaponConfiguration WeaponConfiguration;
    
    [Export] public RangedWeaponConfiguration RangedWeaponConfiguration;
    
    [Export] public MeleeWeaponConfiguration MeleeWeaponConfiguration;
    
    public override void _Ready()
    {
        var weaponModel = WeaponConfiguration.Model.Instantiate();
        
        AddChild(weaponModel);
    }
}
