using Godot;

namespace GodBrawl.Game.Actor;

[Tool]
public partial class ActorAttackIndicatorLine : ActorAttackIndicator {
	protected override ActorAttackIndicator Update() {
		ResetMesh();

		if (!Active) {
			return this;
		}
		
		if (!IsInsideTree()) {
			return this;
		}

		var start = new Basis(new Vector3(0, 1, 0), GlobalRotation.Y) * Vector3.Zero;
		var end =  new Basis(new Vector3(0, 1, 0), GlobalRotation.Y) * new Vector3(0f, 0f, MaxLength);

		var success = TryGetCollision(start, end, out var collisionPoint);
		if (success) {
			var distance = start.DistanceTo(collisionPoint);
			DrawLine(Vector3.Zero, distance, Thickness, FillColor);
			DrawOutline(Vector3.Zero, distance, Thickness, BorderColor);
			return this;
		}
		
		DrawLine(Vector3.Zero, MaxLength, Thickness, FillColor);
		DrawOutline(Vector3.Zero, MaxLength, Thickness, BorderColor);

		return this;
	}
}