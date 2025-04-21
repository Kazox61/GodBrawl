using GodBrawl.Scripts.Game;
using Godot;

namespace GodBrawl.Game.Actor;

public partial class LocalPlayer : Node {
	[Export] public MultiplayerSynchronizer LocalMultiplayerSynchronizer;
	[Export] public ControllerInput ControllerInput;
	
	public ActorPlayer ActorPlayer;
	
	private int _peerId;
	public int PeerId {
		get => _peerId;
		set {
			_peerId = value;
			
			ControllerInput.SetMultiplayerAuthority(_peerId);
			LocalMultiplayerSynchronizer.SetMultiplayerAuthority(_peerId);
		}
	}

	public override void _EnterTree() {
		ControllerMultiplayer.Instance.LocalPlayers.Add(this);
	}

	public override void _Ready() {
		if (ControllerInput.IsMultiplayerAuthority()) {
			ControllerMultiplayer.Instance.ControlledLocalPlayer = this;
		}
	}

	public override void _ExitTree() {
		ControllerMultiplayer.Instance.LocalPlayers.Remove(this);
	}
}