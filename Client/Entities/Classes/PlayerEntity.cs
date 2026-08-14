using Godot;
using Shared.Characters;
using System;

public partial class PlayerEntity : Entity
{

	public override void InitEntity(uint id, int health, int maxHealth, string name, EntityType type, Vector3 pos)
	{
		base.InitEntity(id, health, maxHealth, name, type, pos);
		
		if (_name != null)
			_name.Text = EntityName;
			
	}


}
