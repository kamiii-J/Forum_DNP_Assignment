using RepositoryContracts;

namespace CLI.UI.ManageUsers;

public class ListUsersView
{
    private readonly IUserRepository userRepository;
    
    public ListUsersView(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    public Task ShowAsync()
    {
        var users = userRepository.GetMany().ToList();
        Console.WriteLine("\n-- All Users --");
        foreach (var user in users)
        {
            Console.WriteLine($"ID: {user.Id} | Username: {user.UserName}");
        }

        return Task.CompletedTask;
    }
}