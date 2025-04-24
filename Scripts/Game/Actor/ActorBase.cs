using Godot;

namespace GodBrawl.Game.Actor;

public partial class ActorBase : CharacterBody3D, IAttackable {
	[Export] public Node3D Body;
	[Export] public ActorStatusDisplay StatusDisplay;
	[Export] public Area3D BodyTrigger;
	
	[Export] public ActorCompMultiplayer CompMultiplayer;
	[Export] public ActorCompVisibility CompVisibility;

	[Export] public ActorMovement Movement;
	[Export] public ActorAttackBase Attack;
	
	[Export] public int MaxHealth;
	
	private int _health;
	public int Health {
		get => _health;
		set {
			_health = value;

			StatusDisplay.Healthbar.ProgressRatio = (float)Health / MaxHealth;
			StatusDisplay.HealthLabel.Text = Health.ToString();
			
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

	[Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true)]
	private void PlayAnimation(string animationName) {
		Body.Call(animationName);
	}

	public void ApplyDamage(int damage, ActorBase source) {
		Health -= damage;
	}
}