using Godot;
using System;

public partial class MenuAudioManager : AudioStreamPlayer2D
{
	[Export] public AudioStream HoverSound;
	[Export] public AudioStream ClickSound;
	public override void _Ready()
	{
		
		foreach (var node in GetTree().GetNodesInGroup("Buttons"))
		{
			
			if (node is Button btn)
			{
				btn.MouseEntered += () => PlaySound(HoverSound);
				btn.Pressed += () => PlaySound(ClickSound);
			}

		}
	}

	private void PlaySound(AudioStream sound)
	{
		if (IsInsideTree() && !IsQueuedForDeletion()) {
		if (sound == null) return;
		PitchScale = (float)GD.RandRange(3.8, 4.2);
		Stream = sound;
		Play();
		}
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
