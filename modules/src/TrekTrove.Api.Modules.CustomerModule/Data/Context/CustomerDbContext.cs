using Microsoft.EntityFrameworkCore;

namespace TrekTrove.Api.Modules.CustomerModule.Data.Context
{
    public class CustomerDbContext : DbContext
    {
        public CustomerDbContext(DbContextOptions<CustomerDbContext> options)
            : base(options)
        {
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder builder)
        {
            base.ConfigureConventions(builder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            OnSaveChanges();
            return base.SaveChangesAsync(cancellationToken);
        }
        private void OnSaveChanges()
        {
        }
    }
}
