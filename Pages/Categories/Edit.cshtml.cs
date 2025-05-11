using EventFind.Areas.Identity.Data;
using EventFind.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EventFind.Pages.Categories
{
    [Authorize(Roles = "Admin")]
    public class EditModel : PageModel
    {
        private readonly ApplicationDBContext _context;

        public EditModel(ApplicationDBContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Category Category { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Category = await _context.Categories.FirstOrDefaultAsync(m => m.Id == id);

            if (Category == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Console.WriteLine("======== [DEBUG] OnPostAsync() called ========");

            // ✅ Ignore validation for unbound properties
            ModelState.Remove("Category.User");
            ModelState.Remove("Category.UserId");

            if (!ModelState.IsValid)
            {
                Console.WriteLine("[DEBUG] ModelState is INVALID");
                foreach (var key in ModelState.Keys)
                {
                    var state = ModelState[key];
                    foreach (var error in state.Errors)
                    {
                        Console.WriteLine($"[MODEL ERROR] {key}: {error.ErrorMessage}");
                    }
                }

                return Page();
            }

            Console.WriteLine($"[DEBUG] Incoming ID: {Category.Id}");
            Console.WriteLine($"[DEBUG] New name from form: {Category.Name}");

            var categoryFromDb = await _context.Categories.FirstOrDefaultAsync(c => c.Id == Category.Id);
            if (categoryFromDb == null)
            {
                Console.WriteLine($"[DEBUG] Category not found in DB.");
                return NotFound();
            }

            Console.WriteLine($"[DEBUG] Existing value from DB: {categoryFromDb.Name}");

            // ✅ Only update the name
            categoryFromDb.Name = Category.Name;

            _context.Entry(categoryFromDb).Property(c => c.Name).IsModified = true;

            try
            {
                await _context.SaveChangesAsync();
                Console.WriteLine("[DEBUG] SaveChangesAsync called.");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(Category.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.Id == id);
        }
    }
}
