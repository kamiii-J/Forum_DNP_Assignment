using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class ManagePostsView
{
    private readonly IPostRepository postRepository;
    private readonly ICommentRepository commentRepository;
    private readonly IUserRepository userRepository;
    
    public ManagePostsView(IPostRepository postRepository, ICommentRepository commentRepository, IUserRepository userRepository)
    {
        this.postRepository = postRepository;
        this.commentRepository = commentRepository;
        this.userRepository = userRepository;
    }

    public async Task ShowAsync()
    {
        while (true)
        {
            Console.WriteLine("\n--- Manage Posts ---");
            Console.WriteLine("1. Create new post");
            Console.WriteLine("2. View posts overview");
            Console.WriteLine("3. View specific post (and comments)");
            Console.WriteLine("4. Back to main menu");
            Console.Write("Select an option: ");

            string? input = Console.ReadLine();

            if (input == "1") await new CreatePostView(postRepository, userRepository).ShowAsync();
            else if (input == "2") await new ListPostsView(postRepository).ShowAsync();
            else if (input == "3") await new SinglePostView(postRepository, commentRepository, userRepository).ShowAsync();
            else if (input == "4") break;
        }
    }
}