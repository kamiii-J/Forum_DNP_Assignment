namespace ApiContracts;

public class UserDto
{
    public int Id { get; set; }
    public required string UserName { get; set; }
}

public class CreateUserDto
{
    public required string UserName { get; set; }
    public required string Password { get; set; }
}

public class UpdateUserDto
{
    public string? UserName { get; set; }
    public string? Password { get; set; }
}
