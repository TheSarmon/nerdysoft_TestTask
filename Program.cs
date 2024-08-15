using Microsoft.EntityFrameworkCore;
using nerdysoft_TestTask.Data;

namespace Sprint_16
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<AnnouncementContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<AnnouncementContext>();
                    context.Database.Migrate();

                    SampleData.Initialize(services);
                }
                catch (InvalidOperationException ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogWarning(ex, "Seeding was skipped because the database already contains data.");
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while seeding the database.");
                }
            }
            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Announcements}/{action=Index}/{id?}");

            app.MapGet("/", () => Results.Redirect("/Announcements/Index"));

            app.Run();
        }
    }
}