using Godot;
using System;

public partial class PlayerEntity : Entity
{
	
	[Export] protected Label _name;

	public override void InitEntity(uint id, int health, int maxHealth, string name)
	{
		base.InitEntity(id, health, maxHealth, name);
		
		if (_name != null)
			_name.Text = EntityName;
	}


}
