using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using SagaStateMachineWorkerService.Models;

namespace SagaStateMachineWorkerService.Data;
/// <summary>
/// OrderStateDbContext has been implemented from SagaDbContext class.
/// That implementation is used for store event status to any database. Etc Mssql, Mysql, Sqlite...
/// </summary>
public class OrderStateDbContext : SagaDbContext
{
    public OrderStateDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override IEnumerable<ISagaClassMap> Configurations
    {
        get { yield return new OrderStateMap(); }
    }
}

public class OrderStateDbContextDesignTimeFactory : IDesignTimeDbContextFactory<OrderStateDbContext>
{
    static string env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
    IConfiguration configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{env}.json", optional: false, reloadOnChange: true)
            .Build();

    public OrderStateDbContext CreateDbContext(string[] args)
    {
        var builder = new DbContextOptionsBuilder<OrderStateDbContext>();
        builder.UseSqlServer(configuration.GetConnectionString("SqlConnection"));
        return new OrderStateDbContext(builder.Options);
    }
}