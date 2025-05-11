using EventFind.Areas.Identity.Data;
using EventFind.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EventFind.Pages.Events
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDBContext _context;

        public IndexModel(ApplicationDBContext context)
        {
            _context = context;
        }

        public IList<Event> Events { get; set; }

        [BindProperty(SupportsGet = true)]
        public string CategoryFilter { get; set; }

        public List<SelectListItem> Categories { get; set; }

        public async Task OnGetAsync()
        {
            // Load category list for dropdown
            Categories = await _context.Categories
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                })
                .ToListAsync();

            // Start query
            var events = _context.Events
                .Include(e => e.Category)
                .AsQueryable();

            // Apply category filter if selected
            if (!string.IsNullOrEmpty(CategoryFilter) && int.TryParse(CategoryFilter, out int categoryId))
            {
                events = events.Where(e => e.Category_ID == categoryId);
            }

            // Sort: promoted first, then by start date
            Events = await events
                .OrderByDescending(e => e.IsPromoted)
                .ThenBy(e => e.StartDate)
                .ToListAsync();
        }
    }
}
