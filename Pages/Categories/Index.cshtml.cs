using EventFind.Areas.Identity.Data;
using EventFind.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EventFind.Pages.Categories;

public class IndexModel : PageModel
{
    private readonly ApplicationDBContext _context;

    public IndexModel(ApplicationDBContext context)
    {
        _context = context;
    }

    public IList<Category> Categories { get; set; }
    public async Task OnGetAsync()
    {

        var categories = _context.Categories.AsQueryable();
        if(categories.Count() > 0)
        {
            Categories = await categories.ToListAsync();
        }
        else
        {
            Categories = new List<Category>();
        }
    }
}
