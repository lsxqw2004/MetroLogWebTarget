using System.Data.Entity.ModelConfiguration;
using MetroLogWebTarget.Domain;

namespace MetroLogWebTarget.Data.Mappings
{
    public partial class LogEventMap : EntityTypeConfiguration<LogEvent>
    {
        public LogEventMap()
        {
            this.ToTable("LogEvent");
            this.HasKey(l => l.Id);
            this.Property(l => l.BatId).IsRequired();
            this.Property(l => l.Message).HasMaxLength(200);

            //this.Ignore(l => l);

        }
    }
}