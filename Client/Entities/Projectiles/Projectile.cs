using Godot;
using Shared.ProjectilesData;
using Shared.Udp.Packets.Category.Game.Projectile;
using System;

public partial class Projectile : Node3D
{
	
	public ushort Id {get; set;}
	public Entity TargetEntity {get; set;}
	public float Speed {get; set;}

	public void Init(in S2C_ProjectileCreatedPacket packet, Entity targetEntity, Entity casterEntity)
	{
		
		Id = packet.Id;
		TargetEntity = targetEntity;
		Speed = packet.Speed;

		ProjectileRegistry.TryGetProjectile(packet.Type, out var dllData);

		var scene = GD.Load<PackedScene>(dllData.ScenePath);
		var prj = scene.Instantiate<Node3D>();

		var container = GetNode<Node3D>("Mesh");
		container.AddChild(prj);

		GlobalPosition = casterEntity.GlobalPosition + new Vector3(0, 1.2f, 0);

	}

	public override void _Process(double delta)
	{
		
		if (!IsInstanceValid(TargetEntity))
		{
			QueueFree();
			return;
		}

		Vector3 targetPos = TargetEntity.GlobalPosition + new Vector3(0, 1.0f, 0);
		Vector3 currentPos = GlobalPosition;

		LookAt(targetPos, Vector3.Up);

		GlobalPosition = currentPos.MoveToward(targetPos, Speed * (float)delta);

	}


}
