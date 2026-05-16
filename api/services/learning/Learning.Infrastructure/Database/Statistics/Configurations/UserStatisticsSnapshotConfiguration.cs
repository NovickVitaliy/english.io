using Learning.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Learning.Infrastructure.Database.Configurations;

public class UserStatisticsSnapshotConfiguration : IEntityTypeConfiguration<UserStatisticsSnapshot>
{
    public void Configure(EntityTypeBuilder<UserStatisticsSnapshot> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.UserEmail).IsRequired().HasMaxLength(256);
        builder.Property(x => x.Date).IsRequired();
        builder.HasIndex(x => new { x.UserEmail, x.Date });
    }
}
