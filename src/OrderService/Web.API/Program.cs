using Application.Consumers;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using CoreLib.Configurations;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Order.API.Configurations;
using Persistence;
using SharedLib;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder => containerBuilder.RegisterModule(new CoreModule(builder.Configuration)));
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder => containerBuilder.RegisterModule(new OrderModule(builder.Configuration)));
builder.Configuration.AddEnvironmentVariables();
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMassTransit(conf =>
{
    conf.AddConsumer<OrderRequestFailedEventConsumer>();
    conf.AddConsumer<OrderRequestCompletedEventConsumer>();
    conf.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration.GetConnectionString("RabbitMq"));

        cfg.ReceiveEndpoint(QueueNames.OrderRequestFailed, e =>
        {
            e.ConfigureConsumer<OrderRequestFailedEventConsumer>(context);
        });

        cfg.ReceiveEndpoint(QueueNames.OrderRequestCompleted, e =>
        {
            e.ConfigureConsumer<OrderRequestCompletedEventConsumer>(context);
        });
    });
});

var app = builder.Build();

var context = app.Services.GetService<OrderDbContext>();
if (context.Database.GetPendingMigrations().Any())
{
    await context.Database.MigrateAsync();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
