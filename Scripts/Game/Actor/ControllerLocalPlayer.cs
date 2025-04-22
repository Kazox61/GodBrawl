using GodBrawl.Extensions;
using GodBrawl.Game.UserInterface;
using Godot;

namespace GodBrawl.Game.Actor;

public partial class ControllerLocalPlayer : Node3D {
	[Export] private ActorPlayer _actorPlayer;
	[Export] private Camera3D _camera;
	[Export] private ActorAttackIndicator _attackIndicator;
	
	[Export] private PackedScene _canvasLocalPlayerPrefab;
	
	public Vector2 MoveDirection;
	public Vector2 AimDirection;

	public override void _Ready() {
		if (!IsMultiplayerAuthority() && !Multiplayer.IsServer()) {
			_actorPlayer.CompMultiplayer.LocalPlayerSynchronizer.Remove();
		}
		/*
		 * With this return early, physics process is only called when it has authority.
		 * This is set in ActorPlayer PlayerPeerId property.
		 */
		if (Multiplayer.MultiplayerPeer == null || !IsMultiplayerAuthority()) {
			return;
		}
		
		_camera.Current = true;
		
		var canvasLocalPlayer = _canvasLocalPlayerPrefab.Instantiate<CanvasLocalPlayer>();
		GetTree().Root.AddChild(canvasLocalPlayer);
		
		canvasLocalPlayer.AimJoystick.JoystickReleased += OnAimJoystickReleased;
	}

	public override void _PhysicsProcess(double delta) {
		// This is run only on the server to update the MoveDirection of the ActorPlayer.
		if (Multiplayer.MultiplayerPeer != null && Multiplayer.IsServer()) {
			_actorPlayer.MoveDirection = MoveDirection.Normalized();
		}
		
		/*
		 * With this return early, physics process is only called when it has authority.
		 * This is set in ActorPlayer PlayerPeerId property.
		 */
		if (Multiplayer.MultiplayerPeer == null || !IsMultiplayerAuthority()) {
			return;
		}
		
		MoveDirection = Input.GetVector("move_left", "move_right", "move_up", "move_down");
		AimDirection = Input.GetVector("aim_left", "aim_right", "aim_up", "aim_down");
		if (AimDirection.IsZeroApprox()) {
			_attackIndicator.Active = false;
		}
		else {
			_attackIndicator.Active = true;
			_attackIndicator.HandleInput(AimDirection);
		}
	}
	
	private void OnAimJoystickReleased(Vector2 inputDirection) {
		RpcId(1, MethodName.Shoot, inputDirection);
	}

	[Rpc(CallLocal = false)]
	private void Shoot(Vector2 direction) {
		GD.Print($"Shoot with direction: {direction}");
	}
}