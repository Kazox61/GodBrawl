using GodBrawl.Game.Projectile;
using Godot;

namespace GodBrawl.Game.Actor;

[Tool]
public partial class SpreadAttack : ActorAttackBase {
	[Export] public SpreadConfig SpreadConfig;
	[Export] public float FireInterval;
	[Export] public float MaxDistance = 6f;
	[Export] public float Speed = 10f;
	[Export] public PackedScene ProjectilePrefab;
	
	private float _timer;

	public override void _Ready() {
		
	}

	public override bool CanAttack() {
		return true;
	}

	public override bool TryBegin(Vector2 inputDirection) {
		if (!CanAttack()) {
			return false;
		}

		foreach (var (offset, direction) in SpreadConfig.GetSpread(inputDirection)) {
			var projectile = ProjectilePrefab.Instantiate<ProjectileBase>();
			
			ControllerMultiplayer.Instance.SpawnParent.AddChild(projectile, true);
			
			projectile.GlobalPosition = GlobalPosition + offset;
			
			projectile.Initialize(direction, MaxDistance, Speed);
		}

		return true;
	}

	public override void OnDebug() {
		foreach (var (offset, direction) in SpreadConfig.GetSpread(DebugDirection)) {
			DebugDraw3D.DrawArrow(GlobalPosition + offset, GlobalPosition + offset + direction * MaxDistance, Colors.Red, 0.05f);
		}
	}
}