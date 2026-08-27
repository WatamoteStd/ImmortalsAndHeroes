using Godot;
using Shared.Ability;
using System;

public partial class AbilityButton : Button
{
	
	[Export] private TextureRect _icon;
	[Export] private PanelContainer _reqLvlPanel;
	[Export] private Label _reqLvlLabel;


	private AbilityInfoPanel _infoPanel;
	private Color _defaultColor = new Color(0.694f, 0.694f, 0.694f);
	private Color _hoverColor = new Color(0.788f, 0.808f, 0.816f);
	private Color _pressColor = new Color(0.589f, 0.589f, 0.589f);

	public AbilityTypes AbilityId {get; private set;}

	private AbilityData _dllData;

	public override void _Ready()
	{

		_infoPanel = GetTree().GetFirstNodeInGroup("Interface") as AbilityInfoPanel;

		if (_infoPanel == null)
        {
            GD.PrintErr($"[AbilityButton] Панель с группой 'Interface' не найдена в сцене!");
        }
		
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
			_icon.SelfModulate = _hoverColor;
		};

		Pressed += () =>
		{
			_infoPanel.Initiate(_dllData);
		};

	}
	public void Init(AbilityData data, int requiredLvl, bool isLvlRequired = false)
	{
		
		_icon.Texture = GD.Load<Texture2D>(data.IconPath);
		_icon.SelfModulate = _defaultColor;
		_dllData = data;

		if (isLvlRequired)
		{
			
			_reqLvlPanel.Visible = true;
			_reqLvlLabel.Text = $"LVL {requiredLvl}";

		}
		else
		{
			_reqLvlPanel.Visible = false;
		}

	}




}
