using Godot;
using Godot.Collections;

namespace GodBrawl.Game;

public partial class GridMapSpawner : GridMap {
	[Export] public Dictionary<int, SpawnConfig> TileToSceneMappings = new();
	[Export] private Node _spawnParent;

	public void ReplaceGridTiles() {
		foreach (var cellPos in GetUsedCells()) {
			var tileId = GetCellItem(cellPos);

			if (!TileToSceneMappings.TryGetValue(tileId, out var spawnConfig)) {
				continue;
			}

			SetCellItem(cellPos, -1);
			
			if (GD.Randf() > spawnConfig.SpawnProbability) {
				continue;
			}
			
			var worldPos = MapToLocal(cellPos);

			var instance = spawnConfig.Prefab.Instantiate<Node3D>();
			if (instance == null) {
				continue;
			}

			_spawnParent.AddChild(instance, true);

			instance.GlobalPosition = worldPos;
		}
	}
}