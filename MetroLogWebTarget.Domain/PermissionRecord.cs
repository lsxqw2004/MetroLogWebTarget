using System.Collections.Generic;

namespace MetroLogWebTarget.Domain
{
    public class PermissionRecord : BaseEntity
    {
        private ICollection<Role> _userRoles;

        public string Name { get; set; }

        public string SystemName { get; set; }
        
        public string Category { get; set; }
        
        public virtual ICollection<Role> Roles
        {
            get { return _userRoles ?? (_userRoles = new List<Role>()); }
            protected set { _userRoles = value; }
        }   
    }
}
