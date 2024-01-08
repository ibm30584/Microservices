using Audit.Application.Audit.Commands.AddAuditLog;
using Audit.Contracts.Messages;
using MassTransit;
using MediatR;

namespace Audit.Worker.Consumers
{
    public class AuditConsumer : IConsumer<AuditLogMessage>
    {
        private readonly IMediator _mediator;
        private readonly ILogger<AuditConsumer> _logger;

        public AuditConsumer(IMediator mediator, ILogger<AuditConsumer> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }
        public async Task Consume(ConsumeContext<AuditLogMessage> context)
        {
            var command = MapCommand(context.Message);
            var auditLogId = await _mediator.Send(command, context.CancellationToken);
            _logger.LogInformation("Audit log created with id {createdAuditLogId}", auditLogId);
        }

        private static Command MapCommand(AuditLogMessage message)
        {
            return new Command
            {
                CreatedDate = message.CreatedDate,
                CreatedByUserId = message.CreatedByUserId,
                ServiceId = (Domain.Enums.AuditService)message.ServiceId,
                EventId = (Domain.Enums.AuditEvent)message.EventId,
                EventEntityId = message.EventEntityId,
                Metadata = message.Metadata
                    ?.ConvertAll(messageMetadatum => new Domain.Entities.AuditMetadata(
                            Key: messageMetadatum.Key, 
                            Value: messageMetadatum.Value)),
                JwtToken = message.JwtToken
            };
        }
    }
}
