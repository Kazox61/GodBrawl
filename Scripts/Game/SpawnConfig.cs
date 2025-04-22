using Godot;

namespace GodBrawl.Game;

[GlobalClass]
public partial class SpawnConfig : Resource {
	[Export] public PackedScene Prefab;
	[Export(PropertyHint.Range, "0.0, 1.0, 0.05")] public float SpawnProbability = 1f;
}