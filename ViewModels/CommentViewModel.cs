namespace blog2.ViewModels;

public class CommentViewModel
{
    public string Author { get; set; }

    public string Description { get; set; }

    public Guid PostId { get; set; }
}