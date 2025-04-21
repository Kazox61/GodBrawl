using Godot;
using Godot.Collections;

namespace GodBrawl.Game;

public partial class GridMapSpawner : GridMap {
	[Export] public Dictionary<int, PackedScene> TileToSceneMappings = new();

	public override void _Ready() {
		ReplaceGridTiles();
	}

	private void ReplaceGridTiles() {
		foreach (var cellPos in GetUsedCells()) {
			var tileId = GetCellItem(cellPos);

			if (!TileToSceneMappings.TryGetValue(tileId, out PackedScene prefab)) {
				continue;
			}

			var worldPos = MapToLocal(cellPos);

			var instance = prefab.Instantiate<Node3D>();
			if (instance == null) {
				continue;
			}

			AddChild(instance);

			instance.GlobalPosition = worldPos;

			SetCellItem(cellPos, -1);
		}
	}
}