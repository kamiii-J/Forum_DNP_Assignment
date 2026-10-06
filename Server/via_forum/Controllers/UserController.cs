using ApiContracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace via_forum.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserRepository userRepo;

    public UsersController(IUserRepository userRepo)
    {
        this.userRepo = userRepo;
    }

    [HttpPost]
    public async Task<ActionResult<UserDto>> AddUser([FromBody] CreateUserDto request)
    {
        try
        {
            //Unique username check
            bool exists = userRepo.GetMany().Any(u => u.UserName.Equals(request.UserName, StringComparison.OrdinalIgnoreCase));
            if (exists) return BadRequest("Username is already taken.");

            User user = new User { UserName = request.UserName, password = request.Password };
            User created = await userRepo.AddAsync(user);
            
            UserDto dto = new UserDto { Id = created.Id, UserName = created.UserName };
            return Created($"/Users/{dto.Id}", dto);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpGet]
    public ActionResult<IEnumerable<UserDto>> GetUsers([FromQuery] string? userNameContains)
    {
        try
        {
            IQueryable<User> users = userRepo.GetMany();

            if (!string.IsNullOrWhiteSpace(userNameContains))
            {
                users = users.Where(u => u.UserName.Contains(userNameContains, StringComparison.OrdinalIgnoreCase));
            }

            List<UserDto> userDtos = users.Select(u => new UserDto 
            { 
                Id = u.Id, 
                UserName = u.UserName 
            }).ToList();

            return Ok(userDtos);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> GetSingle(int id)
    {
        try
        {
            User user = await userRepo.GetSingleAsync(id);
            return Ok(new UserDto { Id = user.Id, UserName = user.UserName });
        }
        catch (InvalidOperationException e)
        {
            return NotFound(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateUser(int id, [FromBody] UpdateUserDto request)
    {
        try
        {
            User existingUser = await userRepo.GetSingleAsync(id);

            if (!string.IsNullOrWhiteSpace(request.UserName)) existingUser.UserName = request.UserName;
            if (!string.IsNullOrWhiteSpace(request.Password)) existingUser.password = request.Password;

            await userRepo.UpdateAsync(existingUser);
            return Ok();
        }
        catch (InvalidOperationException e)
        {
            return NotFound(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteUser(int id)
    {
        try
        {
            await userRepo.DeleteAsync(id);
            return NoContent();
        }
        catch (InvalidOperationException e)
        {
            return NotFound(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
}