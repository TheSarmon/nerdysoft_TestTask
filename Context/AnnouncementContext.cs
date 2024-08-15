using Microsoft.EntityFrameworkCore;
using nerdysoft_TestTask.EntityTypeConfiguration;
using nerdysoft_TestTask.Models;

namespace nerdysoft_TestTask.Data
{
    public class AnnouncementContext : DbContext
    {
        public DbSet<AnnouncementModel> Announcements { get; set; }

        public AnnouncementContext(DbContextOptions<AnnouncementContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AnnouncementEntityConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
