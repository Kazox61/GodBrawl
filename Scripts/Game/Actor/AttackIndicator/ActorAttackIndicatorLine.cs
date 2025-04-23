using Godot;

namespace GodBrawl.Game.Actor;

[Tool]
public partial class ActorAttackIndicatorLine : ActorAttackIndicator {
	[Export] private Node3D _pivot;
	
	public override void HandleInput(Vector2 input) {
		var angleRad = Mathf.Atan2(input.X, input.Y);
		_pivot.Rotation = new Vector3(0, angleRad, 0);
	}
	
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