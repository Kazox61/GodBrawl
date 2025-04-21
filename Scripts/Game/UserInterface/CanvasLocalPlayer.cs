using Godot;
using GodotMobileControls.MobileJoystick;

namespace GodBrawl.Game.UserInterface;

public partial class CanvasLocalPlayer : CanvasLayer {
	[Export] public MobileJoystick MoveJoystick;
	[Export] public MobileJoystick AimJoystick;
}