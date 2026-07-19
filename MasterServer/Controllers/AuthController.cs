using Microsoft.AspNetCore.Mvc;

namespace MasterServer.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{

    private LoginDTO _testDTO = new LoginDTO("Admin", "123123");
    
    [HttpPost("login")]
    
    public IActionResult Login( [FromBody]LoginDTO data)
    {
        
        if (data.Username == _testDTO.Username && data.Password == _testDTO.Password)
        {
            return Ok();
        }
        else return Unauthorized("Invalid data.");

    }

}

public record LoginDTO(string Username, string Password);