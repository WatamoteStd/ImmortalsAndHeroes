using Godot;

public partial class River : MeshInstance3D
{
	[Export] public float ScrollSpeed = 0.05f;

	public override void _Process(double delta)
	{
		// Получаем материал. Важно: убедись, что это StandartMaterial3D
		var mat = GetActiveMaterial(0) as StandardMaterial3D;
		
		if (mat != null)
		{
			// Увеличиваем смещение по времени
			float time = (float)Time.GetTicksMsec() / 1000.0f;
			Vector2 offset = new Vector2(time * ScrollSpeed, time * (ScrollSpeed * 0.5f));
			
			// Применяем смещение к текстуре
			mat.Uv1Offset = new Vector3(offset.X, offset.Y, 0);
		}
	}
}
