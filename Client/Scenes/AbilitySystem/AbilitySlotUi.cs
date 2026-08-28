using Godot;
using Shared.Ability;
using System;

public partial class AbilitySlotUi : Control
{
	
	[Export] private TextureRect _icon;
	[Export] private TextureProgressBar _cooldownProgress;
	[Export] private Label _cooldownLabel;
	[Export] private Label _skillBind;

	private float _totalCooldown;
	private float _currentCooldown;
	public bool IsOnCooldown = false;

	public AbilityTypes AbilityId {get; private set;}

	public override void _Ready()
	{

		_cooldownLabel.Visible = false;
		_cooldownProgress.Visible = false;

	}


	public void Init(AbilityTypes abilityId)
	{

		if (AbilityRegistry.TryGetAbility(abilityId, out var dllData))
		{
			_icon.Texture = GD.Load<Texture2D>(dllData.IconPath);
		}
		else
		{
			_icon.Texture = GD.Load<Texture2D>("res://Assets/Icons/Ability/Empty/Null.png");
		}

		AbilityId = abilityId;

	}

	public void UpdateIconBind(string actionName)
	{
		
		var events = InputMap.ActionGetEvents(actionName);

		if (events.Count > 0 && events[0] is InputEventKey keyEvent)
		{
			
			string keyText = OS.GetKeycodeString(keyEvent.PhysicalKeycode != Key.None ? keyEvent.PhysicalKeycode : keyEvent.Keycode);

			_skillBind.Text = keyText;

		}
		else
		{
			_skillBind.Text = "";
		}

	}

	public void StartCooldown(float duration)
	{
		
		_totalCooldown = duration;
		_currentCooldown = duration;
		_cooldownProgress.MaxValue = duration;

		_cooldownProgress.Visible = true;
		_cooldownLabel.Visible = true;

		IsOnCooldown = true;

	}

	public override void _Process(double delta)
	{
		
		if (IsOnCooldown)
		{
			_currentCooldown -= (float)delta;
			_cooldownProgress.Value = _currentCooldown;

			_cooldownLabel.Text = _currentCooldown > 1.0f ? $"{Mathf.CeilToInt(_currentCooldown)}" : $"{_currentCooldown:0.0}";

			if (_currentCooldown <= 0)
			{
				IsOnCooldown = false;
				_cooldownLabel.Visible = false;
				_cooldownProgress.Visible = false;

			}

		}

	}



}
