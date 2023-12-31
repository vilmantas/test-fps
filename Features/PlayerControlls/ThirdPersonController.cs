using Godot;
using System;
using testfps.Scripts;

public class ThirdPersonController
{
    private float JumpVelocity;
    private float JumpGravity;
    private float FallGravity;
    
    private Vector3 m_Velocity;
    private Vector2 m_MovementInput;
    private Vector3 m_Rotation;
    private bool m_JumpEngaged;

    private readonly float m_Speed;
    private readonly float m_LookSpeed;

    public ThirdPersonController(float Speed, float LookSpeed, float JumpHeight, float JumpTimeToPeak, float JumpTimeToDescend)
    {
        m_Speed = Speed;
        m_LookSpeed = LookSpeed;
        
        JumpVelocity = 2 * JumpHeight / JumpTimeToPeak;
        JumpGravity = (-2 * JumpHeight) / Mathf.Pow(JumpTimeToPeak, 2);
        FallGravity = (-2 * JumpHeight) / Mathf.Pow(JumpTimeToDescend, 2);
    }
    
    public MovementResult GetVelocity(double delta, Vector3 CurrentVelocity, Vector3 CurrentRotation, bool JumpQueued)
    {
        var cameraBasis = GameManager.CameraController.GlobalTransform.Basis;

        m_JumpEngaged = false;
        
        m_Velocity = Vector3.Zero;

        m_MovementInput = new Vector2(PlayerInputManager.Instance.MovementInput.Horizontal,
            PlayerInputManager.Instance.MovementInput.Vertical).Normalized();
        
        if (PlayerInputManager.Instance.MovementInputEngaged)
        {
            m_Velocity = cameraBasis.Z * m_MovementInput.Y;
            m_Velocity += cameraBasis.X * m_MovementInput.X;

            m_Velocity = m_Velocity.Normalized();
			
            float angle;
			
            if (PlayerInputManager.Instance.RotationEnabled)
            {
                var vect = -cameraBasis.Z;
				
                angle = new Vector2(vect.Z, vect.X).Angle();
            }
            else
            {
                angle = new Vector2(m_Velocity.Z, m_Velocity.X).Angle();	
            }

            m_Rotation = CurrentRotation;
		
            m_Rotation.Y = (float)Mathf.LerpAngle(m_Rotation.Y, angle - Math.PI, delta * m_LookSpeed);
        }

        m_Velocity.Y = CurrentVelocity.Y;
		
        m_Velocity.Y += GetGravity(CurrentVelocity) * (float)delta;

        if (JumpQueued)
        {
            m_Velocity.Y = JumpVelocity;
            m_JumpEngaged = true;
        }
        
        m_Velocity.X *= m_Speed;
        m_Velocity.Z *= m_Speed;

        return new MovementResult()
        {
            JumpEngaged = m_JumpEngaged,
            MovementInput = m_MovementInput,
            Rotation = m_Rotation,
            Velocity = m_Velocity,
        };
    }
    
    public float GetGravity(Vector3 CurrentVelocity)
    {
        return CurrentVelocity.Y > 0 ? JumpGravity : FallGravity;
    }

    public class MovementResult
    {
        public Vector3 Velocity { get; set; }
        public Vector3 Rotation { get; set; }
        public Vector2 MovementInput { get; set; }
        public bool JumpEngaged { get; set; }
    }
}
