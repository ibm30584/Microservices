using Codes.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Codes.Infrastructures.Services.Persistence.Configuration
{
    public class CodeTypeConfig : IEntityTypeConfiguration<CodeType>
    {
        public void Configure(EntityTypeBuilder<CodeType> builder)
        {
            builder.HasKey(x => x.CodeTypeId);
            builder.Property(x => x.CodeTypeId).ValueGeneratedOnAdd();
            builder.Property(x => x.Value).IsUnicode().HasMaxLength(50).IsRequired();
            builder.Property(x => x.Text).IsUnicode().HasMaxLength(256).IsRequired();
            builder.Property(x => x.Text2).IsUnicode().HasMaxLength(256);
            builder.Property(x => x.CreatedDate).IsRequired();
            builder.Property(x => x.CreatedByUserId).IsRequired();

            builder.HasIndex(x => x.Value).IsUnique();

            builder
                .HasMany(x => x.Codes)
                .WithOne(x => x.CodeType)
                .HasForeignKey(x => x.CodeTypeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
