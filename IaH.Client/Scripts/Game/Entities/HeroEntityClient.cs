using Godot;
using Iah.Shared.Packets;
using System;

public partial class HeroEntityClient : BaseEntityClient
{
	public bool IsLocalPlayer = false;

	public override void _Ready()
	{
		base._Ready();
		
	}
	public override void _Process(double delta)
	{
		base._Process(delta);
	}




}
