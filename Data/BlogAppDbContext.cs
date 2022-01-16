using blog2.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace blog2.Data;

public class BlogAppDbContext : IdentityDbContext<User>
{
    public DbSet<Post> Posts { get; set; }

    public BlogAppDbContext(DbContextOptions<BlogAppDbContext> options)
        : base(options) { }
}