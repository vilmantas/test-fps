using Godot;
using System;

public partial class NPCController : CharacterBody3D
{
    [Export] public float Speed;

    [Export] public float TurnSpeed;
    
    [Export] public float StoppingDistance = 1f;
    
    private Vector3? Destination;

    public override void _PhysicsProcess(double delta)
    {
        if (Destination == null) return;
        
        if (GlobalTransform.Origin.DistanceTo(Destination.Value) <= StoppingDistance)
        {
            Destination = null;
            return;
        }

        var direction = Destination.Value - GlobalTransform.Origin;
        
        var velocity = direction.Normalized() * Speed;

        Velocity = velocity;
        
        var angle = new Vector2(velocity.Z, velocity.X).Angle();	
        
        var m_Rotation = Rotation;
		
        m_Rotation.Y = (float)Mathf.LerpAngle(m_Rotation.Y, angle - Math.PI, delta * TurnSpeed);
        
        Rotation = m_Rotation;
        
        MoveAndSlide();
    }
}
