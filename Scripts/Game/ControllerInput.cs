using GodBrawl.Game.Actor;
using GodBrawl.Game.UserInterface;
using Godot;

namespace GodBrawl.Scripts.Game;

public partial class ControllerInput : Node {
	[Export] private LocalPlayer _localPlayer;
	[Export] private PackedScene _canvasLocalPlayerPrefab;
	
	public CanvasLocalPlayer CanvasLocalPlayer;
	
	public Vector2 MoveDirection;
	public Vector2 AimDirection;
	
	public override void _Ready() {
		if (!IsMultiplayerAuthority()) {
			return;
		}
		
		CanvasLocalPlayer = _canvasLocalPlayerPrefab.Instantiate<CanvasLocalPlayer>();
		GetTree().Root.AddChild(CanvasLocalPlayer);
		
		CanvasLocalPlayer.AimJoystick.JoystickReleased += OnAimJoystickReleased;
	}

	public override void _PhysicsProcess(double delta) {
		if (Multiplayer.IsServer() && _localPlayer.ActorPlayer != null) {
			GD.Print($"{_localPlayer.Name}, {_localPlayer.ActorPlayer.Name}");;
			_localPlayer.ActorPlayer.MoveDirection = MoveDirection;
			return;
		}

		if (!IsMultiplayerAuthority()) {
			return;
		}
		
		MoveDirection = Input.GetVector("move_left", "move_right", "move_up", "move_down");
		AimDirection = Input.GetVector("aim_left", "aim_right", "aim_up", "aim_down");

		// can be null since the ActorPlayer spawns after the LocalPlayer
		_localPlayer.ActorPlayer?.UpdateAttackIndicator(AimDirection);
	}

	private void OnAimJoystickReleased(Vector2 inputDirection) {
		RpcId(1, MethodName.Shoot, inputDirection);
	}

	[Rpc(CallLocal = false)]
	private void Shoot(Vector2 direction) {
		GD.Print($"Shoot with direction: {direction}");
	}
}