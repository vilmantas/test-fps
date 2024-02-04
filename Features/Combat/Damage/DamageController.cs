using Godot;

public partial class DamageController : Node
{
    public int CalculateWeaponDamage(WeaponController weapon)
    {
        return weapon.WeaponConfiguration.DamageMin + GD.RandRange(0,
            weapon.WeaponConfiguration.DamageMax - weapon.WeaponConfiguration.DamageMin);
    }
} 