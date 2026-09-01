using Godot;
using System;

public partial class StaticAbility : AbilityBase
{
	
	[Export] private GpuParticles3D _particles;

	public override void Setup(float lifeTime, float speed = 0f, Vector3 targetPos = default, Entity targetEntity = null)
	{
		if (_particles != null)
		{
			_particles.OneShot = true;
			_particles.Emitting = true;
		
			_particles.Finished += QueueFree; 
		}
		else
		{
			// Если партикла нет, удаляем по старому таймеру
			base.Setup(lifeTime, speed, targetPos, targetEntity);
		}
	}

}
