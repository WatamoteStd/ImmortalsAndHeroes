using Godot;
using System;

public partial class RegionClient : Node3D
{

	public readonly Godot.Collections.Dictionary<uint, EntityClient> _regionEntities = new();
	[Export] private PackedScene _modelPrefab;
 
	public override void _Ready()
	{
		
		NetworkUdpClient.Instance.SendEnterTheWorld(); // UDP REQUEST TO SERVER

	}
	

	public void AddEntity(uint id, Vector3 startPosition)
	{

		if (_regionEntities.ContainsKey(id)) return;
		
		var entity = _modelPrefab.Instantiate<EntityClient>();
		entity.NetworkId = id;
		entity.GlobalPosition = startPosition;
		AddChild(entity);

		_regionEntities.Add(id, entity);

	}


}
