using Application.Services.Concrete;
using Autofac;
using CoreLib.DataAccess;
using Persistence;
using Web.API.Controllers;

namespace Order.API.Configurations;

public class OrderModule : Module
{
    private readonly IConfiguration configuration;
    public OrderModule(IConfiguration configuration)
    {
        this.configuration = configuration;
    }
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterAssemblyTypes(typeof(MainApiController).Assembly).Where(t => t.IsSubclassOf(typeof(MainApiController))).PropertiesAutowired();

        builder.RegisterAssemblyTypes(typeof(BaseService).Assembly)
            .Where(t => t.Name.EndsWith("Service"))
            .AsImplementedInterfaces()
            .PropertiesAutowired()
            .InstancePerLifetimeScope();

        builder.AddUnitOfWorkContext<OrderDbContext>(configuration.GetConnectionString("SqlConnection"));
    }
}
