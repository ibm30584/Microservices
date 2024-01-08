using Audit.Application.DependencyInjection;
using Audit.Application.Models;
using Audit.Infrastructures.DependencyInjection;
using Core.Infrastructures.DependencyInjection;
using Core.Processor.DependencyInjection;
using Core.Worker.Models;

var builder = Host.CreateApplicationBuilder(args);

ILogger logger = null!;
try
{
    var workerOptions = new WorkerOptions
    {
        Name = "Audit Service",
        Builder = builder,
        Configuration = builder.Configuration
    };
    var auditConfiguration = AuditConfiguration.CreateFrom(builder.Configuration, "AuditConsumer");


    logger = builder.Services.AddWorker(workerOptions);
    builder.Services.AddApplication();
    builder.Services.AddInfrastructure(
        auditConfiguration,
        typeof(Program).Assembly);

    //builder.Services.AddMassTransit(x =>
    //{
    //    var uri = new Uri("rabbitmq://localhost:5672/");
    //    x.UsingRabbitMq((bus, rabbitMQ) =>
    //    {
    //        rabbitMQ.Host(
    //            uri.Host,
    //            (ushort)uri.Port,
    //            "audit", x =>
    //            {
    //                x.Username("guest");
    //                x.Password("guest");
    //            });
    //    });
    //    GenericConsumerDefinition<AuditConsumer>.Configuration = new Core.Application.Models.Messaging.MessagingConsumerConfig
    //    {
    //        ConcurrentMessageLimit = 10,
    //    };

    //    x.AddConsumer(
    //        typeof(AuditConsumer),
    //        typeof(GenericConsumerDefinition<AuditConsumer>));
    //});


    var host = builder.Build();
    host.Run();
}
catch (HostAbortedException) { }
catch (Exception exp)
{
    if (logger == null)
    {
        Console.WriteLine($"Application failed with error: {exp.Message}");
    }
    else
    {
        logger.LogError(exp, "Application failed with error: {error}", exp.Message);
    }
}
finally
{
    logger?.CloseAndFlush();
}


//public class AuditConsumerDefinition : ConsumerDefinition<AuditConsumer>
//{
//    public AuditConsumerDefinition()
//    {

//    }
//}


