using Godot;

namespace GodBrawl.Game.Actor;

[Tool, GlobalClass]
public abstract partial class SpreadConfig : Resource {
	[Export] public int ProjectileCount = 1;

	public abstract (Vector3 offset, Vector3 direction)[] GetSpread(Vector2 inputDirection);
}