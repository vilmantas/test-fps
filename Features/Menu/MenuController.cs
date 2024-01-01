using Godot;
using System;
using testfps.Scripts;

public partial class MenuController : Node
{
	[Export] private PackedScene LobbyScene;
	
	private Button HostButton;
	private Button JoinButton;
	private Button SetNameButton;

	private LineEdit NameInput;
    
	public override void _Ready()
	{
		HostButton = GetNode<Button>("host");

		HostButton.Pressed += OnHostPressed;
		
		JoinButton = GetNode<Button>("join");
		
		JoinButton.Pressed += JoinButtonOnPressed;

		SetNameButton = GetNode<Button>("button_name");
		
		SetNameButton.Pressed += SetNameButtonOnPressed;
		
		NameInput = GetNode<LineEdit>("line_name");
		
		NameInput.Text = GameManager.PlayerName;
		
		ClientManager.Instance.OnJoined += OnJoined;
	}

	private void OnHostPressed()
	{
		MultiplayerManager.Instance.StartHost(GameManager.PlayerName);
	}
	
	private void JoinButtonOnPressed()
	{
		MultiplayerManager.Instance.ConnectToHost();
		
		JoinButton.Hide();
	}
	
	private void SetNameButtonOnPressed()
	{
		GameManager.Instance.SetPlayerName(NameInput.Text);
		
		SetNameButton.Hide();
	}
	
	private void OnJoined()
	{
		GetTree().ChangeSceneToPacked(LobbyScene);
	}
}
