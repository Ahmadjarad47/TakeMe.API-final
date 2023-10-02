using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakeMe.Core.Entities
{
    public class Copone:BaseEntity<int>
    {
        public string NameCopone { get; set; }
        public string discound { get; set; }
        public int companyId { get; set; }
        [ForeignKey(nameof(companyId))]
        public virtual Company Company { get; set; }
    }
}
