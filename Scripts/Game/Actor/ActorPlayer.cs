using System.Linq;
using Godot;

namespace GodBrawl.Game.Actor;

public partial class ActorPlayer : ActorBase {
	[Export] private Camera3D _camera;
	[Export] private ActorAttackIndicator _attackIndicator;
	
	public LocalPlayer LocalPlayer;

	public bool IsClientOwner => LocalPlayer != null;

	public override int PlayerPeerId {
		get => _playerPeerId;
		set {
			_playerPeerId = value;
			
			AssignLocalPlayer();
		}
	}

	public override void _EnterTree() {
		ControllerMultiplayer.Instance.ActorPlayers.Add(this);
	}
	
	public override void _ExitTree() {
		ControllerMultiplayer.Instance.ActorPlayers.Remove(this);
	}

	public override void _Ready() {
		if (IsClientOwner) {
			_camera.Current = true;
		}
	}

	public void AssignLocalPlayer() {
		if (LocalPlayer != null) {
			return;
		}
		
		var controlledPlayer = ControllerMultiplayer.Instance.LocalPlayers
			.FirstOrDefault(localPlayer => localPlayer.PeerId == _playerPeerId);
		
		if (controlledPlayer == null) {
			return;
		}
		
		LocalPlayer = controlledPlayer;
		controlledPlayer.ActorPlayer = this;
	}
	
	public void UpdateAttackIndicator(Vector2 aimDirection) {
		if (aimDirection.IsZeroApprox()) {
			_attackIndicator.Active = false;
		}
		else {
			_attackIndicator.Active = true;
			_attackIndicator.HandleInput(aimDirection);
		}
	}
}