using Godot;

namespace GodBrawl;

public partial class PlayerTest : CharacterBody3D {
	[Export] private float _speed = 5f;
	
	private const float Gravity = 9.81f * 3f;

	public override void _PhysicsProcess(double delta) {
		var velocity = Velocity;
		
		if (!IsOnFloor()) {
			velocity.Y -= Gravity * (float)delta;
		}
		
		var inputDir = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		var direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();

		velocity.X = direction.X * _speed;
		velocity.Z = direction.Z * _speed;

		Velocity = velocity;

		MoveAndSlide();
	}
}