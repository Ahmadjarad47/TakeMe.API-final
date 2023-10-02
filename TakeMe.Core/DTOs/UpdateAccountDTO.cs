using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakeMe.Core.DTOs
{
    public record UpdateAccountDTO
  (
      [Required]
        string UserName,
      [Required]
        string PhoneNumber,
      [Required]
        string CollageName,
      [EmailAddress]
        string Email,
      [Required]
        string password
  );
}
