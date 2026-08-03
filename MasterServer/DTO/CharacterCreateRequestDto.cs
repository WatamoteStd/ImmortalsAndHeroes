using System;

namespace MasterServer.DTO;

public record CharacterCreateRequestDto
{
    
    public string Nickname {get; set;} = null!;
    public int Type {get; set;}
    public int StartSkillType {get; set;}

}