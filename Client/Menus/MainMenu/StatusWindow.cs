using Godot;
using System;

public partial class StatusWindow : PanelContainer
{
	
	[Export] private Label _statusName;
	[Export] private Label _statusMessage;

	private Tween _tween;

	public async void ShowMessage(string header, string message)
	{
		
		Visible = true;
		Modulate = new Color(Modulate.R, Modulate.G, Modulate.B, 1.0f);

		_statusName.Text = header;
		_statusMessage.Text = message;

		if (_tween != null && _tween.IsValid())
		{
			if(_tween.IsRunning()) 
			{
				_tween.Kill();
			}
		}

		_tween = CreateTween();
		_tween.TweenProperty(this, "modulate:a", 0.0f, 3.0);

		await ToSignal(_tween, Tween.SignalName.Finished);

		Visible = false;


	}

}
