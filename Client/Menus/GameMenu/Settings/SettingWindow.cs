using Godot;
using System.Collections.Generic;

public partial class SettingWindow : Control
{
    [Export] private CheckBox _attackSpace;
    [Export] private Label _attackSpaceLabel;
    [Export] private CheckBox _attackLmb;
    [Export] private Label _attackLmbLabel;
	[Export] private CheckBox _overheatingBox;
    [Export] private Label _overheatingLabel;

	[Export] private Button _closeWindow;

    private readonly Dictionary<CheckBox, Label> _settingToLabel = new();

    private readonly Color _activeColor = new(0.843f, 0.824f, 0.812f);
    private readonly Color _disabledColor = new(0.506f, 0.525f, 0.506f);

    public override void _Ready()
    {
        _settingToLabel[_attackSpace] = _attackSpaceLabel;
        _settingToLabel[_attackLmb] = _attackLmbLabel;
		_settingToLabel[_overheatingBox] = _overheatingLabel;

        foreach (var (checkBox, label) in _settingToLabel)
        {
            checkBox.Toggled += (isToggled) => UpdateSettingState(checkBox, isToggled);

            UpdateSettingState(checkBox, checkBox.ButtonPressed);
        }


		_closeWindow.Pressed += () => {Visible = false;};
    }

    private void UpdateSettingState(CheckBox checkBox, bool isToggled)
    {
        if (_settingToLabel.TryGetValue(checkBox, out Label label))
        {
            label.AddThemeColorOverride("font_color", isToggled ? _activeColor : _disabledColor);

			if (checkBox == _attackLmb)
			{
				SettingsManager.Instance.AttackOnFirstLmb = isToggled;
			}

        }
    }
}