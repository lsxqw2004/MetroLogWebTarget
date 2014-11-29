using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using MetroLogWebTarget.Domain;

namespace MetroLogWebTarget.Data
{
    public class MetroLogContext : DbContext, IDbContext
    {
        public MetroLogContext()
        {
            // Turn off the Migrations, (NOT a code first Db)
            Database.SetInitializer<MetroLogContext>(null);
        }

        public DbSet<LogEnvironment> LogEnvironments { get; set; }
        public DbSet<LogEvent> LogEvents { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Database does not pluralize table names
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

        public IDbSet<TEntity> Set<TEntity>() where TEntity : BaseEntity
        {
            return base.Set<TEntity>();
        }
    } 
}
