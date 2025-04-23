using Godot;

namespace GodBrawl.Game;

[Tool]
public partial class DamageZone : Area3D {
	[Export] private CollisionShape3D _collisionShape;
	[Export] private BoxShape3D _boxShape;
	[Export] private MeshInstance3D _debugMesh;

	public enum DamageZoneExpandDirection {
		North,
		South,
		West,
		East
	}

	private DamageZoneExpandDirection _expandDirection;
	[Export] public DamageZoneExpandDirection ExpandDirection {
		get => _expandDirection;
		set {
			_expandDirection = value;
			
			Update();
			UpdateDebugMesh();
		}
	}

	private float _mapSize = 74f;
	[Export(PropertyHint.Range, "0,100,1,or_greater")]
	public float MapSize {
		get => _mapSize;
		set {
			if (_mapSize < 0) {
				_mapSize = 0;
				
				return;
			}
			
			_mapSize = value;
			
			Update();
			UpdateDebugMesh();
		}
	}

	private float _height = 1f;
	[Export(PropertyHint.Range, "1,10,1,or_greater")]
	public float Height {
		get => _height;
		set {
			if (_height < 1) {
				_height = 1;
				
				return;
			}
			
			_height = value;
			
			Update();
			UpdateDebugMesh();
		}
	}
	
	private float _expandSize = 1f;

	[Export(PropertyHint.Range, "1,100,1,or_greater")]
	public float ExpandSize {
		get => _expandSize;
		set {
			if (_expandSize < 1f) {
				_expandSize = 1f;

				return;
			}
			
			_expandSize = value;
			
			Update();
			UpdateDebugMesh();
		}
	}

	private void Update() {
		if (_boxShape == null || _collisionShape == null) {
			return;
		}
		
		var size = new Vector3(1f, Height, 1f);
		var position = new Vector3();

		switch (_expandDirection) {
			case DamageZoneExpandDirection.North:
				size.Z = _expandSize;
				size.X = _mapSize;
				position.Z = -_mapSize * 0.5f + _expandSize * 0.5f;
				break;
			case DamageZoneExpandDirection.South:
				size.Z = _expandSize;
				size.X = _mapSize;
				position.Z = _mapSize * 0.5f - _expandSize * 0.5f;
				break;
			case DamageZoneExpandDirection.West:
				size.X = _expandSize;
				size.Z = _mapSize;
				position.X = -_mapSize * 0.5f + _expandSize * 0.5f;
				break;
			case DamageZoneExpandDirection.East:
				size.X = _expandSize;
				size.Z = _mapSize;
				position.X = _mapSize * 0.5f - _expandSize * 0.5f;
				break;
		}

		_boxShape.Size = size;
		_collisionShape.Position = position;
	}
	
	private void UpdateDebugMesh() {
		if (_debugMesh == null) {
			return;
		}
		
		if (_boxShape == null || _collisionShape == null) {
			return;
		}
		
		GD.Print($"Height: {Height}, MapSize: {_mapSize}, ExpandSize: {_expandSize}");
		
		var size = new Vector3(1f, Height, 1f);
		var position = new Vector3();

		switch (_expandDirection) {
			case DamageZoneExpandDirection.North:
				size.Z = _expandSize;
				size.X = _mapSize;
				position.Z = -_mapSize * 0.5f + _expandSize * 0.5f;
				break;
			case DamageZoneExpandDirection.South:
				size.Z = _expandSize;
				size.X = _mapSize;
				position.Z = _mapSize * 0.5f - _expandSize * 0.5f;
				break;
			case DamageZoneExpandDirection.West:
				size.X = _expandSize;
				size.Z = _mapSize;
				position.X = -_mapSize * 0.5f + _expandSize * 0.5f;
				break;
			case DamageZoneExpandDirection.East:
				size.X = _expandSize;
				size.Z = _mapSize;
				position.X = _mapSize * 0.5f - _expandSize * 0.5f;
				break;
		}

		_debugMesh.Scale = size;
		_debugMesh.Position = position;
	}
}