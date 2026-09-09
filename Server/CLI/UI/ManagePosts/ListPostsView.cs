using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class ListPostsView
{
    private readonly IPostRepository postRepository;
    
    public ListPostsView(IPostRepository postRepository)
    {
        this.postRepository = postRepository;
    }

    public Task ShowAsync()
    {
        var posts = postRepository.GetMany().ToList();
        Console.WriteLine("\n--- Posts Overview ---");
        foreach (var post in posts)
        {
            Console.WriteLine($"[ID: {post.Id}] {post.Title}");
        }
        return Task.CompletedTask;
    }
}