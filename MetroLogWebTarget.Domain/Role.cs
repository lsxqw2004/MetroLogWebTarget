using System.Collections.Generic;
using Microsoft.AspNet.Identity;

namespace MetroLogWebTarget.Domain
{

    public partial class Role : BaseEntity, IRole<int>
    {
        private ICollection<PermissionRecord> _permissionRecords;

        public string Name { get; set; }

        public bool Active { get; set; }

        public bool IsSystemRole { get; set; }

        public string SystemName { get; set; }

        public virtual ICollection<PermissionRecord> PermissionRecords
        {
            get { return _permissionRecords ?? (_permissionRecords = new List<PermissionRecord>()); }
            protected set { _permissionRecords = value; }
        }
    }

}