using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using EventFind.Areas.Identity.Data;
using EventFind.Models;
using EventFind.Services;

namespace EventFind
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var connectionString = builder.Configuration.GetConnectionString("EventFindDBConnectionString") ?? throw new InvalidOperationException("Connection string 'EventFindDBConnectionString' not found.");

            builder.Services.AddDbContext<ApplicationDBContext>(options => options.UseSqlServer(connectionString));

            builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>() // Add role management
                .AddEntityFrameworkStores<ApplicationDBContext>()
                .AddDefaultTokenProviders();
            
            builder.Services.AddScoped<AdminAccountService>();
            // Add services to the container.
            builder.Services.AddRazorPages();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();
            app.MapRazorPages();

            using (var scope = app.Services.CreateScope())
            {
                var adminService = scope.ServiceProvider.GetRequiredService<AdminAccountService>();
                await adminService.SetupAdminAccountAsync();
            }

            app.Run();
        }
    }
}
