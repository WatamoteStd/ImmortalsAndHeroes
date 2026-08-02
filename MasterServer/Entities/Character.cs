using System;

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


}