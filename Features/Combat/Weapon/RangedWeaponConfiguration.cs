using Godot;

public partial class RangedWeaponConfiguration : Node
{
    [Export] public PackedScene ProjectileModel;
    
    [Export] public float ProjectileSpeed = 10f;
}