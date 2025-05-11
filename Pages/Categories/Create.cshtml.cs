using EventFind.Areas.Identity.Data;
using EventFind.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
namespace EventFind.Pages.Categories;

public class CreateModel : PageModel
{
    private readonly ApplicationDBContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public CreateModel(ApplicationDBContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [BindProperty]
    public Category Category { get; set; }

    public async Task<IActionResult> OnPostAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        Category.UserId = user.Id;

        _context.Categories.Add(Category);
        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}
