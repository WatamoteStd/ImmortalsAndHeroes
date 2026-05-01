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

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
