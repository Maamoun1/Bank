using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Entities
{
    public  partial class TbRole
    {

        public int RoleId {  get; set; }
        public string RoleName { get; set; } = null!;
        public bool IsActive {  get; set; }

        public virtual ICollection<TbUserRole> UserRoles { get; set; } = new List<TbUserRole>();
    }
}
