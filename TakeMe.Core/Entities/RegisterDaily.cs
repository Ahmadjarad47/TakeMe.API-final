using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakeMe.Core.Entities
{

    public class RegisterDaily : BaseEntity<int>
    {
        [Required]
        [DisplayName("Bus Name")]
        public string BusName { get; set; }
        [DisplayName("Name of street")]
        [Required]
        public string NameOfstreet { get; set; }
        [Required]
        [DisplayName("Name Of Collage")]
        public string NameOfCollage { get; set; }
        [DisplayName("Date And Time Of Register")]
        public string TimeOfRegister { get; set; }
        [Required]
        public double price { get; set; }
        public string AppUserId { get; set; }
        [ForeignKey(nameof(AppUserId))]
        public virtual AppUsers AppUsers { get; set; }

    }
}
