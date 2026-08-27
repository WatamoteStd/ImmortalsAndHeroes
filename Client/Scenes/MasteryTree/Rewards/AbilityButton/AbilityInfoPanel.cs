using Godot;
using Shared.Ability;
using System;
using Shared.Ability.Params;

public partial class AbilityInfoPanel : Control
{
	
	[Export] private CategoryComponent _castTypeComponent;
	[Export] private CategoryComponent _addCastTypeComponent;
	[Export] private CategoryComponent _targetTypeComponent;
	[Export] private CategoryComponent _relationComponent;
	[Export] private CategoryComponent _interruptibleComponent;
	[Export] private CategoryComponent _moveWhileCastComponent;

	[Export] private Label _title;
	[Export] private Label _description;
	[Export] private TextureRect _icon;
	[Export] private Label _damageType;

	public AbilityTypes AbilityId {get; private set;}

	public void Initiate(AbilityData data)
	{

		if (data.AbilityId == AbilityId)
		{
			Visible = false;
			AbilityId = AbilityTypes.None;
			return;
		}

		AbilityId = data.AbilityId;
		_icon.Texture = GD.Load<Texture2D>(data.IconPath);
		_title.Text = data.Title;
		_description.Text = data.Description;
		if (data.DamageType == Shared.Characters.DamageTypes.None)
		{
			_damageType.Visible = false;
		}
		else
		{
			_damageType.Text = data.DamageType.ToString();
		}


		_castTypeComponent.SetValue(data.CastType.ToString());
		_addCastTypeComponent.SetValue(data.CastTypeAdditional.ToString());
		_targetTypeComponent.SetValue(data.TargetType.ToString());
		_relationComponent.SetValue(data.TargetRelation.ToString());

		_interruptibleComponent.SetValue(data.IsInterruptible ? "YES" : "NO");
		_moveWhileCastComponent.SetValue(data.IsMoveWhileCast ? "YES" : "NO");

		Visible = true;

	}

}
