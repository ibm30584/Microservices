using Audit.Contracts.Messages;
using Codes.Application.Services.Audit;
using MassTransit;

namespace Codes.Infrastructures.Services
{
    public class AuditService : IAuditService
    {
        private readonly IBus _bus;

        public AuditService(IBus bus)
        {
            _bus = bus;
        }
        public Task Audit(AuditLogMessage auditLogMessage)
        {
            return _bus.Publish(auditLogMessage);
        }
    }
}
