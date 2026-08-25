using System;
using Shared.Characters;

namespace MasterServer.Entities;

public class Character
{
    
    public long Id {get; set;} // global id at udpserver
    public long RegionId {get; set;}
    public string Name {get; set;} = "";
    public float PosX {get; set;}
    public float PosY {get; set;}
    public float PosZ {get; set;}
    public long UserId {get; set;}
    public User User {get; set;} = null!;

    // CHARACTER

    public EntityType Type {get; set;} = EntityType.Default;
    public float CurrentHp {get; set;} = 220;
    public float CurrentMp {get; set;} = 100;
    public float Exp {get; set;} = 0;
    public long Silver {get; set;} = 0;



}