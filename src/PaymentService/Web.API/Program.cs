using Application.Consumers;
using MassTransit;
using SharedLib;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Configuration.AddEnvironmentVariables();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMassTransit(conf =>
{
    conf.AddConsumer<StockReservedRequestPaymentEventConsumer>();
    conf.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("s_rabbitmq", (auth) =>
        {
            auth.Username("guest");
            auth.Password("guest");
        });
        cfg.ReceiveEndpoint(QueueNames.PaymentStockReservedRequest, e =>
        {
            e.ConfigureConsumer<StockReservedRequestPaymentEventConsumer>(context);
        });
    });
});

var app = builder.Build();

app.UseSwagger();

app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
