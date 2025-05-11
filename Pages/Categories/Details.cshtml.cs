using EventFind.Areas.Identity.Data;
using EventFind.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace EventFind.Pages.Categories;

public class DetailsModel : PageModel
{
    private readonly ApplicationDBContext _context;

    public DetailsModel(ApplicationDBContext context)
    {
        _context = context;
    }

    public Category Category { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        Category = await _context.Categories
            .Include(e => e.User)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (Category == null)
        {
            return NotFound();
        }
        return Page();
    }
}
