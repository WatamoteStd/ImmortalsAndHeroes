using Godot;
using System;

public partial class GameData : Node
{
	public static GameData Instance { get; private set; }	
	public ushort MyLocalPeer;
	public short MyLocalId = -1;
	public override void _Ready()
	{
		Instance = this;
	}

	
	public override void _Process(double delta)
	{
	}
}
