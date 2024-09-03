using Microsoft.AspNetCore.Identity;

namespace Core.Entities;

// App user entity (Identity framework)
public class AppUser : IdentityUser
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }

    public Address? Address { get; set; }
}
