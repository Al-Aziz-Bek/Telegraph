namespace blog2.Entities;

public class Comment
{


    public Guid Id { get; set; }

    public string Author { get; set; }

    public string Content { get; set; }

    public Guid PostId { get; set; }

    public Comment(string author, string content, Guid postId)
    {
        Author = author;
        Content = content;
        PostId = postId;
    }
}