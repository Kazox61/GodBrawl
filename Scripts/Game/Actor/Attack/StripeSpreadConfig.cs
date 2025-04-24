using Godot;

namespace GodBrawl.Game.Actor;

[Tool, GlobalClass]
public partial class StripeSpreadConfig : SpreadConfig {
	[Export(PropertyHint.Range, "0, 1, 0.1")]
	public float StripeThickness = 0.35f;

	public override (Vector3 offset, Vector3 direction)[] GetSpread(Vector2 inputDirection) {
		var result = new (Vector3 offset, Vector3 direction)[ProjectileCount];

		var forward = new Vector3(inputDirection.X, 0, inputDirection.Y).Normalized();
		var right = forward.Cross(Vector3.Up).Normalized();

		var totalWidth = StripeThickness;
		var step = ProjectileCount <= 1 ? 0 : totalWidth / (ProjectileCount - 1);

		for (var i = 0; i < ProjectileCount; i++) {
			var offsetAmount = 0f;

			if (ProjectileCount > 1) {
				offsetAmount = -totalWidth / 2 + i * step;
			}

			var offset = right * offsetAmount;

			result[i] = (offset, forward);
		}
		
		return result;
	}
}