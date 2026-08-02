using System;

namespace MasterServer.DTO;

public record RegisterRequestDto
{
    
    public string Username {get; set;} = null!;
    public string Password {get; set;} = null!;
    public string Email {get; set;} = null!;

}