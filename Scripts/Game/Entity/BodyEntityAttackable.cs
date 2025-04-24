using GodBrawl.Game.Actor;
using Godot;

namespace GodBrawl.Game.Entity;

[GlobalClass]
public partial class BodyEntityAttackable : Node3D, IAttackable {
	[Export] private EntityBase _entity;
	
	public void ApplyDamage(int damage, ActorBase source) {
		_entity.ApplyDamage(damage, source);
	}
}