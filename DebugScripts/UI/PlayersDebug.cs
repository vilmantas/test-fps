using Godot;

namespace testfps.DebugScripts.UI;

public partial class PlayersDebug : Label
{
    public override void _Process(double delta)
    {
        if (GameManager.CameraController == null) return;

        Text = "";
        
        foreach (var data in GameManager.ConnectedPlayers)
        {
            Text += $"{data.Key} : {data.Value}\n";
        }
    }
}