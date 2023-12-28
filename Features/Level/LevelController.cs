using System.Diagnostics;
using Godot;
using testfps.Scripts;

namespace testfps.Features.Level;

public partial class LevelController : Node
{
	public override void _Ready()
	{
		GameManager.Instance.OnLevelLoaded(this);
	}

	public void _on_join_pressed()
	{
		MultiplayerManager.Instance.ConnectToHost();
	}
	
	public void _on_host_pressed()
	{
		MultiplayerManager.Instance.StartHost();
	}
}
