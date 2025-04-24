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
		
		canvasLocalPlayer.AimJoystick.Flicked += OnAimJoystickFlicked;
		canvasLocalPlayer.AimJoystick.FlickCanceled += OnAimJoystickFlickCanceled;
		canvasLocalPlayer.AimJoystick.Tapped += OnAimJoystickTapped;
	}

	public override void _PhysicsProcess(double delta) {
		// This is run only on the server to update the MoveDirection of the ActorPlayer.
		if (Multiplayer.MultiplayerPeer != null && Multiplayer.IsServer()) {
			_actorPlayer.Movement.MoveDirection = MoveDirection.Normalized();
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
		
		_actorPlayer.Attack.DebugDirection = AimDirection;
		
		if (AimDirection.IsZeroApprox()) {
			_attackIndicator.Active = false;
		}
		else {
			_attackIndicator.Active = true;
			_attackIndicator.HandleInput(AimDirection);
		}
	}
	
	private void OnAimJoystickFlicked(Vector2 inputDirection) {
		RpcId(1, MethodName.Attack, inputDirection);
	}
	
	private void OnAimJoystickFlickCanceled() {
		Input.VibrateHandheld(Mathf.RoundToInt(0.1f * 1000), 0.1f);
	}
	
	private void OnAimJoystickTapped() {
		RpcId(1, MethodName.AutoAttack);
	}

	[Rpc(CallLocal = false)]
	private void Attack(Vector2 direction) {
		GD.Print($"Attack with direction: {direction}");
		_actorPlayer.Attack.TryBegin(direction);
	}
	
	[Rpc(CallLocal = false)]
	private void AutoAttack() {
		GD.Print("Auto attack");
	}
	
	[Rpc(CallLocal = false)]
	private void SuperAttack(Vector2 direction) {
		GD.Print($"Super attack with direction: {direction}");
	}
}