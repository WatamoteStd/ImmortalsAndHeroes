using System;
using Shared.Characters;

namespace MasterServer.DTO;

public record CharacterCreateRequestDto
{
    
    public string Nickname {get; set;} = null!;
    public EntityType Type {get; set;}

}