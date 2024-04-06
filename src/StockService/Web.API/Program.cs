using Application.Consumers;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using CoreLib.Configurations;
using Domain.Concrete;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Persistence;
using SharedLib;
using Web.API.Configurations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder => containerBuilder.RegisterModule(new CoreModule(builder.Configuration)));
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder => containerBuilder.RegisterModule(new StockModule(builder.Configuration)));
builder.Configuration.AddEnvironmentVariables();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMassTransit(conf =>
{
    conf.AddConsumer<OrderCreatedEventConsumer>();
    conf.AddConsumer<StockRollbackConsumer>();
    conf.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("s_rabbitmq", (auth) =>
        {
            auth.Username("guest");
            auth.Password("guest");
        });

        cfg.ReceiveEndpoint(QueueNames.StockOrderCreated, e =>
        {
            e.ConfigureConsumer<OrderCreatedEventConsumer>(context);
        });
        cfg.ReceiveEndpoint(QueueNames.StockRollback, e =>
        {
            e.ConfigureConsumer<StockRollbackConsumer>(context);
        });
    });
});

var app = builder.Build();

var context = app.Services.GetRequiredService<StockDbContext>();
if (context.Database.GetPendingMigrations().Any())
{
    await context.Database.MigrateAsync();
}
if (!context.Stocks.Any())
{
    await context.Stocks.AddRangeAsync(new List<Stock>
    {
        new Stock { ProductId = 1, Count = 100},
        new Stock { ProductId = 2, Count = 200},
        new Stock { ProductId = 3, Count = 300},
    });

    await context.SaveChangesAsync();
}

app.UseSwagger();

app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
