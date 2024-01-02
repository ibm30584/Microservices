using Audit.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Audit.Infrastructures.Services.Persistence.Configuration
{
    public class AuditConfig : IEntityTypeConfiguration<AuditLog>
    {
        public void Configure(EntityTypeBuilder<AuditLog> builder)
        {
            builder.HasKey(x => x.AuditLogId);
            builder.Property(x => x.AuditLogId).ValueGeneratedOnAdd();
            builder.Property(x => x.Description).IsUnicode().HasMaxLength(50).IsRequired();

            builder.Property(x => x.CreatedDate).IsRequired();
            builder.Property(x => x.CreatedByUserId).IsRequired();

            //builder.HasIndex(x => x.Value).IsUnique();
            //builder
            //   .HasMany(x => x.Children)
            //   .WithOne(x => x.Parent)
            //   .HasForeignKey(x => x.ParentId)
            //   .OnDelete(DeleteBehavior.Restrict);

            builder.OwnsMany(x => x.Metadata, x => x.ToJson());
        }
    }
}
