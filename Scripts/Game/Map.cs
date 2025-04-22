using Godot;

namespace GodBrawl.Game;

public partial class Map : Node3D {
	[Export] public GridMapSpawner GridMapSpawner;
	[Export] public Node3D[] ActorPlayerSpawners;
}