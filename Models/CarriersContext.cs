using carriersApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CarriersAPI.Models
{
    public class CarriersContext : DbContext
    {
        public DbSet<Carrier> Carriers { get; set; }
        public DbSet<CarrierConfiguration> CarrierConfigurations { get; set; }
        public DbSet<Orders> Orders{ get; set; }

        public CarriersContext(DbContextOptions<CarriersContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            
        }
    }
}
