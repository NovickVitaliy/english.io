using Learning.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Learning.Infrastructure.Database.Configurations;

public class WordProgressHistoryConfiguration : IEntityTypeConfiguration<WordProgressHistory>
{
    public void Configure(EntityTypeBuilder<WordProgressHistory> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.UserEmail).IsRequired().HasMaxLength(256);
        builder.Property(x => x.Task).HasConversion<string>();
        builder.HasIndex(x => new { x.UserEmail, x.PracticedAt });
        builder.HasIndex(x => new { x.UserEmail, x.SenseId });
    }
}