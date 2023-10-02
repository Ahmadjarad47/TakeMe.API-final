using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TakeMe.Core.Entities;
using TakeMe.Core.Interfaces;
using TakeMe.InferStructuer.Data;

namespace TakeMe.InferStructuer.Repositries
{
    public class QrCodeReaderRepositry : GenericRepositrie<QRCodeCheck>, IQrCodeReader
    {
        private readonly ApplicationDbContext context;

        public QrCodeReaderRepositry(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }

       
    }
}
