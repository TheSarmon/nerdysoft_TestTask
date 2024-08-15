using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using nerdysoft_TestTask.Models;
using nerdysoft_TestTask.Controllers;

namespace nerdysoft_TestTask.Data
{
    public static class SampleData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new AnnouncementContext(
                serviceProvider.GetRequiredService<DbContextOptions<AnnouncementContext>>()))
            {
                if (context.Announcements.Any())
                {
                    return;
                }

                var announcements = new AnnouncementModel[]
                {
                new AnnouncementModel
                {
                    Title = "Grand Opening of New Coffee Shop",
                    Description = "Join us for the grand opening of our new coffee shop! Enjoy complimentary coffee and pastries.Date: August 20, 2024.",
                    DateAdded = new DateTime(2024, 7, 15, 19, 30, 0)
                },
                new AnnouncementModel
                {
                    Title = "Summer Sale - Up to 50% Off",
                    Description = "Our summer sale is here! Get up to 50% off on selected items. Visit our store from August 15 to August 25, 2024.",
                    DateAdded = new DateTime(2024, 8, 6, 18, 30, 0)
                },
                new AnnouncementModel
                {
                    Title = "Job Vacancy: Software Developer",
                    Description = "We are looking for a talented software developer to join our team. Apply now to work on exciting projects. Full-time position, competitive salary.",
                    DateAdded = new DateTime(2024, 6, 10, 10, 30, 0)
                }
                };

                foreach (var announcement in announcements)
                {
                    context.Announcements.Add(announcement);
                }

                context.SaveChanges();
            }
        }
    }
}
