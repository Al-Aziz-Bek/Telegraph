namespace blog2.Entities;

public class Post
{
    public Guid Id { get; set; }

    public string Title { get; set; }

    public string Content { get; set; }

    public string Author { get; set; }

    public string Tags { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset ModifiedAt { get; set; }

    public ulong Claps { get; set; }

    public bool Edited => false;

    public byte[] BannerImageUrl { get; set; }

    [Obsolete("Not allowed", true)]
    public Post() { }

    public Post(string title, string content, string author, string tags, Guid createdBy)
    {
        Title = title;
        Content = content;
        Author = author;
        Tags = tags;
        CreatedBy = createdBy;

        Id = Guid.NewGuid();
        CreatedAt = DateTimeOffset.UtcNow;
        Claps = 0;
    }
}