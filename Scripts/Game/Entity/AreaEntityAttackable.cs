using GodBrawl.Game.Actor;
using Godot;

namespace GodBrawl.Game.Entity;

[GlobalClass]
public partial class AreaEntityAttackable : Area3D, IAttackable {
	[Export] private EntityBase _entity;
	
	public void ApplyDamage(int damage, ActorBase source) {
		_entity.ApplyDamage(damage, source);
	}
}