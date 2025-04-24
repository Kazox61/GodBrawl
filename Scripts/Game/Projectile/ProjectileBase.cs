using GodBrawl.Game.Actor;
using Godot;

namespace GodBrawl.Game.Projectile;

public partial class ProjectileBase : Node3D {
	[Export] private Area3D _area;
	
	private Vector3 _direction;
	private float _maxDistance;
	private float _speed;
	private int _damage;
	private ActorBase _source;

	private Vector3 _destination;

	public override void _Ready() {
		SetProcess(IsMultiplayerAuthority());

		if (IsMultiplayerAuthority()) {
			_area.AreaEntered += OnAreaEntered;
			_area.BodyEntered += OnBodyEntered;
		}
	}

	public void Initialize(Vector3 direction, float maxDistance, float speed, int damage, ActorBase source) {
		_direction = direction;
		_maxDistance = maxDistance;
		_speed = speed;
		_damage = damage;
		_source = source;
		
		_destination = GlobalPosition + direction * maxDistance;
	}

	public override void _Process(double delta) {
		GlobalPosition = GlobalPosition.MoveToward(_destination, (float)delta * _speed);
		
		if (GlobalPosition.IsEqualApprox(_destination)) {
			this.Remove();
		}
	}

	private void OnAreaEntered(Area3D area) {
		if (area is IAttackable attackable) {
			attackable.ApplyDamage(_damage, _source);
		}
	}
	
	private void OnBodyEntered(Node3D body) {
		if (body != _source && body is IAttackable attackable) {
			attackable.ApplyDamage(_damage, _source);
			Callable.From(this.Remove).CallDeferred();
		}
	}
}