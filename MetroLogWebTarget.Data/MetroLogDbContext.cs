using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using MetroLogWebTarget.Domain;

namespace MetroLogWebTarget.Data
{
    public class MetroLogDbContext :DbContext, IDbContext
    {
        public MetroLogDbContext(): base("DefaultConnection")
        {
            
        }

        public DbSet<LogEnvironment> LogEnvironments { get; set; }
        public DbSet<LogEvent> LogEvents { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Database does not pluralize table names
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();


            Database.SetInitializer(new SqliteDbInitializer(modelBuilder));
        }

        public IDbSet<TEntity> Set<TEntity>() where TEntity : BaseEntity
        {
            return base.Set<TEntity>();
        }


        public static MetroLogDbContext Create()
        {
            return new MetroLogDbContext();
        }
    }

}
