using Godot;
using System;

public partial class HeroCard : Button
{
	[Export] public VBoxContainer TankContainer;
	[Export] public VBoxContainer CarryContainer;
	[Export] public VBoxContainer MageContainer;
	[Export] public HeroResource Data;
	[Signal] public delegate void HeroSelectedEventHandler(HeroResource data);
	public override void _Ready()
	{
		
		if (Data != null)
		{
			GetNode<Label>("VBoxContainer/TextureRect/Label").Text = Data.Name;
		}
		MouseEntered += MouseEnter;
		MouseExited += MouseLeaved;
		Pressed += ButtonWasPressed;

	}
	private void ButtonWasPressed()
	{
		EmitSignal(SignalName.HeroSelected, Data);
	}
	
	private void MouseEnter()
	{
		Modulate = new Color("#d6d3bc");
	}
	private void MouseLeaved()
	{
		Modulate = Colors.White;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
