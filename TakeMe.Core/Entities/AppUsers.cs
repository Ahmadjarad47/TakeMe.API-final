using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakeMe.Core.Entities
{
    public class AppUsers : IdentityUser
    {
        public DateTime TimeRegister { get; set; } = DateTime.Now;
        public string refreshToken { get; set; }
        public DateTime refreshTokenTime { get; set; }
        public string Token { get; set; }
        public DateTime TokenTime { get; set; }
        public string ResetPasswordToken { get; set; }

        public DateTime ResetPasswordTokenExpier { get; set; }
        public string CollageName { get; set; }
    }
}
