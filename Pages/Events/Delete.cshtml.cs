using EventFind.Areas.Identity.Data;
using EventFind.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace EventFind.Pages.Events;

public class DeleteModel : PageModel
{
    private readonly ApplicationDBContext _context;

    public DeleteModel(ApplicationDBContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Event Event { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        Event = await _context.Events.Include(e => e.Category).FirstOrDefaultAsync(m => m.Id == id);

        if (Event == null)
        {
            return NotFound();
        }
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        Event = await _context.Events.FindAsync(Event.Id);

        if (Event != null)
        {
            _context.Events.Remove(Event);
            await _context.SaveChangesAsync();
        }

        return RedirectToPage("./Index");
    }
}
