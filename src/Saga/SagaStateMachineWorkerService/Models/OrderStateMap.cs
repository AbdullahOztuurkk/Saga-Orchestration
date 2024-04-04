using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SagaStateMachineWorkerService.Data;

namespace SagaStateMachineWorkerService.Models;

/// <summary>
/// OrderStateMap class is the configuration of database entities.
/// It can added Configurations method from its own DbContext.
/// </summary>
public class OrderStateMap : SagaClassMap<OrderStateInstance>
{
    protected override void Configure(EntityTypeBuilder<OrderStateInstance> entity, ModelBuilder model)
    {
        entity.Property(x => x.BuyerId).HasMaxLength(256);
    }
}
