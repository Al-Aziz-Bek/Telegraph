using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using blog2.Data;
using blog2.Entities;
using blog2.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace blog2.Controllers;

public class BlogsController : Controller
{
    private readonly ILogger<BlogsController> _logger;
    private readonly BlogAppDbContext _context;
    private readonly UserManager<User> _userManager;

    public BlogsController(ILogger<BlogsController> logger, BlogAppDbContext context, UserManager<User> userManager)
    {
        _logger = logger;
        _context = context;
        _userManager = userManager;
    }

    [HttpGet("blogs")]
    public async Task<IActionResult> GetBlogs()
    {
        return View(new BlogsViewModel()
        {
            Blogs = await _context.Posts.Select(p => new PostViewModel()
            {
                Id = p.Id,
                Title = p.Title,
                Content = p.Content,
                Author = p.Author,
                Tags = p.Tags
            }).ToListAsync()
        });
    }

    [Authorize]
    [HttpGet("write")]
    public IActionResult Write()
    {
        return View();
    }

    [Authorize]
    [HttpPost("write")]
    public async Task<IActionResult> Write([FromForm] PostViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest($"{ModelState.ErrorCount} errors detected.");
        }
        if (model.Edited)
        {
            var post = await _context.Posts.FirstOrDefaultAsync(p => p.Id == model.Id);
            if (post.Title == model.Title && post.Tags == model.Tags && post.Author == model.Author && post.Content == model.Content)
            {
                return LocalRedirect($"~/post/{post.Id}");
            }
            post.Title = model.Title;
            post.Tags = model.Tags;
            post.Author = model.Author;
            post.Content = model.Content;
            post.ModifiedAt = DateTimeOffset.UtcNow;
            _context.Posts.Update(post);
            await _context.SaveChangesAsync();

            return LocalRedirect($"~/post/{post.Id}");
        }
        var userId = _userManager.GetUserId(User);
        var newPost = new Post(model.Title, model.Content, model.Author, string.Join(',', model.Tags), Guid.Parse(userId));

        await _context.Posts.AddAsync(newPost);
        await _context.SaveChangesAsync();

        return LocalRedirect($"~/post/{newPost.Id}");
    }

    [HttpGet("post/{id}")]
    public async Task<IActionResult> Post(Guid id)
    {
        var post = await _context.Posts.FirstOrDefaultAsync(p => p.Id == id);
        var model = new PostViewModel()
        {
            Id = post.Id,
            Title = post.Title,
            Content = post.Content,
            Author = post.Author,
            Tags = post.Tags,
            Edited = post.Edited,
            Claps = post.Claps,
            CreatedAt = post.CreatedAt
        };
        return View(model);
    }

    [Authorize]
    [HttpGet("edit/{id}")]
    public async Task<IActionResult> Edit(Guid id)
    {
        var post = await _context.Posts.FirstOrDefaultAsync(p => p.Id == id);
        var model = new PostViewModel()
        {
            Id = post.Id,
            Title = post.Title,
            Content = post.Content,
            Author = post.Author,
            Tags = post.Tags,
            Edited = true,
            Claps = post.Claps,
            CreatedAt = post.CreatedAt
        };
        return View("write", model);
    }
}