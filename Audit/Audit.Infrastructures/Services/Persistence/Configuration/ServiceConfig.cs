using Audit.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Audit.Infrastructures.Services.Persistence.Configuration
{
    public class ServiceConfig : IEntityTypeConfiguration<Service>
    {
        public void Configure(EntityTypeBuilder<Service> builder)
        {
            builder.HasKey(x => x.ServiceId);
            builder.Property(x => x.ServiceId).ValueGeneratedNever();
            builder.Property(x => x.Name).IsUnicode(false).HasMaxLength(200).IsRequired();
            builder.Property(x => x.Text).IsUnicode().HasMaxLength(500).IsRequired();
            builder.Property(x => x.Text2).IsUnicode().HasMaxLength(500).IsRequired(false);

            builder.HasIndex(x => x.Name).IsUnique();
            builder.HasIndex(x => x.Text).IsUnique();
            builder.HasIndex(x => x.Text2).IsUnique();
            builder
                .HasMany(x => x.Events)
                .WithOne(x => x.Service)
                .HasForeignKey(x => x.ServiceId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasMany(x => x.AuditLogs)
                .WithOne(x => x.Service)
                .HasForeignKey(x => x.ServiceId)
                .OnDelete(DeleteBehavior.Restrict);


            builder.HasData(
                new Service()
                {
                    ServiceId = Domain.Enums.AuditService.CodesManagement,
                    Name = "CodesManagement",
                    Text = "CodesManagement",
                    Text2 = "ادارة الاكواد"
                });
        }
    }
}
