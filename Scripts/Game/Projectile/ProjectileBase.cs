using Godot;

namespace GodBrawl.Game.Projectile;

public partial class ProjectileBase : Node3D {
	[Export] private Area3D _area;
	
	private Vector3 _direction;
	private float _maxDistance;
	private float _speed;

	private Vector3 _destination;

	public override void _Ready() {
		SetProcess(IsMultiplayerAuthority());

		if (IsMultiplayerAuthority()) {
			_area.AreaEntered += OnAreaEntered;
			_area.BodyEntered += OnBodyEntered;
		}
	}

	public void Initialize(Vector3 direction, float maxDistance, float speed) {
		_direction = direction;
		_maxDistance = maxDistance;
		_speed = speed;
		
		_destination = GlobalPosition + direction * maxDistance;
	}

	public override void _Process(double delta) {
		GlobalPosition = GlobalPosition.MoveToward(_destination, (float)delta * _speed);
		
		if (GlobalPosition.IsEqualApprox(_destination)) {
			this.Remove();
		}
	}

	private void OnAreaEntered(Area3D area) {
		GD.Print(area.Name);
	}
	
	private void OnBodyEntered(Node3D body) {
		GD.Print(body.Name);
	}
}