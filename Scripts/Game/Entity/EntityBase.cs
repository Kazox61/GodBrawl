using GodBrawl.Game.Actor;
using GodBrawl.Game.UserInterface;
using Godot;

namespace GodBrawl.Game.Entity;

public partial class EntityBase : Node3D {
	[Export] public Progressbar HealthProgress;
	[Export] public Label HealthLabel;

	[Export] public bool RequiresSuperAttack;
	[Export] public int MaxHealth;
	
	private int _health;
	public int Health {
		get => _health;
		set {
			_health = value;

			if (HealthProgress != null) {
				HealthProgress.ProgressRatio = (float)Health / MaxHealth;
			}
			
			if (HealthLabel != null) {
				HealthLabel.Text = Health.ToString();
			}
			
			if (!IsMultiplayerAuthority()) {
				return;
			}
			
			if (_health <= 0) {
				_health = 0;
				this.Remove();
			}
		}
	}
	
	public override void _Ready() {
		if (IsMultiplayerAuthority()) {
			_health = MaxHealth;
		}
	}

	public void ApplyDamage(int damage, ActorBase source) {
		if (RequiresSuperAttack) {
			return;
		}
		
		Health -= damage;
	}
	
	public void ApplySuperAttackDamage(int damage, ActorBase source) {
		Health -= damage;
	}
}