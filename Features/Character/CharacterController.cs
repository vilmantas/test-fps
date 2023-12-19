using Godot;
using System;

public partial class CharacterController : Node3D
{
	[Export] public Mesh Mesh;
	
	private MeshInstance3D m_CharacterMesh;

	private AnimationPlayer m_AnimationPlayer;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		m_AnimationPlayer = GetNode<AnimationPlayer>("animation_player");
		
		m_CharacterMesh = GetNode<MeshInstance3D>("Armature/Skeleton3D/mesh");

		m_CharacterMesh.Mesh = Mesh;
	}

	public void PlayAnimation(string animation)
	{
		m_AnimationPlayer.Play(animation);
	}
	
}
