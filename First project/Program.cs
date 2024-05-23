using Microsoft.EntityFrameworkCore;
using First_project.Models;
// Test Commit.

namespace First_project
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<ModelContext>(x => x.UseOracle(builder.Configuration.GetConnectionString("RecipeBlog")));
            builder.Services.AddSession(options =>
            { 
                //how to much time it will keep session's data.
                options.IdleTimeout = TimeSpan.FromSeconds(30);
            });
            builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

            builder.Services.AddHttpContextAccessor();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSession();
            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}