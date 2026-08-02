using System;

namespace MasterServer.Entities;

public class User
{
    
    public long Id {get; set;}
    public string Login {get; set;} = "";
    public string Email {get; set;} = "";
    public string PasswordHash {get; set;} = "";
    public DateTime CreatedAt {get; set;} = DateTime.UtcNow;
    public List<Character> Characters {get; set;} = new ();

}