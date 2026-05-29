using Godot;
using System;
using System.Collections.Generic;
using Iah.Shared.Packets;

public partial class SearchGamePanel : PanelContainer
{
	[Export] private Button RankedButton;
	[Export] private Button NormalButton;
	[Export] private Button _searchGameButton;
	private Godot.Collections.Array<Button> _buttons;
	[Export] private VBoxContainer _playerCountContainer;
	[Export] private MarginContainer _seartchTimePanel;
	private Button _selectedButton;
	public override void _Ready()
	{
		_buttons = new Godot.Collections.Array<Button>()
		{
			RankedButton,
			NormalButton
		};
		
		foreach (var btn in _buttons)
		{
			btn.Pressed += () => SelectGameMode(btn);
		}

		_searchGameButton.Pressed += () =>
		{
			if (_selectedButton == null) return;
			EventBus.Instance.PublishJoinQueueRequested(PacketType.JoinQueue);

			// TIMER PANEL

			_seartchTimePanel.Modulate = new Color(1, 1, 1, 0);
			_seartchTimePanel.Visible = true;
			var tween = CreateTween();
			tween.TweenProperty(_seartchTimePanel, "modulate:a", 0.85f, 0.35f);
			Visible = false;
			

		};
	}

	private void SelectGameMode(Button mode)
	{
		
		foreach (var button in _buttons)
		{
			
			if (button == mode)
			{
				var tween = CreateTween();
				tween.TweenProperty(button, "modulate", Colors.White, 0.2f);
				_selectedButton = button;
			}
			
			else
			{
				var tween = CreateTween();
				tween.TweenProperty(button, "modulate", new Color(0.5f, 0.5f, 0.5f), 0.2f);
			}

		}
		if (!_playerCountContainer.Visible) _playerCountContainer.Visible = true;

	}

}
