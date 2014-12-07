using System.Collections.Generic;
using Microsoft.AspNet.Identity;

namespace MetroLogWebTarget.Domain
{
    public class User : BaseEntity, IUser<int>
    {
        public bool IsSystemAccount { get; set; }
        
        public string UserName { get; set; }

        public virtual string PasswordHash { get; set; }

        public virtual string SecurityStamp { get; set; }

        private ICollection<UserRole> _userRoles;
        
        public virtual ICollection<UserRole> UserRoles
        {
            get { return _userRoles ?? (_userRoles = new List<UserRole>()); }
            protected set { _userRoles = value; }
        }

        private ICollection<UserClaim> _userClaims; 

        public virtual ICollection<UserClaim> Claims
        {
            get { return _userClaims ?? (_userClaims = new List<UserClaim>()); }
        }
    }
}
