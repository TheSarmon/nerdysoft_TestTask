using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using nerdysoft_TestTask.Models;

namespace nerdysoft_TestTask.EntityTypeConfiguration
{
    public class AnnouncementEntityConfiguration : IEntityTypeConfiguration<AnnouncementModel>
    {
        public void Configure(EntityTypeBuilder<AnnouncementModel> builder)
        {
            builder.HasKey(a => a.Id);

            builder.Property(a => a.Title).IsRequired().HasMaxLength(200);

            builder.Property(a => a.Description).IsRequired().HasMaxLength(1000);

            builder.Property(a => a.DateAdded).IsRequired().HasDefaultValueSql("GETDATE()");
        }
    }
}
