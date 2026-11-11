using ApiContracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace via_forum.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserRepository userRepo;
    
    public AuthController(IUserRepository userRepo)
    {
        this.userRepo = userRepo;
    }

    [HttpPost("login")]
    public ActionResult<UserDto> Login([FromBody] LoginRequest request)
    {
        try
        {
            User? user = userRepo.GetMany().SingleOrDefault(u => u.UserName.Equals(request.Username, StringComparison.OrdinalIgnoreCase));

            if (user is null)
            {
                return Unauthorized("User not found");
            }

            if (user.password != request.Password)
            {
                return Unauthorized("Wrong password");
            }

            UserDto dto = new UserDto
            {
                Id = user.Id, UserName = user.UserName
            };
            return Ok(dto);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
}