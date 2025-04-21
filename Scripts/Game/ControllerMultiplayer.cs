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
	[Export] private PackedScene _localPlayerPrefab;
	[Export] private PackedScene _actorPlayerPrefab;
	[Export] private Node3D _rootNode;
	[Export] private Map _map;
	
	public readonly HashSet<LocalPlayer> LocalPlayers = [];
	public readonly HashSet<ActorPlayer> ActorPlayers = [];

	public LocalPlayer ControlledLocalPlayer;
	
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
	}
	
	private void StartClient() {
		var peer = new ENetMultiplayerPeer();
		
		peer.CreateClient(_startConfig.Ip, _startConfig.Port);
		
		Multiplayer.MultiplayerPeer = peer;
	}
	
	private void OnClientConnected(long peer) {
		var localPlayer = _localPlayerPrefab.Instantiate<LocalPlayer>();
		localPlayer.PeerId = (int)peer;
		
		_rootNode.AddChild(localPlayer, true);
		
		var actorPlayer = _actorPlayerPrefab.Instantiate<ActorPlayer>();
		actorPlayer.PlayerPeerId = (int)peer;
		actorPlayer.CompVisibility.InitializeVisibilityFilter();
		
		_rootNode.AddChild(actorPlayer, true);
		actorPlayer.AssignLocalPlayer();

		var spawner = _actorPlayerSpawners.GetRandomElement();
		_actorPlayerSpawners.Remove(spawner);
		
		actorPlayer.GlobalPosition = spawner.GlobalPosition;
	}
	
	private void OnClientDisconnected(long peer) { }
}