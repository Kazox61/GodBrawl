using Godot;

namespace GodBrawl.Game.Actor;

[Tool, GlobalClass]
public partial class BurstSpreadConfig : SpreadConfig {
	[Export(PropertyHint.Range, "0, 360, 1")] 
	public float BurstAngle = 45f;
	
	public override (Vector3 offset, Vector3 direction)[] GetSpread(Vector2 inputDirection) {
		var result = new (Vector3 offset, Vector3 direction)[ProjectileCount];
    
		var forward = new Vector3(inputDirection.X, 0, inputDirection.Y).Normalized();

		var step = BurstAngle / (ProjectileCount - 1);

		for (var i = 0; i < ProjectileCount; i++) {
			var angle = 0f;
			
			if (ProjectileCount > 1) {
				angle = -BurstAngle / 2 + i * step;
			}
			
			var rotation = Basis.Identity.Rotated(Vector3.Up, Mathf.DegToRad(angle));
			var direction = (rotation * forward).Normalized();
			
			result[i] = (Vector3.Zero, direction);
		}

		return result;
	}
}