using Godot;

namespace GodBrawl.Game.Actor;

public partial class ActorPlayer : ActorBase {
	[Export] public MultiplayerSynchronizer LocalPlayerSynchronizer;
	[Export] public ControllerLocalPlayer ControllerLocalPlayer;

	private int _playerPeerId;

	public int PlayerPeerId {
		get => _playerPeerId;
		set {
			_playerPeerId = value;

			LocalPlayerSynchronizer.SetMultiplayerAuthority(_playerPeerId);
			ControllerLocalPlayer.SetMultiplayerAuthority(_playerPeerId);
		}
	}
}