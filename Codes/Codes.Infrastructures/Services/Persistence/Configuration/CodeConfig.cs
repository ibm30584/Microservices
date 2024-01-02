using Codes.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Codes.Infrastructures.Services.Persistence.Configuration
{
    public class CodeConfig : IEntityTypeConfiguration<Code>
    {
        public void Configure(EntityTypeBuilder<Code> builder)
        {
            builder.HasKey(x => x.CodeId);
            builder.Property(x => x.CodeId).ValueGeneratedOnAdd();
            builder.Property(x => x.Value).IsUnicode().HasMaxLength(50).IsRequired();
            builder.Property(x => x.Text).IsUnicode().HasMaxLength(256).IsRequired();
            builder.Property(x => x.Value).IsUnicode().HasMaxLength(50).IsRequired();
            builder.Property(x => x.CreatedDate).IsRequired();
            builder.Property(x => x.CreatedByUserId).IsRequired();

            builder.HasIndex(x => x.Value).IsUnique();

            builder.OwnsMany(x => x.Metadata, x => x.ToJson());
        }
    }
}
