using Godot;
using System;

public partial class HitboxConfiguration : Node
{
    [Export] public PackedScene HitboxModel;
    [Export] public float Delay = 0.3f;
    [Export] public float Duration = 0.3f;
}
