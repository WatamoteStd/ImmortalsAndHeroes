using Godot;
using Shared.Ability;
using System;

public partial class AbilityButton : Button
{
	
	[Export] private TextureRect _icon;
	private Color _defaultColor = new Color(0.694f, 0.694f, 0.694f);
	private Color _hoverColor = new Color(0.788f, 0.808f, 0.816f);
	private Color _pressColor = new Color(0.589f, 0.589f, 0.589f);

	public override void _Ready()
	{
		
		MouseEntered += () =>
		{
			_icon.SelfModulate = _hoverColor;
		};
		MouseExited += () =>
		{
			_icon.SelfModulate = _defaultColor;
		};
		ButtonDown += () =>
		{
			_icon.SelfModulate = _pressColor;
		};
		ButtonUp += () =>
		{
			_icon.SelfModulate = _defaultColor;
		};

	}
	public void Init(AbilityData data)
	{
		
		_icon.Texture = GD.Load<Texture2D>(data.IconPath);
		_icon.SelfModulate = _defaultColor;

	}




}
