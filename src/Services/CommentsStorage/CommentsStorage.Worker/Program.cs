using CommentsStorage.Worker;
using CommentsStorage.Worker.Configurations;
using MassTransit;

Microsoft.Extensions.Hosting.IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostingContext, services) =>
    {
        services.AddHostedService<Worker>();
        services.AddMassTransit(configurator =>
        {
            configurator.UsingRabbitMq((context, cfg) =>
            {
                var rabbitMqConfig = hostingContext.Configuration.GetSection("RabbitMq").Get<RabbitMqConfig>(); 

                cfg.Host(new Uri(rabbitMqConfig.Hostname), configure =>
                {
                    configure.Username(rabbitMqConfig.User);
                    configure.Password(rabbitMqConfig.Password);
                });
            });
        });
        services.AddMassTransitHostedService();
    })
    .Build();

await host.RunAsync();
