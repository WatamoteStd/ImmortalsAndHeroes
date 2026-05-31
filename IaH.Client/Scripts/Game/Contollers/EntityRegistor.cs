using Godot;
using Iah.Shared.Packets;
using System;
using System.Collections.Generic;

public partial class EntityRegistor : Node
{
	
	// CARRY & DD & RDD
	[Export] private PackedScene FrozenModel;
	[Export] private PackedScene OzonidModel;

	// MAGE 
	[Export] private PackedScene VoidlessStarModel;
	[Export] private PackedScene LilithModel;

	// TANK
	[Export] private PackedScene WhipModel;

	public PackedScene GetModel(UnitList unit)
	{
		
		return unit switch
		{
			UnitList.Frozen => FrozenModel,
			UnitList.VoidlessStar => VoidlessStarModel,
			UnitList.Whip => WhipModel,
			UnitList.Lilith => LilithModel,
			UnitList.Ozonid => OzonidModel,
			_ => null
		};

	}



	
}
