using System.Data.Entity.ModelConfiguration;
using MetroLogWebTarget.Domain;

namespace MetroLogWebTarget.Data.Mappings
{
    public class RoleMap:EntityTypeConfiguration<Role>
    {
        public RoleMap()
        {
            this.ToTable("Role");
            this.HasKey(r => r.Id);
            this.Property(r => r.Name).IsRequired().HasMaxLength(255);
            this.Property(r => r.SystemName).HasMaxLength(255);
        }
    }
}
