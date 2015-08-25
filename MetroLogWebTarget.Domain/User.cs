using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace MetroLogWebTarget.Domain
{
    public class User : BaseEntity, IUser<int>
    {
        public bool IsSystemAccount { get; set; }
        
        public string UserName { get; set; }

        public virtual string PasswordHash { get; set; }

        public virtual string SecurityStamp { get; set; }

        private ICollection<Role> _userRoles;
        
        public virtual ICollection<Role> Roles
        {
            get { return _userRoles ?? (_userRoles = new List<Role>()); }
            protected set { _userRoles = value; }
        }

        private ICollection<UserClaim> _userClaims; 

        public virtual ICollection<UserClaim> Claims
        {
            get { return _userClaims ?? (_userClaims = new List<UserClaim>()); }
        }

        public string Email { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User,int> manager)
        {
            // 请注意，authenticationType 必须与 CookieAuthenticationOptions.AuthenticationType 中定义的相应项匹配
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // 在此处添加自定义用户声明
            return userIdentity;
        }
    }
}
