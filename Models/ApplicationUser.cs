using Microsoft.AspNetCore.Identity;

namespace EventFind.Models;

public class ApplicationUser : IdentityUser
{
    public string FullName { get; set; }
}
