using GodBrawl.Config;
using Godot;

namespace GodBrawl.Game.UserInterface;

public partial class ComponentServer : Control {
	[Export] private StartConfig _startConfig;
	[Export] private PackedScene _battleScene;
	
	private void OnPortTextChanged(string text) {
		var success = int.TryParse(text, out var port);

		if (!success) {
			return;
		}
		
		_startConfig.Port = port;
	}
	
	private void OnConfirmButtonPressed() {
		_startConfig.IsServer = true;

		GetTree().ChangeSceneToPacked(_battleScene);
	}
}