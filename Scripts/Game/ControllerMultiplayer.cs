using System.Collections.Generic;
using System.Linq;
using GodBrawl.Config;
using GodBrawl.Extensions;
using GodBrawl.Game.Actor;
using Godot;

namespace GodBrawl.Game;

public partial class ControllerMultiplayer : Node {
	public static ControllerMultiplayer Instance;
	
	[Export] private StartConfig _startConfig;
	[Export] private PackedScene _actorPlayerPrefab;
	[Export] private Node3D _spawnParent;
	[Export] private Map _map;
	
	private HashSet<ActorPlayer> _actorPlayers = [];
	
	private List<Node3D> _actorPlayerSpawners = [];

	public override void _EnterTree() {
		Instance = this;
	}

	public override void _Ready() {
		if (_startConfig.IsServer) {
			StartServer();
			
			return;
		}
		
		StartClient();
	}
	
	private void StartServer() {
		var peer = new ENetMultiplayerPeer();
		
		peer.CreateServer(_startConfig.Port);
		
		Multiplayer.MultiplayerPeer = peer;
		
		Multiplayer.PeerConnected += OnClientConnected;
		Multiplayer.PeerDisconnected += OnClientDisconnected;
		
		_actorPlayerSpawners = _map.ActorPlayerSpawners.ToList();
		Callable.From(_map.GridMapSpawner.ReplaceGridTiles).CallDeferred();
	}
	
	private void StartClient() {
		var peer = new ENetMultiplayerPeer();
		
		peer.CreateClient(_startConfig.Ip, _startConfig.Port);
		
		Multiplayer.MultiplayerPeer = peer;
		
		Callable.From(_map.GridMapSpawner.Remove).CallDeferred();
	}
	
	private void OnClientConnected(long peer) {
		var actorPlayer = _actorPlayerPrefab.Instantiate<ActorPlayer>();
		actorPlayer.CompMultiplayer.PlayerPeerId = (int)peer;
		actorPlayer.CompVisibility.InitializeVisibilityFilter();
		
		_spawnParent.AddChild(actorPlayer, true);

		var spawner = _actorPlayerSpawners.GetRandomElement();
		_actorPlayerSpawners.Remove(spawner);
		
		actorPlayer.GlobalPosition = spawner.GlobalPosition;
		
		_actorPlayers.Add(actorPlayer);
	}
	
	private void OnClientDisconnected(long peer) { }
}