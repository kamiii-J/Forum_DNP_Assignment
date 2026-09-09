using Entities;
using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class CreatePostView
{
    private readonly IPostRepository postRepository;
    private readonly IUserRepository userRepository;

    public CreatePostView(IPostRepository postRepository, IUserRepository userRepository)
    {
        this.postRepository = postRepository;
        this.userRepository = userRepository;
    }

    public async Task ShowAsync()
    {
        Console.Write("Enter Author User ID: ");
        if (!int.TryParse(Console.ReadLine(), out int userId))
        {
            Console.WriteLine("Error: User ID must be a number.");
            return;
        }
        
        bool userExists = userRepository.GetMany().Any(u => u.Id == userId);
        if (!userExists)
        {
            Console.WriteLine($"Error: User with ID '{userId}' does not exist. You cannot create a post for a non-existent user.");
            return;
        }
        
        Console.Write("Enter Title: ");
        string title = Console.ReadLine() ?? "";
        
        Console.Write("Enter Body: ");
        string body = Console.ReadLine() ?? "";

        var newPost = new Post { UserId = userId, Title = title, Body = body };
        await postRepository.AddAsync(newPost);
        
        Console.WriteLine($"Post created successfully with ID: {newPost.Id}");
    }
}