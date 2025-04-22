using Godot;

namespace GodBrawl.Game.Actor;

/*
 * MultiplayerSynchronizer public_visibility needs to be true
 * if not, the visibility filter in ActorCompVisibility will not work as expected
 */
public partial class ActorCompMultiplayer : Node {
	[Export] public MultiplayerSynchronizer MultiplayerSynchronizer;
	[Export] public MultiplayerSynchronizer LocalPlayerSynchronizer;
	[Export] public ControllerLocalPlayer ControllerLocalPlayer;

	private int _playerPeerId;

	public int PlayerPeerId {
		get => _playerPeerId;
		set {
			_playerPeerId = value;

			LocalPlayerSynchronizer?.SetMultiplayerAuthority(_playerPeerId);
			ControllerLocalPlayer?.SetMultiplayerAuthority(_playerPeerId);
		}
	}
}