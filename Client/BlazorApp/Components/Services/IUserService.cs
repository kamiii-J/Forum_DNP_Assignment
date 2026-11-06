using ApiContracts;

namespace BlazorApp.Components.Services;

public interface IUserService
{
    public Task<UserDto> AddUserAsync(CreateUserDto request);
    public Task UpdateUserAsync(int id, UpdateUserDto request);
    public Task<IEnumerable<UserDto>> GetUserAsync(int? id = null);
}