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
}
