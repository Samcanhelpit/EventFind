using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using EventFind.Models;
using EventFind.Areas.Identity.Data;

namespace EventFind.Pages
{
    [Authorize(Roles = "Admin")]
    public class DashboardModel : PageModel
    {
        private readonly ApplicationDBContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public DashboardModel(ApplicationDBContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public List<Event> Events { get; set; }
        public List<Category> Categories { get; set; }
        public List<ApplicationUser> Users { get; set; }

        [BindProperty(SupportsGet = true)]
        public string CategoryFilter { get; set; }

        [BindProperty(SupportsGet = true)]
        public string PromotionFilter { get; set; }

        public async Task OnGetAsync()
        {
            Categories = await _context.Categories.ToListAsync();
            Users = await _userManager.Users.ToListAsync();

            var query = _context.Events.Include(e => e.Category).AsQueryable();

            if (!string.IsNullOrEmpty(CategoryFilter) && int.TryParse(CategoryFilter, out int catId))
            {
                query = query.Where(e => e.Category_ID == catId);
            }

            if (!string.IsNullOrEmpty(PromotionFilter) && bool.TryParse(PromotionFilter, out bool isPromoted))
            {
                query = query.Where(e => e.IsPromoted == isPromoted);
            }

            Events = await query.ToListAsync();
        }
    }
}
