using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakeMe.Core.DTOs.GenericRepositriesDTO
{
    public record AddCompanyDTO
    ([Required]string name,[Required]string description);
    public record UpdateCompanyDTO([Required] string name, [Required] string description);
}
