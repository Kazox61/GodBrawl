using Godot;

namespace GodBrawl.Game.Actor;

public abstract partial class ActorAttackBase : Node3D {
	[Export] protected bool Debug;

	public Vector2 DebugDirection;
	
	protected ActorBase Actor;

	public override void _EnterTree() {
		Actor = this.FindParentOfType<ActorBase>();
	}
	
	public abstract bool CanAttack();

	public abstract bool TryBegin(Vector2 inputDirection);

	public override void _Process(double delta) {
		if (Debug) {
			OnDebug();
		}
		
		OnProcess();
	}
	
	public virtual void OnDebug() { }

	public virtual void OnProcess() {
		
	}
}