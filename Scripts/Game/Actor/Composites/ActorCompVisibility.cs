using System.Collections.Generic;
using System.Linq;
using Godot;

namespace GodBrawl.Game.Actor;


/*
 * _compMultiplayer.MultiplayerSynchronizer public_visibility needs to be true
 * if not, the visibility filter will not work as expected
 */
public partial class ActorCompVisibility : Node3D {
	[Export] private ActorCompMultiplayer _compMultiplayer;
	[Export] private Area3D _closeArea;
	
	private ActorBase _actor;
	
	private readonly HashSet<ActorPlayer> _closeActors = [];
	private readonly HashSet<Area3D> _bushAreas = [];
	
	private bool InsideBush => _bushAreas.Count > 0;
	
	private Callable VisibilityFilter => Callable.From((int peerId) => {
		if (peerId == _compMultiplayer.PlayerPeerId) {
			return true;
		}
		
		return !InsideBush || _closeActors.Any(actor => actor.CompMultiplayer.PlayerPeerId == peerId);
 	});

	public override void _EnterTree() {
		_actor = this.FindParentOfType<ActorBase>();
	}

	public override void _Ready() {
		if (_compMultiplayer.ControllerLocalPlayer.IsMultiplayerAuthority()) {
			_closeArea.AreaEntered += (area) => {
				if (area.IsInGroup("Bush")) {
					var mesh = (BoxMesh)area.GetParent<MeshInstance3D>().GetMesh();
					var mat = (StandardMaterial3D)mesh.Material;
					var color = mat.AlbedoColor;
					color.A = 0.2f;
					mat.AlbedoColor = color;
				}
			};

			_closeArea.AreaExited += (area) => {
				if (area.IsInGroup("Bush")) {
					var mesh = (BoxMesh)area.GetParent<MeshInstance3D>().GetMesh();
					var mat = (StandardMaterial3D)mesh.Material;
					var color = mat.AlbedoColor;
					color.A = 1f;
					mat.AlbedoColor = color;
				}
			};
		}
		
		if (!IsMultiplayerAuthority()) {
			return;
		}
		
		_closeArea.BodyEntered += (body) => {
			if (body is ActorPlayer enteredActor && enteredActor != _actor) {
				_closeActors.Add(enteredActor);
			}
		};
		
		_closeArea.BodyExited += (body) => {
			if (body is ActorPlayer exitedActor && exitedActor != _actor) {
				_closeActors.Remove(exitedActor);
			}
		};
		
		_actor.BodyTrigger.AreaEntered += (area) => {
			if (area.IsInGroup("Bush")) {
				_bushAreas.Add(area);
			}
		};
		
		_actor.BodyTrigger.AreaExited += (area) => {
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