using Godot;

namespace GodBrawl.Game.Actor;

public partial class ControllerLocalPlayer : Node3D {
	[Export] private ActorPlayer _actorPlayer;
	[Export] private Camera3D _camera;
	
	public Vector2 MoveDirection;

	public override void _Ready() {
		if (!IsMultiplayerAuthority()) {
			return;
		}
		
		_camera.Current = true;
		ControllerMultiplayer.Instance.AimJoystick.JoystickReleased += OnAimJoystickReleased;
	}

	public override void _PhysicsProcess(double delta) {
		// This is run only on the server to update the MoveDirection of the ActorPlayer.
		if (Multiplayer.MultiplayerPeer != null && Multiplayer.IsServer()) {
			_actorPlayer.MoveDirection = MoveDirection;
		}
		
		/*
		 * With this return early, physics process is only called when it has authority.
		 * This is set in ActorPlayer PlayerPeerId property.
		 */
		if (Multiplayer.MultiplayerPeer == null || !IsMultiplayerAuthority()) {
			return;
		}
		
		MoveDirection = Input.GetVector("move_left", "move_right", "move_up", "move_down");
	}
	
	private void OnAimJoystickReleased(Vector2 inputDirection) {
		RpcId(1, MethodName.Shoot, inputDirection);
	}

	[Rpc(CallLocal = false)]
	private void Shoot(Vector2 direction) {
		GD.Print($"Shoot with direction: {direction}");
	}
}