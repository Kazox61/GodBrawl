using System;
using Godot;

namespace GodBrawl.Game.Actor;

public abstract partial class ActorAttackIndicator : MeshInstance3D {
	private Color _fillColor = new(1f, 1f, 1f, 0.5f);
	[Export] public Color FillColor {
		get => _fillColor;
		set {
			_fillColor = value;
			
			Update();
		}
	}
	
	private Color _borderColor = new(1f, 1f, 1f);
	[Export] public Color BorderColor {
		get => _borderColor;
		set {
			_borderColor = value;
			
			Update();
		}
	}
	
	private float _thickness = 0.35f;

	[Export]
	public float Thickness {
		get => _thickness;
		set {
			_thickness = value;
			
			Update();
		}
	}
	
	private float _maxLength = 5f;
	[Export] public float MaxLength {
		get => _maxLength;
		set {
			_maxLength = value;
			
			Update();
		}
	}

	[Export]
	public virtual bool Active {
		get => Visible;
		set {
			Visible = value;
			
			Update();
		}
	}

	public virtual void HandleInput(Vector2 input) {
		var angleRad = Mathf.Atan2(input.X, input.Y);
		Rotation = new Vector3(0, angleRad, 0);
	}

	protected ActorAttackIndicator ResetMesh() {
		if (Mesh is ImmediateMesh immediateMesh) {
			immediateMesh.ClearSurfaces();
		}

		return this;
	}

	protected ActorAttackIndicator DrawLine(Vector3 startPoint, float length, float thickness, Color color) {
		if (Mesh is not ImmediateMesh immediateMesh) {
			return this;
		}

		immediateMesh.SurfaceBegin(Mesh.PrimitiveType.TriangleStrip);
		immediateMesh.SurfaceSetColor(color);

		var a = new Vector3(startPoint.X - thickness, startPoint.Y, startPoint.Z);
		var b = new Vector3(startPoint.X + thickness, startPoint.Y, startPoint.Z);
		var c = new Vector3(startPoint.X - thickness, startPoint.Y, startPoint.Z + length);
		var d = new Vector3(startPoint.X + thickness, startPoint.Y, startPoint.Z + length);

		immediateMesh.SurfaceAddVertex(a);
		immediateMesh.SurfaceAddVertex(b);
		immediateMesh.SurfaceAddVertex(c);
		immediateMesh.SurfaceAddVertex(d);

		immediateMesh.SurfaceEnd();

		return this;
	}


	protected ActorAttackIndicator DrawLine(Vector3 startPoint, Vector3 endPoint, float thickness, Color color) {
		if (Mesh is not ImmediateMesh immediateMesh) {
			return this;
		}

		immediateMesh.SurfaceBegin(Mesh.PrimitiveType.TriangleStrip);
		immediateMesh.SurfaceSetColor(color);

		var a = new Vector3(startPoint.X - thickness, startPoint.Y, startPoint.Z);
		var b = new Vector3(startPoint.X + thickness, startPoint.Y, startPoint.Z);
		var c = new Vector3(startPoint.X - thickness, endPoint.Y, endPoint.Z);
		var d = new Vector3(startPoint.X + thickness, endPoint.Y, endPoint.Z);

		immediateMesh.SurfaceAddVertex(a);
		immediateMesh.SurfaceAddVertex(b);
		immediateMesh.SurfaceAddVertex(c);
		immediateMesh.SurfaceAddVertex(d);

		immediateMesh.SurfaceEnd();

		return this;
	}

	protected ActorAttackIndicator DrawLine2(Vector3 startPoint, Vector3 endPoint, float thickness, Color color) {
		if (Mesh is not ImmediateMesh immediateMesh) {
			return this;
		}

		immediateMesh.SurfaceBegin(Mesh.PrimitiveType.TriangleStrip);
		immediateMesh.SurfaceSetColor(color);

		var a = new Vector3(startPoint.X, startPoint.Y, startPoint.Z - thickness);
		var b = new Vector3(startPoint.X, startPoint.Y, startPoint.Z + thickness);
		var c = new Vector3(endPoint.X, startPoint.Y, endPoint.Z - thickness);
		var d = new Vector3(endPoint.X, startPoint.Y, endPoint.Z + thickness);

		immediateMesh.SurfaceAddVertex(a);
		immediateMesh.SurfaceAddVertex(b);
		immediateMesh.SurfaceAddVertex(c);
		immediateMesh.SurfaceAddVertex(d);

		immediateMesh.SurfaceEnd();
		
		return this;
	}

	protected ActorAttackIndicator DrawOutline(Vector3 startPoint, float length, float thickness, Color color, bool drawEnd = true) {
		const float lineThickness = 0.03f;
		var endPoint1 = new Vector3(thickness + lineThickness, startPoint.Y, startPoint.Z + length);
		var endPoint2 = new Vector3(-thickness - lineThickness, startPoint.Y, startPoint.Z + length);
		DrawLine(
			startPoint + new Vector3(thickness + lineThickness, 0f, 0f),
			endPoint1,
			lineThickness,
			color
		);
		DrawLine(
			startPoint + new Vector3(-thickness - lineThickness, 0f, 0f),
			endPoint2,
			lineThickness,
			color
		);

		if (drawEnd) {
			DrawLine2(
				endPoint1 + new Vector3(lineThickness, 0f, 0f),
				endPoint2 + new Vector3(-lineThickness, 0f, 0f),
				lineThickness,
				color
			);
		}

		return this;
	}

	protected ActorAttackIndicator DrawOutline(
		Vector3 startPoint, 
		Vector3 endPoint, 
		float thickness, 
		Color color,
		bool drawEnd = true
	) {
		const float lineThickness = 0.03f;
		var endPoint1 = new Vector3(thickness + lineThickness, endPoint.Y, endPoint.Z);
		var endPoint2 = new Vector3(-thickness - lineThickness, endPoint.Y, endPoint.Z);
		DrawLine(
			startPoint + new Vector3(thickness + lineThickness, 0f, 0f),
			endPoint1,
			lineThickness,
			color
		);
		DrawLine(
			startPoint + new Vector3(-thickness - lineThickness, 0f, 0f),
			endPoint2,
			lineThickness,
			color
		);

		if (drawEnd) {
			DrawLine2(
				endPoint1 + new Vector3(lineThickness, 0f, 0f),
				endPoint2 + new Vector3(-lineThickness, 0f, 0f),
				lineThickness,
				color
			);
		}
		
		return this;
	}

	protected bool TryGetCollision(Vector3 start, Vector3 end, out Vector3 collisionPoint) {
		collisionPoint = Vector3.Zero;
		
		var world3d = GetWorld3D();
		
		if (world3d == null || !IsInstanceValid(world3d)) {
			return false;
		}
		
		var space = world3d.DirectSpaceState;
		var query = PhysicsRayQueryParameters3D.Create(GlobalPosition + start, GlobalPosition + end);

		var collision = space.IntersectRay(query);
		try {
			if (collision.Count == 0) {
				return false;
			}

			collisionPoint = collision["position"].AsVector3() - GlobalPosition;
			return true;
		}
		catch (Exception) {
			return false;
		}
	}

	public override void _Process(double delta) {
		Update();
	}

	protected virtual ActorAttackIndicator Update() {
		return this;
	}
}