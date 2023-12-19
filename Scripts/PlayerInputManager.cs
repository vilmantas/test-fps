using System;
using System.Diagnostics;
using Godot;

namespace testfps.Scripts;

public partial class PlayerInputManager : Node
{
    public static PlayerInputManager Instance;
    
    public const string move_left = "player_move_left";
    public const string move_right = "player_move_right";
    public const string move_forward = "player_move_forward";
    public const string move_backward = "player_move_backward";
    
    public (int Horizontal, int Vertical) MovementInput = new (0, 0);
    
    public bool MovementInputEngaged => MovementInput.Horizontal != 0 || MovementInput.Vertical != 0;

    public bool RotationEnabled = false;
    
    public Action<bool> OnRotationEnabled;
    
    public override void _Ready()
    {
        Instance = this;
    }

    public override void _Process(double delta)
    {
        CheckRotation();
    }

    private void _input(InputEvent @event)
    {
        if (@event.IsActionPressed(move_forward))
        {
            MovementInput.Vertical--;
        }
        if (@event.IsActionPressed(move_backward))
        {
            MovementInput.Vertical++;
        }
        if (@event.IsActionPressed(move_left))
        {
            MovementInput.Horizontal--;
        }
        if (@event.IsActionPressed(move_right))
        {
            MovementInput.Horizontal++;
        }

        if (@event.IsActionReleased(move_forward))
        {
            MovementInput.Vertical++;
        }
        if (@event.IsActionReleased(move_backward))
        {
            MovementInput.Vertical--;
        }
		
        if (@event.IsActionReleased(move_left))
        {
            MovementInput.Horizontal++;
        }
        if (@event.IsActionReleased(move_right))
        {
            MovementInput.Horizontal--;
        }
    }
    
    private void CheckRotation()
    {
        if (Input.IsActionJustPressed("player_camera_rotate"))
        {
            EnableRotation();
        }

        if (Input.IsActionJustReleased("player_camera_rotate"))
        {
            DisableRotation();
        }
    }
    
    private void EnableRotation()
    {
        RotationEnabled = true;
        OnRotationChanged();
    }

    private void DisableRotation()
    {
        RotationEnabled = false;
        OnRotationChanged();
    }

    private void OnRotationChanged()
    {
        OnRotationEnabled?.Invoke(RotationEnabled);
    }
    
}