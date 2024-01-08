using Audit.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Audit.Infrastructures.Services.Persistence.Configuration
{
    public class AuditLogConfig : IEntityTypeConfiguration<AuditLog>
    {
        public void Configure(EntityTypeBuilder<AuditLog> builder)
        {
            builder.HasKey(x => x.AuditLogId);
            builder.Property(x => x.AuditLogId).ValueGeneratedOnAdd();
           
            builder.Property(x => x.CreatedDate).IsRequired();
            builder.Property(x => x.CreatedByUserId).IsRequired();
            builder.Property(x => x.ServiceId).IsRequired();
            builder.Property(x => x.EventId).IsRequired();
            builder.Property(x => x.EventEntityId).IsUnicode().HasMaxLength(50).IsRequired();

            builder.OwnsMany(x => x.Metadata, x => x.ToJson());
        }
    }
}
