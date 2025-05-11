using EventFind.Areas.Identity.Data;
using EventFind.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EventFind.Pages.Events;

public class EditModel : PageModel
{
    private readonly ApplicationDBContext _context;

    public EditModel(ApplicationDBContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Event Event { get; set; }
    public List<SelectListItem> Categories { get; set; }
    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        Event = await _context.Events.Include(e => e.Category).Include(e => e.User).FirstOrDefaultAsync(m => m.Id == id);
        if (Event == null)
        {
            return NotFound();
        }

        // Populate Categories dropdown
        Categories = await _context.Categories
            .Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name // Assuming Category has a Name property
            })
            .ToListAsync();
        return Page();
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
                Text = c.Name // Assuming Category has a Name property
            })
            .ToListAsync();
            return Page();
        }

        _context.Attach(Event).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!EventExists(Event.Id))
            {
                return NotFound();
            }
            else
            {
                Categories = await _context.Categories
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                })
                .ToListAsync();
                throw;
            }
        }

        return RedirectToPage("./Index");
    }

    private bool EventExists(int id)
    {
        return _context.Events.Any(e => e.Id == id);
    }
}
