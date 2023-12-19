using System.Diagnostics;
using Godot;

namespace testfps.Features.Level;

public partial class LevelController : Node
{
	public override void _Ready()
	{
		Debug.Print(GetChildren().Count.ToString());
		
		GameManager.Instance.OnLevelLoaded(this);
	}
}
