using Godot;
using System;

public partial class SettingsWindow : MarginContainer
{
	[Export] private HSlider _musicSlider;
	[Export] private HSlider _soundSlider;
	private int _musicBusIndex;
	private int _soundBusIndex;
	public override void _Ready()
	{
		
		_musicBusIndex = AudioServer.GetBusIndex("Background");
		_soundBusIndex = AudioServer.GetBusIndex("UI");

		_musicSlider.ValueChanged += OnMusicValueChanged;
		_soundSlider.ValueChanged += OnSoundValueChanged;

	}
	private void OnMusicValueChanged(double value)
	{
		float linearValue = (float)value / 100.0f;
		
		float db = (float)Mathf.LinearToDb(linearValue);
		AudioServer.SetBusVolumeDb(_musicBusIndex, db);

		AudioServer.SetBusMute(_musicBusIndex, value <= 0);

	}
	private void OnSoundValueChanged(double value)
	{
		float linearValue = (float)value / 100.0f;
		
		float db = (float)Mathf.LinearToDb(linearValue);
		AudioServer.SetBusVolumeDb(_soundBusIndex, db);

		AudioServer.SetBusMute(_soundBusIndex, value <= 0);

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
