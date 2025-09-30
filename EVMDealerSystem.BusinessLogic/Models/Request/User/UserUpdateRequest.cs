using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVMDealerSystem.BusinessLogic.Models.Request.User
{
    public class UserUpdateRequest
    {
        public int? RoleId { get; set; }

        public Guid? DealerId { get; set; }

        public bool? IsActive { get; set; }
    }
}
