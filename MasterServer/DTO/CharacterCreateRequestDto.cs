using System;
using Shared.Characters;

namespace MasterServer.DTO;

public record CharacterCreateRequestDto
{
    
    public long UserId {get; set;}
    public string Nickname {get; set;} = null!;
    public CharacterType Type {get; set;}

}