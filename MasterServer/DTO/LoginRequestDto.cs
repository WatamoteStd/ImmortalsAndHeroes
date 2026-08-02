using System;

namespace MasterServer.DTO;

public record LoginRequestDto
{
    
    public string Username {get; set;} = null!;
    public string Password {get; set;} = null!;


}