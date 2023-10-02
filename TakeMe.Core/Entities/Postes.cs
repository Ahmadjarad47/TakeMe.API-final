using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakeMe.Core.Entities
{
    public class Postes:BaseEntity<int>
    {
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
    }
}
