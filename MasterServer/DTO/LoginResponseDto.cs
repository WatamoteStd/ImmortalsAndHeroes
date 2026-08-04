using System;

namespace MasterServer.DTO;

public record LoginResponseDto
{
    
    public long UserId {get; set;}
    public string Username {get; set;} = null!;
    public DateTime CreatedAt {get; set;}
    public string Token { get; set; } = null!;

}