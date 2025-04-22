using Godot;

namespace GodBrawl.Game.UserInterface;

[Tool]
public partial class Progressbar : Panel {
	private float _progressRatio = 0.5f;
	[Export(PropertyHint.Range, "0.0, 1.0,")] public float ProgressRatio {
		get => _progressRatio;
		set {
			_progressRatio = value;
			if (_fillContainer == null || _spacer == null) {
				return;
			}
			
			_fillContainer.SizeFlagsStretchRatio = _progressRatio;
			_spacer.SizeFlagsStretchRatio = 1 - _progressRatio;
		}
	}

	private Color _backgroundColorUp = new("7c7c7c");
	[Export] public Color BackgroundColorUp {
		get => _backgroundColorUp;
		set {
			_backgroundColorUp = value;
			_backgroundUp?.SetColor(_backgroundColorUp);
		}
	}

	private Color _backgroundColorDown = new("4d4d4d");
	[Export] public Color BackgroundColorDown {
		get => _backgroundColorDown;
		set {
			_backgroundColorDown = value;
			_backgroundDown?.SetColor(_backgroundColorDown);
		}
	}
	
	private Color _fillColorUp = new("33ff59");
	[Export] public Color FillColorUp {
		get => _fillColorUp;
		set {
			_fillColorUp = value;
			_fillUp?.SetColor(_fillColorUp);
		}
	}
	
	private Color _fillColorDown = new("26bf43");
	[Export] public Color FillColorDown {
		get => _fillColorDown;
		set {
			_fillColorDown = value;
			_fillDown?.SetColor(_fillColorDown);
		}
	}

	[Export] private Control _fillContainer;
	[Export] private Control _spacer;
	[Export] private ColorRect _backgroundUp;
	[Export] private ColorRect _backgroundDown;
	[Export] private ColorRect _fillUp;
	[Export] private ColorRect _fillDown;
}