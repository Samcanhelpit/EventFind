using EventFind.Areas.Identity.Data;
using EventFind.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace EventFind.Pages.Events;

public class DetailsModel : PageModel
{
    private readonly ApplicationDBContext _context;

    public DetailsModel(ApplicationDBContext context)
    {
        _context = context;
    }

    public Event Event { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        Event = await _context.Events.Include(e => e.User).Include(e=>e.Category).FirstOrDefaultAsync(m => m.Id == id);

        if (Event == null)
        {
            return NotFound();
        }
        return Page();
    }
}
