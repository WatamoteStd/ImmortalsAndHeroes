using Godot;
using Iah.Shared.Packets;
using System;
using System.Collections.Generic;

public partial class EntityRegistor : Node
{
	[Export] public HeroData VoidlessStarConfig;
	[Export] public HeroData FrozenConfig;
	private readonly Dictionary<UnitList, HeroData> _numToConfig = new();
	
	public override void _Ready()
	{
		
		_numToConfig[UnitList.VoidlessStar] = VoidlessStarConfig;
		_numToConfig[UnitList.Frozen] = FrozenConfig;

	}
	public PackedScene GetEntity(UnitList unitType)
	{
		if (_numToConfig.TryGetValue(unitType, out HeroData config))
		{
			return config.Model;
		}
		else return null;
	}
	public HeroData GetConfig(UnitList unitType)
{
	if (_numToConfig.TryGetValue(unitType, out HeroData config))
	{
		return config; 
	}
	return null;
}

	
	
}
