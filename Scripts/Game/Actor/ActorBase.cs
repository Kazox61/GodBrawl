using Godot;

namespace GodBrawl.Game.Actor;

public partial class ActorBase : CharacterBody3D {
	[Export] public float Speed = 5f;
	[Export] public float MaxVelocity = 10f;

	[Export] public Node3D Body;
	
	// This can be set from outside to move the Actor
	[Export] public Vector2 MoveDirection;
	
	private float Gravity => ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();

	public override void _Ready() {
		/*
		 * This will enable the physics process only if it is the authority.
		 * This will be always the server
		 */

		SetPhysicsProcess(IsMultiplayerAuthority());
	}

	// This method will only run on the server
	public override void _PhysicsProcess(double delta) {
		var deltaTime = (float)delta;
		
		var velocity = Velocity;

		if (!IsOnFloor()) {
			velocity.Y -= Gravity * deltaTime;
		}

		var direction = new Vector3(MoveDirection.X, 0, MoveDirection.Y);
		if (direction.IsZeroApprox()) {
			velocity.X = Mathf.MoveToward(velocity.X, 0, Speed);
			velocity.Z = Mathf.MoveToward(velocity.Z, 0, Speed);
		}
		else {
			velocity.X = direction.X * Speed;
			velocity.Z = direction.Z * Speed;
		}

		velocity = velocity.LimitLength(MaxVelocity);
		
		var velocity2d = new Vector2(velocity.X, velocity.Z);

		if (velocity2d.Length() > 0f) {
			var targetDirection = new Vector2(direction.Z, direction.X).Normalized();
			var currentDirection = Vector2.FromAngle(Body.Rotation.Y).Normalized();
			
			var bodyRotation = Body.Rotation;
			bodyRotation.Y = currentDirection.Slerp(targetDirection, deltaTime * 10f).Angle();
			Body.Rotation = bodyRotation;
		}
		
		Velocity = velocity;

		MoveAndSlide();

		Rpc(MethodName.PlayAnimation, velocity2d.Length() > 0f ? "run" : "idle");
	}
	
	[Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true)]
	private void PlayAnimation(string animationName) {
		Body.Call(animationName);
	}
}