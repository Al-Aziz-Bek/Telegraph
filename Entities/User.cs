using Microsoft.AspNetCore.Identity;

namespace blog2.Entities;

public class User : IdentityUser
{
    public string Fullname { get; set; }

    public DateTimeOffset Birthdate { get; set; }
}