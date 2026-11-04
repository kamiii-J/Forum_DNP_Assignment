using ApiContracts;

namespace BlazorApp.Components.Services;

public interface IUserService
{
    public Task<UserDto> AddUserAsync(CreateUserDto request);
    public Task<UserDto> UpdateUserAsync(UpdateUserDto request);
    public Task<UserDto> GetUserAsync(int id);
    public Task<UserDto> DeleteUserAsync(int id);
}