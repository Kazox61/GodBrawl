using System.Collections.Generic;
using System.Linq;
using Godot;

namespace GodBrawl.Game;

public partial class GridMapSpawner : GridMap {
	[Export] public Godot.Collections.Dictionary<int, SpawnConfig> TileToSceneMappings = new();
	[Export] private int _actorPlayerSpawnerTileId;

	private readonly HashSet<Vector3> _spawnPositions = [];
	
	public HashSet<Vector3> SpawnPositions => _spawnPositions.ToHashSet();
	
	public void ReplaceGridTiles() {
		foreach (var cellPos in GetUsedCells()) {
			var tileId = GetCellItem(cellPos);
			
			SetCellItem(cellPos, -1);
			
			var worldPos = MapToLocal(cellPos);
			
			if (tileId == _actorPlayerSpawnerTileId) {
				_spawnPositions.Add(worldPos);
				continue;
			}

			if (!TileToSceneMappings.TryGetValue(tileId, out var spawnConfig)) {
				continue;
			}
			
			if (GD.Randf() > spawnConfig.SpawnProbability) {
				continue;
			}

			var instance = spawnConfig.Prefab.Instantiate<Node3D>();
			if (instance == null) {
				continue;
			}

			ControllerMultiplayer.Instance.SpawnParent.AddChild(instance, true);

			instance.GlobalPosition = worldPos;
		}
	}
}