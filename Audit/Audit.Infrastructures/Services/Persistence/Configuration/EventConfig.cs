using Audit.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Audit.Infrastructures.Services.Persistence.Configuration
{
    public class EventConfig : IEntityTypeConfiguration<Event>
    {
        public void Configure(EntityTypeBuilder<Event> builder)
        {
            builder.HasKey(x => x.EventId);
            builder.Property(x => x.EventId).ValueGeneratedOnAdd();
            builder.Property(x => x.Name).IsUnicode().HasMaxLength(50).IsRequired();

            builder.Property(x => x.Name).IsUnicode(false).HasMaxLength(200).IsRequired();
            builder.Property(x => x.Text).IsUnicode().HasMaxLength(500).IsRequired();
            builder.Property(x => x.Text2).IsUnicode().HasMaxLength(500).IsRequired(false);

            builder.HasIndex(x => x.Name).IsUnique();
            builder.HasIndex(x => x.Text).IsUnique();
            builder.HasIndex(x => x.Text2).IsUnique();
            builder
                .HasMany(x => x.AuditLogs)
                .WithOne(x => x.Event)
                .HasForeignKey(x => x.EventId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasData(
                new Event()
                {
                    EventId = Domain.Enums.AuditEvent.AddCodeType,
                    Name = "AddCodeType",
                    ServiceId = Domain.Enums.AuditService.CodesManagement,
                    Text = "Add Code Type",
                    Text2 = "اضافة نوع كود"
                },
                new Event()
                {
                    EventId = Domain.Enums.AuditEvent.EditCodeType,
                    Name = "EditCodeType",
                    ServiceId = Domain.Enums.AuditService.CodesManagement,
                    Text = "Edit Code Type",
                    Text2 = "تعديل نوع كود"
                },
                new Event()
                {
                    EventId = Domain.Enums.AuditEvent.DeleteCodeType,
                    Name = "DeleteCodeType",
                    ServiceId = Domain.Enums.AuditService.CodesManagement,
                    Text = "Delete Code Type",
                    Text2 = "حذف نوع كود"
                },

                new Event()
                {
                    EventId = Domain.Enums.AuditEvent.AddCode,
                    Name = "AddCode",
                    ServiceId = Domain.Enums.AuditService.CodesManagement,
                    Text = "Add Code",
                    Text2 = "اضافة كود"
                },
                new Event()
                {
                    EventId = Domain.Enums.AuditEvent.EditCode,
                    Name = "EditCode",
                    ServiceId = Domain.Enums.AuditService.CodesManagement,
                    Text = "Edit Code",
                    Text2 = "تعديل كود"
                },
                new Event()
                {
                    EventId = Domain.Enums.AuditEvent.EnableCode,
                    Name = "EnableCode",
                    ServiceId = Domain.Enums.AuditService.CodesManagement,
                    Text = "Enable Code",
                    Text2 = "تفعيل كود"
                },
                new Event()
                {
                    EventId = Domain.Enums.AuditEvent.DisableCode,
                    Name = "DisableCode",
                    ServiceId = Domain.Enums.AuditService.CodesManagement,
                    Text = "Disable Code",
                    Text2 = "تعطيل كود"
                });
        }
    }
}
