using Godot;
using System;
using System.Diagnostics;
using testfps.Scripts;

public partial class CameraFreeLookController : Node3D
{
    [Export]
    public Camera3D Camera;

    [Export] public float Speed = 5f;

    [Export] public float SpeedRunMultiplier = 2f;
    
    [Export] public float MouseSensitivity = 3f;

    private Vector2 Offset = Vector2.Zero;
    
    private bool RotationEnabled;

    private float CurrentSpeed;

    public void Initialize(bool enabled)
    {
        SetProcess(enabled);
        SetProcessInput(enabled);
        SetPhysicsProcess(enabled);
    }

    public override void _Ready()
    {
        CurrentSpeed = Speed;
        
        PlayerInputManager.Instance.OnRotationEnabled += OnRotationChanged;
        PlayerInputManager.Instance.OnRunToggled += OnRunToggled;
    }

    private void OnRunToggled(bool obj)
    {
        CurrentSpeed = obj ? CurrentSpeed * SpeedRunMultiplier : Speed;
    }

    private void OnRotationChanged(bool obj)
    {
        if (obj)
        {
            EnableRotation();
        }
        else
        {
            DisableRotation();
        }
    }

    private void DisableRotation()
    {
        var screenCenter = GetViewport().GetVisibleRect().Size / 2;
        
        GetViewport().WarpMouse(screenCenter);
        
        RotationEnabled = false;
        Input.MouseMode = Input.MouseModeEnum.Visible;
    }

    private void EnableRotation()
    {
        RotationEnabled = true;
        Input.MouseMode = Input.MouseModeEnum.Captured;
    }

    public override void _PhysicsProcess(double delta)
    {
        RotateMouse(delta);
        
        Offset = Vector2.Zero;
        
        var input = PlayerInputManager.Instance.MovementInput;
        
        var inputVector = new Vector3(input.Horizontal, 0, input.Vertical);
        
        if (Input.IsActionPressed("player_jump"))
        {
            inputVector.Y += 1;
        }
        
        if (Input.IsActionPressed("player_crouch"))
        {
            inputVector.Y -= 1;
        }

        GlobalPosition += GlobalTransform.Basis * inputVector * (float) delta * CurrentSpeed;
    }

    public override void _Input(InputEvent @event)
    {
        if (!RotationEnabled) return;
        
        if (@event is InputEventMouseMotion mouseMotion)
        {
            Offset = mouseMotion.Relative;
        }
    }

    private void RotateMouse(double delta)
    {
        if (!RotationEnabled) return;
        
        var x = -Offset.X;
        var y = -Offset.Y;
        
        Camera.RotateX(Mathf.DegToRad(y * (float)delta * MouseSensitivity));
        RotateY(Mathf.DegToRad(x * (float)delta * MouseSensitivity));
    }
}
