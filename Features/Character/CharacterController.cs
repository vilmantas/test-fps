using Godot;
using System;
using System.Diagnostics;

public partial class CharacterController : Node3D
{
	private MeshInstance3D m_CharacterMesh;

	private AnimationTree m_AnimationTree;

	public Skeleton3D m_Skeleton3D;

	public Node3D SlotLeftArm;

	public Node3D SlotRightArm;
	
	public override void _Ready()
	{
		m_AnimationTree = GetNode<AnimationTree>("animation_tree");
		
		m_Skeleton3D = GetNode<Skeleton3D>("Armature/Skeleton3D");
		
		m_CharacterMesh = GetNode<MeshInstance3D>("Armature/Skeleton3D/mesh");

		SlotLeftArm = m_Skeleton3D.FindChild("slot_left_arm") as Node3D;
		
		SlotRightArm = m_Skeleton3D.FindChild("slot_right_arm") as Node3D;
	}

	public void SetPlayerAnimation(string animation)
	{
		m_AnimationTree.Set("parameters/Run/transition_request", animation);
	}

	public void SetMovementInput(Vector2 dir)
	{
		m_AnimationTree.Set("parameters/AimingRun/blend_position", dir);
	}

	public void TriggerJump()
	{
		m_AnimationTree.Set("parameters/JumpOneShot/request",
			(int) AnimationNodeOneShot.OneShotRequest.Fire);
	}
    
	public void TriggerAttack()
	{
		m_AnimationTree.Set("parameters/AttackOneShot/request",
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
	
	public void SetMainWeapon(Node3D item)
	{
		SlotRightArm.AddChild(item);
		
		item.Scale = Vector3.One * 100;
	}
}
