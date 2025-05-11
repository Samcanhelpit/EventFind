using System.ComponentModel.DataAnnotations;

namespace EventFind.Models;

public class Category
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    public string UserId { get; set; }
    public ApplicationUser User { get; set; }
}
