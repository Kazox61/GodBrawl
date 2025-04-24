using GodBrawl.Game.UserInterface;
using Godot;

namespace GodBrawl.Game.Actor;

public partial class ActorStatusDisplay : Node3D {
	[Export] public Progressbar Healthbar;
	[Export] public Label HealthLabel;
}