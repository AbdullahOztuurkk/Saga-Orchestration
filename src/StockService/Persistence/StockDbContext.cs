using CoreLib.DataAccess.Concrete;
using Domain.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Persistence;
public class StockDbContext : DbContextBase
{
    public StockDbContext(DbContextOptions options) : base(options)
    {
    }
    public DbSet<Stock> Stocks { get; set; }
}

public class StockDbContextDesignTimeFactory : IDesignTimeDbContextFactory<StockDbContext>
{
    static string env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
    
    IConfiguration configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{env}.json", optional: false, reloadOnChange: true)
            .Build();

    public StockDbContext CreateDbContext(string[] args)
    {
        var builder = new DbContextOptionsBuilder<StockDbContext>();
        builder.UseSqlServer(configuration.GetConnectionString("SqlConnection"));
        return new StockDbContext(builder.Options);
    }
}