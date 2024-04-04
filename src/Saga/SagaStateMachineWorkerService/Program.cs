using MassTransit;
using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;
using SagaStateMachineWorkerService;
using SagaStateMachineWorkerService.Data;
using SagaStateMachineWorkerService.StateMachine;
using SharedLib;
using System.Reflection;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddMassTransit(cfg =>
        {
            cfg.AddSagaStateMachine<OrderStateMachine,OrderStateInstance>()
            .EntityFrameworkRepository(opt =>
            {
                opt.AddDbContext<DbContext,OrderStateDbContext>((provider , builder)=>
                {
                    builder.UseSqlServer(hostContext.Configuration.GetConnectionString("SqlConnection"),mig =>
                    {
                        mig.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name);
                    });
                });
            });

            cfg.AddBus((provider) => Bus.Factory.CreateUsingRabbitMq(busConf =>
            {
                busConf.Host(hostContext.Configuration.GetConnectionString("RabbitMq"));

                //Set Initial state via listen saga queue
                busConf.ReceiveEndpoint(QueueNames.OrderSaga, e =>
                {
                    e.ConfigureSaga<OrderStateInstance>(provider);
                });
            }));
        });
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
