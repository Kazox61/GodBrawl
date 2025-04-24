using GodBrawl.Game.Actor;

namespace GodBrawl.Game;

public interface IAttackable {
	public void ApplyDamage(int damage, ActorBase source);
}