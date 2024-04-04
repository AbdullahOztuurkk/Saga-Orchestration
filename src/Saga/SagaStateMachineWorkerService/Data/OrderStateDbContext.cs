using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;
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
