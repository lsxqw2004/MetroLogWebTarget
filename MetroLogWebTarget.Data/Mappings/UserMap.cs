using System.Data.Entity.ModelConfiguration;
using MetroLogWebTarget.Domain;

namespace MetroLogWebTarget.Data.Mappings
{
    public class UserMap : EntityTypeConfiguration<User>
    {
        public UserMap()
        {
            this.ToTable("Users");
            this.HasKey(pr => pr.Id);

            this.Property(u => u.UserName).HasMaxLength(100);
            this.Property(u => u.Email).HasMaxLength(100);

            this.HasMany(c => c.Roles)
                .WithMany()
                .Map(m => m.ToTable("UserRoleMapping"));
        }
    }
}
