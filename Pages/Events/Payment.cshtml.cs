using EventFind.Areas.Identity.Data;
using EventFind.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace EventFind.Pages.Events;

public class PaymentModel : PageModel
{
    private readonly ApplicationDBContext _context;

    public PaymentModel(ApplicationDBContext context)
    {
        _context = context;
    }

    public Event Event { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        Event = await _context.Events.FirstOrDefaultAsync(e => e.Id == id);
        if (Event == null)
        {
            return NotFound();
        }

        return Page();
    }
}
