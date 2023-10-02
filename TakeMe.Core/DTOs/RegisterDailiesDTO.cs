using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakeMe.Core.DTOs
{
    public record RegisterDailiesDTO
    (
         [Required]
        string BusName,

         [Required]
        string NameOfstreet,
         [Required][EmailAddress]
        string EmailAddress,
         [Required]
        string NameOfCollage,

         [Required]
        DateTime TimeOfRegister,


       double price,

         [Required]
        string TimeGo

    );
}
