using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakeMe.Core.Interfaces
{
    public interface IUnitOfWork
    {
        public ICompanies Companies { get; }
        public IPostes Postes { get; }
        public IRegisterDaily RegisterDaily { get; }
        public IQrCodeReader IQrCodeReader { get; }

    }
}
