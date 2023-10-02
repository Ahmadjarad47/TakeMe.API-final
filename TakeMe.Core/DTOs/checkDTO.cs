using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakeMe.Core.DTOs
{
    public class checkDTO
    {
        public string code { get; set; }
        [EmailAddress]
        public string email { get; set; }
        public string password { get; set; }

    }
}
