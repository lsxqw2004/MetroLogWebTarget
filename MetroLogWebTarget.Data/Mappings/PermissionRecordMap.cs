using System.Data.Entity.ModelConfiguration;
using MetroLogWebTarget.Domain;

namespace MetroLogWebTarget.Data.Mappings
{
    public class PermissionRecordMap:EntityTypeConfiguration<PermissionRecord>
    {
        public PermissionRecordMap()
        {
            this.ToTable("PermissionRecord");
            this.HasKey(pr => pr.Id);

            this.Property(pr => pr.Name).IsRequired();
            this.Property(pr => pr.SystemName).IsRequired().HasMaxLength(255);
            this.Property(pr => pr.Category).IsRequired().HasMaxLength(255);

            this.HasMany(pr => pr.Roles)
                .WithMany(r => r.PermissionRecords)
                .Map(m => m.ToTable("PermissionRecordMapping"));
        }
    }
}
