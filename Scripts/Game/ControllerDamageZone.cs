using Godot;

namespace GodBrawl.Game;

[Tool]
public partial class ControllerDamageZone : Node {
	[Export] public DamageZone[] DamageZones = [];

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
			
			foreach (var damageZone in DamageZones) {
				damageZone.MapSize = _mapSize;
			}
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
			
			foreach (var damageZone in DamageZones) {
				damageZone.Height = _height;
			}
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
			
			foreach (var damageZone in DamageZones) {
				damageZone.ExpandSize = _expandSize;
			}
		}
	}

	[Export] public float TickDuration = 2f;

	private float _timer;

	public bool DamageZoneClosed => ExpandSize >= MapSize * 0.5f;

	public override void _Ready() {
		SetProcess(IsMultiplayerAuthority());
	}

	public override void _Process(double delta) {
		if (Engine.IsEditorHint()) {
			return;
		}
		
		if (DamageZoneClosed) {
			SetProcess(false);
			
			return;
		}
		
		_timer += (float)delta;

		if (_timer >= TickDuration) {
			_timer -= TickDuration;
			
			ExpandSize += 1f;
		}
	}
}