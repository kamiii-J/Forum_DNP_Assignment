
using Entities;
using RepositoryContracts;

namespace CLI.UI.ManageUsers;

public class CreateUserView
{
    private readonly IUserRepository userRepository;
    
    public CreateUserView(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    public async Task ShowAsync()
    {
        Console.Write("Enter username: ");
        string username = Console.ReadLine() ?? "";
        
        if (string.IsNullOrWhiteSpace(username))
        {
            Console.WriteLine("Username cannot be empty");
            return;
        }

        bool usernameExists = userRepository.GetMany()
            .Any(u => u.UserName.Equals(username, StringComparison.OrdinalIgnoreCase));

        if (usernameExists)
        {
            Console.WriteLine($"The username '{username}' already exists");
            return;
        }
        
        Console.Write("Enter password: ");
        string password = Console.ReadLine() ?? "";

        if (string.IsNullOrWhiteSpace(password))
        {
            Console.WriteLine("Password cannot be empty.");
            return;
        }

        var newUser = new User
        {
            UserName = username, password = password
        };
        
        await userRepository.AddAsync(newUser);
        
        Console.WriteLine($"User '{username}' created successfully with ID: {newUser.Id}");
    }
}