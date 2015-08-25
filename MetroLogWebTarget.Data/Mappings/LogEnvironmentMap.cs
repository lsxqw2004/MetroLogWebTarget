using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetroLogWebTarget.Domain;

namespace MetroLogWebTarget.Data.Mappings
{
    public class LogEnvironmentMap:EntityTypeConfiguration<LogEnvironment>
    {
        public LogEnvironmentMap()
        {
            ToTable("LogEnvironment");
            HasKey(le => le.Id);

            HasMany(le => le.Events).WithRequired().HasForeignKey(e => e.LogEnvironmentId);
        }
    }
}
