using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakeMe.Core.DTOs
{
    public record AddPostDTO
    ([Required]
        DateTime TimeRegister,
        [Required]
        DateTime TimeRerturn,
        [Required]
            string street,
        [Required]
        double phoneNumber
        , [Required]
        int companyId

    );

}
/*
   [Required]
        public DateTime Timetogo { get; set; }
        [Required]
        public DateTime Returntime  { get; set; }
        public string street { get; set; }
        [Required,Range(0,10,ErrorMessage = "You must enter ten numbers")]
        public double phoneNumber { get; set; }
        public int companyId { get; set; }
        [ForeignKey(nameof(companyId))]
        public virtual Company Company { get; set; }
 
 */