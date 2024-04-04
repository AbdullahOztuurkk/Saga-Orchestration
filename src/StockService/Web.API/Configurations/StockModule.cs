using Application.Services.Concrete;
using Autofac;
using CoreLib.DataAccess;
using Persistence;
using Web.API.Controllers;

namespace Web.API.Configurations;

public class StockModule : Module
{
    private readonly IConfiguration configuration;

    public StockModule(IConfiguration configuration)
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

        builder.AddUnitOfWorkContext<StockDbContext>(configuration.GetConnectionString("SqlConnection"));
    }
}
