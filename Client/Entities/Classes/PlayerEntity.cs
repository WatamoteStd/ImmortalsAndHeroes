using Godot;
using Shared.Characters;
using System;

public partial class PlayerEntity : Entity
{
	
	[Export] protected Label _name;

	public override void InitEntity(uint id, int health, int maxHealth, string name, EntityType type)
	{
		base.InitEntity(id, health, maxHealth, name, type);
		
		if (_name != null)
			_name.Text = EntityName;
			
	}


}
