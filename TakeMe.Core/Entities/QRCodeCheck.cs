using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakeMe.Core.Entities
{
    public class QRCodeCheck : BaseEntity<int>
    {
        public string MyQRCode { get; set; }
        public string Name { get; set; }
        public bool CheckedGo { get; set; }
        public bool CheckedReturn { get; set; }
        public int CompanyId { get; set; }
        [ForeignKey(nameof(CompanyId))]
        public virtual Company company { get; set; }

        public string AppUserId { get; set; }
        [ForeignKey(nameof(AppUserId))]
        public virtual AppUsers AppUsers { get; set; }
    }
}
