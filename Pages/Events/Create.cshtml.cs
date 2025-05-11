using EventFind.Areas.Identity.Data;
using EventFind.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EventFind.Pages.Events;

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
    public Event Event { get; set; }
    public List<SelectListItem> Categories { get; set; }

    public async Task OnGetAsync()
    {
        // Fetch categories for the dropdown
        Categories = await _context.Categories
            .Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            })
            .ToListAsync();
    }
    public async Task<IActionResult> OnPostAsync()
    {
        ModelState.Remove("Event.Category");
        ModelState.Remove("Event.User");
        if (!ModelState.IsValid)
        {
            Categories = await _context.Categories
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                })
                .ToListAsync();
            return Page();
        }

        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            ModelState.AddModelError("", "User not found.");
            Categories = await _context.Categories
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                })
                .ToListAsync();
            return Page();
        }

        Event.User_ID = user.Id;

        // Ensure the selected Category_ID exists
        if (!await _context.Categories.AnyAsync(c => c.Id == Event.Category_ID))
        {
            ModelState.AddModelError("Event.Category_ID", "Invalid category selected.");
            Categories = await _context.Categories
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                })
                .ToListAsync();
            return Page();
        }

        _context.Events.Add(Event);
        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}
