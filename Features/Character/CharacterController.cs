using Godot;
using System;
using System.Diagnostics;

public partial class CharacterController : Node3D
{
	private MeshInstance3D m_CharacterMesh;

	private AnimationTree m_AnimationTree;
	
	public override void _Ready()
	{
		m_AnimationTree = GetNode<AnimationTree>("AnimationTree");
		
		m_CharacterMesh = GetNode<MeshInstance3D>("Armature/Skeleton3D/mesh");
	}

	public void SetPlayerAnimation(string animation)
	{
		m_AnimationTree.Set("parameters/Run/transition_request", animation);
	}

	public void SetMovementInput(Vector2 dir)
	{
		m_AnimationTree.Set("parameters/AimingRun/blend_position", dir.Normalized());
	}

	public void TriggerJump()
	{
		m_AnimationTree.Set("parameters/JumpOneShot/request",
			(int) AnimationNodeOneShot.OneShotRequest.Fire);
	}

	public void SetFloorDistance(float distance, float gravity)
	{
		m_AnimationTree.Set("parameters/JumpStateMachine/conditions/stopped", gravity < 0 && distance <= 0.7f);
	}
	
	public void SetModel(Mesh model)
	{
		m_CharacterMesh.Mesh = model;
	}

	public void SetAimingState(bool active)
	{
		m_AnimationTree.Set("parameters/AimingBlend/transition_request", active ? "aiming" : "not_aiming");
	}
}
