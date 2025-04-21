using System.Collections.Generic;
using System.Linq;
using Godot;

namespace GodBrawl.Game.Actor;


/*
 * _compMultiplayer.MultiplayerSynchronizer public_visibility needs to be true
 * if not, the visibility filter will not work as expected
 */
public partial class ActorCompVisibility : Node3D {
	[Export] private ActorBase _actor;
	[Export] private ActorCompMultiplayer _compMultiplayer;
	[Export] private Area3D _closeArea;

	
	private readonly HashSet<ActorPlayer> _closeActors = [];
	private readonly HashSet<Area3D> _bushAreas = [];
	
	private bool InsideBush => _bushAreas.Count > 0;
	
	private Callable VisibilityFilter => Callable.From((int peerId) => {
		if (peerId == _compMultiplayer.PlayerPeerId) {
			return true;
		}
		
		// _closeActors.Any(actor => actor.CompMultiplayer.PlayerPeerId == peerId)
		return !InsideBush;
 	});

	public override void _Ready() {
		if (!IsMultiplayerAuthority()) {
			return;
		}
		
		_closeArea.AreaEntered += (area) => {
			var enteredActor = area.GetParentOrNull<ActorPlayer>();
			if (enteredActor == null || enteredActor == _actor) {
				return;
			}
			
			_closeActors.Add(enteredActor);
		};
		
		_closeArea.AreaExited += (area) => {
			var exitedActor = area.GetParentOrNull<ActorPlayer>();
			if (exitedActor == null || exitedActor == _actor) {
				return;
			}
			
			_closeActors.Remove(exitedActor);
		};
		
		_actor.BodyArea.AreaEntered += (area) => {
			if (area.IsInGroup("Bush")) {
				_bushAreas.Add(area);
			}
		};
		
		_actor.BodyArea.AreaExited += (area) => {
			if (area.IsInGroup("Bush")) {
				_bushAreas.Remove(area);
			}
		};
	}

	public void InitializeVisibilityFilter() {
		_compMultiplayer.MultiplayerSynchronizer.AddVisibilityFilter(VisibilityFilter);
		_compMultiplayer.MultiplayerSynchronizer.UpdateVisibility();
	}
}