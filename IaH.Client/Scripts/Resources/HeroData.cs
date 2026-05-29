using Godot;
using Iah.Shared.Packets;
using System;
using System.Collections.Generic;

[GlobalClass]
public partial class HeroData : Resource
{
	
	[Export] public string Name;
	[Export] public PackedScene Model;
	[Export] public PackedScene ProjectileModel;
	[Export] public Godot.Collections.Array<AbilityType> Abilities;

}
