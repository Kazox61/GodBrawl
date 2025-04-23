using Godot;

namespace GodBrawl.Game.Actor;

public partial class ActorBase : CharacterBody3D {
	[Export] public Node3D Body;
	[Export] public Area3D BodyArea;
	
	[Export] public ActorCompMultiplayer CompMultiplayer;
	[Export] public ActorCompVisibility CompVisibility;

	[Export] public ActorMovement Movement;
	
	[Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true)]
	private void PlayAnimation(string animationName) {
		Body.Call(animationName);
	}
}