using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVMDealerSystem.BusinessLogic.Models.Request.User
{
    public class AdminUserRegistrationRequest
    {
        public string Email { get; set; } = null!;
        public string FullName { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string RoleName { get; set; } = string.Empty;

        public Guid? DealerId { get; set; }

        public bool IsActive { get; set; }
    }
}
