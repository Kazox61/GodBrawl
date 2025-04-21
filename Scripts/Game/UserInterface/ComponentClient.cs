using GodBrawl.Config;
using Godot;

namespace GodBrawl.Game.UserInterface;

public partial class ComponentClient : Control {
	[Export] private StartConfig _startConfig;
	[Export] private PackedScene _battleScene;
	
	private void OnIpTextChanged(string text) {
		_startConfig.Ip = text;
	}
	
	private void OnPortTextChanged(string text) {
		var success = int.TryParse(text, out var port);

		if (!success) {
			return;
		}
		
		_startConfig.Port = port;
	}

	private void OnBrawlerToggled(bool toggledOn, int id) {
		if (!toggledOn) {
			return;
		}
		
		_startConfig.BrawlerId = id;
	}
	
	private void OnConfirmButtonPressed() {
		_startConfig.IsServer = false;

		GetTree().ChangeSceneToPacked(_battleScene);
	}
}