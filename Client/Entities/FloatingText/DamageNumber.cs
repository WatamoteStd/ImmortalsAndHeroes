using Godot;
using System;

public partial class DamageNumber : Node3D
{
	
	[Export] private Label3D _label;

	public void Setup(int damage, Vector3 spawnPosition)
	{
		
		GlobalPosition = spawnPosition;
		_label.Text = damage.ToString();

		var randomOffset = new Vector3
		(
			(float)GD.RandRange(-0.3, 0.3),
			(float)GD.RandRange(0.2, 0.5),
			(float)GD.RandRange(-0.3, 0.3)
		);

		Vector3 targetPos = GlobalPosition + randomOffset + new Vector3(0, 2.5f, 0);

		Tween tween = CreateTween().SetParallel(true);

		tween.TweenProperty(this, "global_position", targetPos, 1f)
			.SetTrans(Tween.TransitionType.Cubic)
			.SetEase(Tween.EaseType.Out);

		
		tween.TweenProperty(_label, "modulate:a", 0.0f, 1f)
			.SetTrans(Tween.TransitionType.Quad)
			.SetEase(Tween.EaseType.In);

		
		tween.Chain().TweenCallback(Callable.From(QueueFree));

	}

}
