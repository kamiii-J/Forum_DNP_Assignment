using CLI.UI.ManagePosts;
using CLI.UI.ManageUsers;
using RepositoryContracts;

namespace CLI.UI;

public class CliApp
{
    private readonly IUserRepository userRepository;
    private readonly ICommentRepository commentRepository;
    private readonly IPostRepository postRepository;

    public CliApp(IUserRepository userRepository, ICommentRepository commentRepository, IPostRepository postRepository)
    {
        this.userRepository = userRepository;
        this.commentRepository = commentRepository;
        this.postRepository = postRepository;
    }

    public async Task StartAsync()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("-- Forum Command Line Interface --");
            Console.WriteLine("1. Manage Users");
            Console.WriteLine("2. Manage Posts");
            Console.WriteLine("3. Exit");
            Console.WriteLine("Select an option:");

            string? input = Console.ReadLine();

            if (input == "1")
            {
                var manageUserView = new ManageUsersView(userRepository);
                await manageUserView.ShowAsync();
            }
            else if (input == "2")
            {
                var managePostView = new ManagePostsView(postRepository, commentRepository, userRepository);
                await managePostView.ShowAsync();
            }
            else if (input == "3")
            {
                break;
            }
        }
    }
}