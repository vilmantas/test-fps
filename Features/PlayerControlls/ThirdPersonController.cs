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
    
    public MovementResult GetVelocity(ThirdPersonControllerArgs args)
    {
        var cameraBasis = GameManager.CameraController.GlobalTransform.Basis;

        m_JumpEngaged = false;
        
        m_Velocity = Vector3.Zero;

        m_MovementInput = new Vector2(
            PlayerInputManager.Instance.MovementInput.Horizontal,
            PlayerInputManager.Instance.MovementInput.Vertical
            );
        
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

            m_Rotation = args.CurrentRotation;
		
            m_Rotation.Y = (float)Mathf.LerpAngle(m_Rotation.Y, angle - Math.PI, args.Delta * m_LookSpeed);
        }

        m_Velocity.Y = args.CurrentVelocity.Y;
		
        m_Velocity.Y += GetGravity(args.CurrentVelocity) * (float)args.Delta;

        if (args.JumpQueued)
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
            LerpedMovementInput = LerpWithLimit(args.CurrentMovementInput, m_MovementInput, (float)args.Delta * m_LookSpeed)
        };
    }
    
    public float GetGravity(Vector3 CurrentVelocity)
    {
        return CurrentVelocity.Y > 0 ? JumpGravity : FallGravity;
    }

    private Vector2 LerpWithLimit(Vector2 from, Vector2 to, float by)
    {
        var result = from.Lerp(to, by);;
        
        if (m_MovementInput == Vector2.Zero)
        {
            if (Math.Abs(result.X) < 0.001 && Math.Abs(result.Y) < 0.001)
            {
                result = Vector2.Zero;
            }
        }

        return result;
    }

    public class ThirdPersonControllerArgs
    {
        public double Delta { get; set; }
        public Vector3 CurrentVelocity { get; set; }
        public Vector3 CurrentRotation { get; set; }
        public bool JumpQueued { get; set; }
        public Vector2 CurrentMovementInput { get; set; }
        public float Speed { get; set; }
        public float LookSpeed { get; set; }
    }
    
    public class MovementResult
    {
        public Vector3 Velocity { get; set; }
        public Vector3 Rotation { get; set; }
        public bool JumpEngaged { get; set; }
        public Vector2 MovementInput { get; set; }
        public Vector2 LerpedMovementInput { get; set; }
    }
}
