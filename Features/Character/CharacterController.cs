using Godot;
using System;

public partial class CharacterController : Node3D
{
	private MeshInstance3D m_CharacterMesh;

	private AnimationPlayer m_AnimationPlayer;

	private AnimationTree m_AnimationTree;
	
	public override void _Ready()
	{
		m_AnimationPlayer = GetNode<AnimationPlayer>("animation_player");

		m_AnimationTree = GetNode<AnimationTree>("AnimationTree");
		
		m_CharacterMesh = GetNode<MeshInstance3D>("Armature/Skeleton3D/mesh");
	}

	public void PlayAnimation(string animation)
	{
		m_AnimationTree.Set("parameters/Transition/transition_request", animation);
	}

	public void SetModel(Mesh model)
	{
		m_CharacterMesh.Mesh = model;
	}
}
