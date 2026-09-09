using RepositoryContracts;

namespace CLI.UI.ManageUsers;

public class ManageUsersView
{
    private readonly IUserRepository userRepository;
    
    public ManageUsersView(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    public async Task ShowAsync()
    {
        while (true)
        {
            Console.WriteLine("\n-- Manage Users --");
            Console.WriteLine("1. Create new user");
            Console.WriteLine("2. List all users");
            Console.WriteLine("3. Back to main menu");
            Console.Write("Select an option: ");

            string? input = Console.ReadLine();

            if (input == "1")
            {
                await new CreateUserView(userRepository).ShowAsync();
            }
            else if (input == "2")
            {
                await new ListUsersView(userRepository).ShowAsync();
            }
            else if (input == "3") break;
        }
    }
}